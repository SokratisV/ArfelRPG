using System;
using System.Collections.Generic;
using Core.Interfaces;
using RPG.Combat;
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

		/// <summary>
		/// true = requires target, false = requires point, null = self cast
		/// </summary>
		public bool? SkillRequiresTarget => _selectedSkill.RequiresTarget;

		public bool IsPreparingSkill => _selectedSkill != null;
		public bool CanCurrentSkillBeUsed => _selectedSkill != null && !IsSkillOnCooldown(_selectedSkill);

		private bool _activeListCleanup, _cooldownListCleanup;
		private float _globalCooldownTimer;
		private Vector3? _targetPoint;
		private GameObject _target;
		private Mover _mover;
		private Skill _selectedSkill;
		private Animator _animator;
		private ActionScheduler _actionScheduler;
		private CastingSkill _currentCastingSkill;
		private List<CooldownSkill> _skillsOnCooldown = new List<CooldownSkill>();
		private List<ActivatedSkill> _activatedSkills = new List<ActivatedSkill>();
		private Dictionary<int, Skill> _learnedSkills = new Dictionary<int, Skill>();

		public static SkillUser GetPlayerSkills() => PlayerFinder.Player.GetComponent<SkillUser>();
		private static SkillNamesAndIds SkillsDatabase;
		private static readonly int SkillAnimationHash = Animator.StringToHash("skill");
		private static readonly int SkillAnimationExtraHash = Animator.StringToHash("skillExtra");

		#region Unity

		private void Awake()
		{
			if(SkillsDatabase == null)
			{
				SkillsDatabase = Resources.Load<SkillNamesAndIds>("SkillDatabase");
			}

			_mover = GetComponent<Mover>();
			_actionScheduler = GetComponent<ActionScheduler>();
			_animator = GetComponent<Animator>();
		}


		private void OnEnable()
		{
			GetComponent<Fighter>().OnWeaponChanged += SwapSkills;
			OnSkillCast += UseSkillAnimation;
		}

		private void OnDisable()
		{
			GetComponent<Fighter>().OnWeaponChanged -= SwapSkills;
			OnSkillCast -= UseSkillAnimation;
		}

		private void Update()
		{
			SkillListCleanup();
			UpdateCooldowns();
			UpdateActiveSkills();
			UpdateCastingSkill();
			MoveToCast();
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

		public void Execute(GameObject target)
		{
			if(_selectedSkill.MoveInRangeBeforeCasting)
			{
				_target = target;
				_actionScheduler.StartAction(this);
			}
			else
			{
				UseSelectedSkill(target, null);
			}
		}

		public void Execute(Vector3 hitPoint)
		{
			if(_selectedSkill.MoveInRangeBeforeCasting)
			{
				_targetPoint = hitPoint;
				_actionScheduler.StartAction(this);
			}
			else
			{
				UseSelectedSkill(null, hitPoint);
			}
		}

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
			_target = null;
			_targetPoint = null;
		}

		public void CompleteAction()
		{
			OnActionComplete?.Invoke();
			_actionScheduler.CompleteAction();
		}

		public void ExecuteQueuedAction(IActionData data) => throw new NotImplementedException();

		public bool CanExecute(Vector3 target)
		{
			if(_selectedSkill.MinClickDistance > 0 && Helper.IsWithinDistance(target, transform.position, _selectedSkill.MinClickDistance))
			{
				return false;
			}

			return _mover.CanMoveTo(target);
		}

		//if is within range or if can self target
		public bool CanExecute(GameObject target)
		{
			if(!CanExecute(target.transform.position)) return false;
			return _selectedSkill.CanTargetSelf || gameObject != target;
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

		#endregion

		#region Private

		private void MoveToCast()
		{
			if(_targetPoint != null)
			{
				if(_mover.IsInRange(_targetPoint.Value, _selectedSkill.CastingRange))
				{
					_mover.CancelAction();
					UseSelectedSkill(null, _targetPoint);
				}
				else
				{
					_mover.MoveWithoutAction(_targetPoint.Value);
				}
			}
			else if(_target != null)
			{
				if(_mover.IsInRange(_target.transform, _selectedSkill.CastingRange))
				{
					_mover.CancelAction();
					UseSelectedSkill(_target, null);
				}
				else
				{
					_mover.MoveWithoutAction(_target.transform.position);
				}
			}
		}

		private void UpdateCastingSkill()
		{
			if(_currentCastingSkill != null)
			{
				if(_currentCastingSkill.Update())
				{
					OnSkillEnd?.Invoke(_currentCastingSkill.Skill);
					OnActionComplete?.Invoke();
					_currentCastingSkill = null;
					_selectedSkill = null;
					_target = null;
					_targetPoint = null;
				}
			}
		}

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

			OnSkillCast?.Invoke(_selectedSkill);
			_skillsOnCooldown.Add(new CooldownSkill(_selectedSkill));
			if(_selectedSkill.HasCastTime)
			{
				_currentCastingSkill = new CastingSkill(_selectedSkill, data);
			}
			else
			{
				_activatedSkills.Add(new ActivatedSkill(_selectedSkill, data));
				OnActionComplete?.Invoke();
				_selectedSkill = null;
				_target = null;
				_targetPoint = null;
			}

			_globalCooldownTimer = GlobalValues.GlobalCooldown;
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

			// _audioPlayer.PlaySound(cooldownAudio);
			return false;
		}

		private bool CanSkillBeUsed(Skill skill)
		{
			if(_globalCooldownTimer > 0) return false;
			return!IsSkillOnCooldown(skill);
		}

		private void UseSkillAnimation(Skill selectedSkill)
		{
			if(selectedSkill.HasExtraAnimation) _animator.SetTrigger(SkillAnimationExtraHash);
			_animator.SetInteger(SkillAnimationHash, selectedSkill.AnimationHash);
		}

		private void SwapSkills(WeaponConfig config)
		{
			foreach(var skill in _learnedSkills)
			{
				RemoveSkill(skill.Key);
			}

			foreach(var activatedSkill in _activatedSkills)
			{
				activatedSkill.HasEnded = true; //temp until I have to end it forcefully
			}

			for(var index = 0;index < config.SkillIds.Length;index++)
			{
				AddSkill(Skill.GetFromID(SkillsDatabase.GetSkillId(config.SkillIds[index])), index);
			}
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
			public bool HasEnded;
			public Skill Skill {get;}
			private float _timer;
			private readonly float _duration;
			private readonly SkillData _data;

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

		private class CastingSkill
		{
			public Skill Skill {get;}
			private float _timer;
			private readonly float _castTime;
			private readonly SkillData _data;

			public CastingSkill(Skill skill, SkillData data)
			{
				Skill = skill;
				_castTime = skill.Duration;
				_data = data;
				_timer = 0;
			}

			internal bool Update()
			{
				if(_castTime < 0) return false;
				if(_timer <= _castTime)
				{
					Skill.OnUpdate(_data);
					_timer += Time.deltaTime;
				}
				else
				{
					Skill.OnEnd(_data);
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
			private float _timer;

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