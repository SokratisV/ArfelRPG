using System;
using System.Collections.Generic;
using System.Linq;
using Core.Interfaces;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using UnityEngine;

namespace RPG.Skills
{
	public class SkillUser : MonoBehaviour, IAction, ISaveable, ISpellActionable
	{
		public event Action SkillsUpdated;
		public event Action OnActionComplete;
		public event Action<Skill> OnSkillCast, OnSkillEnd, OnSkillSelected;

		public bool IsPreparingSkill => _selectedSkill != null;

		/// <summary>
		/// true = requires target, false = requires point, null = self cast
		/// </summary>
		public bool? SkillRequiresTarget => _selectedSkill.RequiresTarget;

		public bool CanCurrentSkillBeUsed => _selectedSkill != null && !IsSkillOnCooldown(_selectedSkill);

		private bool _activeListCleanup = false, _cooldownListCleanup = false;
		private float _globalCooldownTimer = 0;
		private Mover _mover = null;
		private Skill _selectedSkill = null;
		private ActionScheduler _actionScheduler = null;
		private List<CooldownSkill> _skillsOnCooldown = new List<CooldownSkill>();
		private List<ActivatedSkill> _activatedSkills = new List<ActivatedSkill>();
		private Dictionary<int, Skill> _learnedSkills = new Dictionary<int, Skill>();

		public static SkillUser GetPlayerSkills() => PlayerFinder.Player.GetComponent<SkillUser>();

		#region Unity

		private void Awake()
		{
			_mover = GetComponent<Mover>();
			_actionScheduler = GetComponent<ActionScheduler>();
		}

		private void Start()
		{
			AddSkill(Skill.GetFromID("092f09f3-e273-4b47-aaf5-4483984a1cfa"), 0);
			AddSkill(Skill.GetFromID("5231d976-a9d9-4917-95fe-1e870c11bb3c"), 1);
			AddSkill(Skill.GetFromID("83bef455-e343-4538-97ce-79acdf2471e5"), 2);
		}

		private void Update()
		{
			SkillListCleanup();
			UpdateCooldowns();
			UpdateActiveSkills();
		}

		#endregion

		#region Public

		public void AddSkill(Skill skill, int index = -1)
		{
			if(index >= 0) AddToKnownIndex(skill, index);
			else AddToFirstAvailableSlot(skill);
			SkillsUpdated?.Invoke();
		}

		public void SelectSkill(int index)
		{
			if(_learnedSkills.TryGetValue(index, out var skill))
			{
				if(!CanSkillBeUsed(skill)) return;
				_selectedSkill = skill;
				OnSkillSelected?.Invoke(_selectedSkill);
			}
		}

		public void Execute() => UseSelectedSkill(null, null);

		public void Execute(GameObject target) => UseSelectedSkill(target, null);

		public void Execute(Vector3 hitPoint) => UseSelectedSkill(null, hitPoint);

		public void QueueExecution(GameObject target)
		{
			Debug.Log("Queue Skill action Not Yet Implemented");
			Execute(target);
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

		public void ExecuteQueuedAction(IActionData data) => throw new NotImplementedException();

		public bool CanExecute(GameObject o) => true;

		public void RemoveSkill(int index)
		{
			if(index > GlobalValues.ActionBarCount) return;
			if(_learnedSkills.ContainsKey(index))
			{
				_learnedSkills.Remove(index);
				SkillsUpdated?.Invoke();
			}
		}

		#endregion

		#region Private

		private void UpdateActiveSkills()
		{
			foreach(var activeSkill in _activatedSkills)
			{
				if(activeSkill.Update())
				{
					_activeListCleanup = true;
					OnSkillEnd?.Invoke(activeSkill.Skill);
				}
			}
		}

		private void UpdateCooldowns()
		{
			if(_globalCooldownTimer > 0) _globalCooldownTimer -= Time.deltaTime;
			foreach(var skill in _skillsOnCooldown)
			{
				if(skill.Update())
				{
					_cooldownListCleanup = true;
				}
			}
		}

		private void SkillListCleanup()
		{
			if(_activeListCleanup)
			{
				_activatedSkills.RemoveAll(activatedSkill => activatedSkill.HasEnded);
				_activeListCleanup = false;
			}

			if(_cooldownListCleanup)
			{
				_skillsOnCooldown.RemoveAll(cooldownSkill => cooldownSkill.HasCooledDown);
				_cooldownListCleanup = false;
			}
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

		private void UseSelectedSkill(GameObject target, Vector3? hitPoint)
		{
			if(IsSkillOnCooldown(_selectedSkill)) return;
			var data = _selectedSkill.OnStart(gameObject, target, hitPoint);
			if(data == null)
			{
				_selectedSkill = null;
				return;
			}

			_activatedSkills.Add(new ActivatedSkill(_selectedSkill, data));
			_skillsOnCooldown.Add(new CooldownSkill(_selectedSkill));
			OnSkillCast?.Invoke(_selectedSkill);
			_selectedSkill = null;
		}

		private bool IsSkillOnCooldown(Skill selectedSkill)
		{
			foreach(var skill in _skillsOnCooldown)
			{
				if(skill.Skill == selectedSkill) return true;
			}

			foreach(var skill in _activatedSkills)
			{
				if(skill.Skill == selectedSkill) return true;
			}

			return false;
		}

		private bool CanSkillBeUsed(Skill skill)
		{
			if(_globalCooldownTimer > 0) return false;
			foreach(var activeSkill in _activatedSkills)
			{
				if(activeSkill.Skill == skill)
				{
					return false;
				}
			}

			return true;
		}

		//Add more of its state, e.g cooldown
		[Serializable]
		private struct DockedItemRecord
		{
			public string skillID;
			public float remainingCooldown;
		}

		private class ActivatedSkill
		{
			public bool HasEnded = false;
			public Skill Skill {get;}
			private float _duration;
			private float _timer;
			private SkillData _data;


			//TODO: create new instance of skill here?
			public ActivatedSkill(Skill skill, SkillData data)
			{
				Skill = skill;
				_duration = skill.Duration;
				_data = data;
				_timer = 0;
			}

			internal bool Update()
			{
				if(_duration < 0) return false;
				if(_timer <= _duration)
				{
					Skill.OnUpdate(_data);
					_timer += Time.deltaTime;
				}
				else
				{
					Skill.OnEnd(_data);
					HasEnded = true;
					return true;
				}

				return false;
			}
		}

		private class CooldownSkill
		{
			public bool HasCooledDown;
			public Skill Skill {get;}
			private readonly float _cooldown;
			private float _timer = 0;

			internal bool Update()
			{
				if(_timer <= _cooldown)
				{
					_timer += Time.deltaTime;
				}
				else
				{
					HasCooledDown = true;
					return true;
				}

				return false;
			}

			public CooldownSkill(Skill skill)
			{
				_cooldown = skill.Cooldown;
				Skill = skill;
			}
		}

		#endregion
	}
}