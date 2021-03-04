using System;
using System.Collections.Generic;
using System.Linq;
using RPG.Core;
using RPG.Saving;
using RPG.Skills;
using UnityEngine;

namespace RPG.Skills
{
	public class SkillUser : MonoBehaviour, ISaveable
	{
		public event Action SkillsUpdated;

		private List<ActivatedSkill> _activedSkills = new List<ActivatedSkill>();
		private Dictionary<int, Skill> _learnedSkills = new Dictionary<int, Skill>();
		private float _globalCooldownTimer = 0;
		public Skill skill;

		public static SkillUser GetPlayerSkills() => PlayerFinder.Player.GetComponent<SkillUser>();

		private void Start()
		{
			AddSkill(skill, 0);
			UseSkill(0, gameObject);
		}

		private void Update()
		{
			if(_globalCooldownTimer > 0) _globalCooldownTimer -= Time.deltaTime;
			foreach(var activeSkill in _activedSkills)
			{
				activeSkill.Update();
			}
		}

		private class ActivatedSkill
		{
			private float _duration;
			private float _timer;
			private List<ActivatedSkill> _activeSkills;

			public Skill Skill {get;}

			public ActivatedSkill(Skill skill, List<ActivatedSkill> activeSkills, float duration)
			{
				_duration = duration;
				Skill = skill;
				_activeSkills = activeSkills;
				_timer = 0;
			}

			internal void Update()
			{
				if(_duration < 0) return;
				if(_timer <= _duration)
				{
					Skill.OnUpdate();
					_timer -= Time.deltaTime;
				}
				else _activeSkills.Remove(this);
			}
		}

		public void AddSkill(Skill skill, int index = -1)
		{
			if(index >= 0) AddToKnownIndex(skill, index);
			else AddToFirstAvailableSlot(skill);
			SkillsUpdated?.Invoke();
		}

		private void AddToFirstAvailableSlot(Skill skill)
		{
			for(var i = 0;i < GlobalValues.ActionBarCount;i++)
			{
				if(_learnedSkills.ContainsKey(i)) continue;
				AddToKnownIndex(skill, i);
				return;
			}
		}

		private void AddToKnownIndex(Skill skill, int index) => _learnedSkills[index] = skill;

		public void UseSkill(int index, GameObject user)
		{
			if(_learnedSkills.TryGetValue(index, out var skill))
			{
				if(!CanSkillBeUsed(skill)) return;
				_activedSkills.Add(new ActivatedSkill(skill, _activedSkills, skill.Duration));
				skill.OnStart(user);
			}
		}

		private bool CanSkillBeUsed(Skill skill)
		{
			if(_globalCooldownTimer > 0) return false;
			foreach(var activeSkill in _activedSkills)
			{
				if(activeSkill.Skill == skill)
				{
					return false;
				}
			}

			return true;
		}

		public void RemoveSkill(int index)
		{
			if(index > GlobalValues.ActionBarCount) return;
			if(_learnedSkills.ContainsKey(index))
			{
				_learnedSkills.Remove(index);
				SkillsUpdated?.Invoke();
			}
		}

		//Add more of its state, e.g cooldown
		[Serializable]
		private struct DockedItemRecord
		{
			public string skillID;
		}

		public object CaptureState()
		{
			var state = new Dictionary<int, DockedItemRecord>();
			foreach(var pair in _learnedSkills)
			{
				var record = new DockedItemRecord {skillID = pair.Value.SkillID};
				state[pair.Key] = record;
			}

			return state;
		}

		public void RestoreState(object state)
		{
			var stateDict = (Dictionary<int, DockedItemRecord>)state;
			foreach(var pair in stateDict)
			{
				AddSkill(Skill.GetFromID(pair.Value.skillID), pair.Key);
			}
		}

		public Skill GetSkill(int index) => _learnedSkills.TryGetValue(index, out var skill)? skill:null;
	}
}