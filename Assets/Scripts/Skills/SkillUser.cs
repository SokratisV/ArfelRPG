using System;
using System.Collections.Generic;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using UnityEngine;

namespace RPG.Skills
{
	public class SkillUser : MonoBehaviour, IAction, ISaveable
	{
		public event Action SkillsUpdated;
		public event Action OnActionComplete;

		private List<ActivatedSkill> _activedSkills = new List<ActivatedSkill>();
		private Dictionary<int, Skill> _learnedSkills = new Dictionary<int, Skill>();
		private Skill _selectedSkill = null;
		private ActionScheduler _actionScheduler = null;
		private Mover _mover = null;
		public bool IsPreparingSkill => _selectedSkill != null;
		public bool SkillRequiresTarget => _selectedSkill.RequiresTarget;
		private float _globalCooldownTimer = 0;
		public Skill skill;

		public static SkillUser GetPlayerSkills() => PlayerFinder.Player.GetComponent<SkillUser>();

		private void Awake()
		{
			_mover = GetComponent<Mover>();
			_actionScheduler = GetComponent<ActionScheduler>();
		}

		private void Start()
		{
			AddSkill(skill, 0);
			SelectSkill(0);
			// UseSelectedSkill();
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
			private SkillData _data;

			public Skill Skill {get;}

			//TODO: create new instance of skill here?
			public ActivatedSkill(Skill skill, List<ActivatedSkill> activeSkills, SkillData data, float duration)
			{
				_duration = duration;
				Skill = skill;
				_activeSkills = activeSkills;
				_data = data;
				_timer = 0;
			}

			internal void Update()
			{
				if(_duration < 0) return;
				if(_timer <= _duration)
				{
					Skill.OnUpdate(_data);
					_timer -= Time.deltaTime;
				}
				else
				{
					Skill.OnEnd(_data);
					_activeSkills.Remove(this);
				}
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

		public void SelectSkill(int index)
		{
			if(_learnedSkills.TryGetValue(index, out var skill))
			{
				if(!CanSkillBeUsed(skill)) return;
				_selectedSkill = skill;
			}
		}

		public void UseSelectedSkill()
		{
			var data = _selectedSkill.OnStart(gameObject);
			_activedSkills.Add(new ActivatedSkill(_selectedSkill, _activedSkills, data, _selectedSkill.Duration));
			_selectedSkill = null;
		}

		public void UseSelectedSkill(GameObject target)
		{
			var data = _selectedSkill.OnStart(gameObject, target);
			_activedSkills.Add(new ActivatedSkill(_selectedSkill, _activedSkills, data, _selectedSkill.Duration));
			_selectedSkill = null;
		}

		public void UseSelectedSkill(Vector3 hitPoint)
		{
			var data = _selectedSkill.OnStart(gameObject, null, hitPoint);
			_activedSkills.Add(new ActivatedSkill(_selectedSkill, _activedSkills, data, _selectedSkill.Duration));
			_selectedSkill = null;
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

		public void CancelAction()
		{
			_mover.CancelAction();
			_selectedSkill = null;
		}

		public void CompleteAction()
		{
			OnActionComplete?.Invoke();
			_actionScheduler.CompleteAction();
		}

		public void ExecuteAction(IActionData data)
		{
			throw new NotImplementedException();
		}

		public bool CanCast(GameObject o)
		{
			return true;
		}
	}
}