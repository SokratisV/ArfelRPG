using System;
using System.Collections.Generic;
using Core.Interfaces;
using RPG.AnimatorBehaviors;
using RPG.Utils;
using RPG.Attributes;
using RPG.Core;
using RPG.Movement;
using RPG.Inventories;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{
	public class Fighter : MonoBehaviour, IAction, ICombatActionable
	{
		public event Action OnActionComplete;
		public event Action<WeaponConfig> OnWeaponChanged;

		public float AttackSpeed
		{
			get => _attackSpeed;
			private set => _attackSpeed = value;
		}

		[SerializeField] private WeaponConfig defaultWeapon = null;
		[SerializeField] private float autoAttackRange = 4f;

		private bool CanAttack => TimeSinceLastAttack >= AttackSpeed;
		private bool _attackAnimationDone = true;

		public float TimeSinceLastAttack
		{
			get => _timeSinceLastAttack;
			private set
			{
				_timeSinceLastAttack = value;
				if (_timeSinceLastAttack > AttackSpeed) _timeSinceLastAttack = AttackSpeed;
			}
		}

		private float _attackSpeed;
		private BodyParts _bodyParts;
		private Mover _mover;
		private Health _target;
		private BaseStats _stats;
		private Animator _animator;
		private Equipment _equipment;
		private ActionScheduler _actionScheduler;
		private LazyValue<Weapon> _currentWeapon;
		private WeaponConfig _currentWeaponConfig;
		private RaycastHit[] _autoTargetResults = new RaycastHit[5];
		private float _timeSinceLastAttack;

		private static readonly int StopAttackHash = Animator.StringToHash("stopAttack");
		private static readonly int AttackHash = Animator.StringToHash("attack");

		#region Unity

		private void Awake()
		{
			_currentWeaponConfig = defaultWeapon;
			_currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
			_animator = GetComponent<Animator>();
			_equipment = GetComponent<Equipment>();
			_actionScheduler = GetComponent<ActionScheduler>();
			_mover = GetComponent<Mover>();
			_stats = GetComponent<BaseStats>();
			_bodyParts = GetComponent<BodyParts>();
			var attackListenerBehavior = _animator.GetBehaviour<AttackAnimationInfo>();
			attackListenerBehavior.OnAnimationComplete += () => _attackAnimationDone = true;
			AttackSpeed = _currentWeaponConfig.AttackSpeed;
			if (_equipment) _equipment.EquipmentUpdated += UpdateWeapon;
		}

		private void Start() => _currentWeapon.ForceInit();

		private void Update()
		{
			TimeSinceLastAttack += Time.deltaTime;
			if (_target == null) return;
			if (_target.IsDead)
			{
				_target = FindNewTargetInRange();
				if (_target == null)
				{
					CompleteAction();
					return;
				}
			}

			if (_attackAnimationDone && _mover.IsInRange(_target.transform, _currentWeaponConfig.GetRange()))
			{
				_mover.CancelAction();
				Attack();
			}
			else
			{
				if (_attackAnimationDone) _mover.MoveWithoutAction(_target.transform.position);
			}
		}

		#endregion

		#region Public

		public Health GetTarget() => _target;

		public WeaponConfig GetWeaponConfig() => _currentWeaponConfig;

		public bool CanExecute(GameObject target)
		{
			if (target == null) return false;
			if (!_mover.CanMoveTo(target.transform.position) && !_mover.IsInRange(target.transform, _currentWeaponConfig.GetRange())) return false;
			var health = target.GetComponent<Health>();
			return health != null && !health.IsDead;
		}

		public bool CanExecute(Vector3 point) => throw new NotImplementedException();

		public void Execute(GameObject combatTarget)
		{
			_mover.CancelAction();
			_target = combatTarget.GetComponent<Health>();
			_actionScheduler.StartAction(this);
		}

		public void QueueAction(IActionData data)
		{
		}

		public void QueueExecution(GameObject obj) => _actionScheduler.EnqueueAction(new FighterActionData(this, obj));

		public void CancelAction()
		{
			StopAttack();
			_mover.CancelAction();
			_target = null;
		}

		public void CompleteAction()
		{
			OnActionComplete?.Invoke();
			_actionScheduler.CompleteAction();
		}

		public void ExecuteQueuedAction(IActionData data) => _target = ((FighterActionData) data).Target.GetComponent<Health>();

		#endregion

		#region Private

		[ContextMenu(nameof(UpdateWeapon))]
		private void UpdateWeapon()
		{
			var weapon = _equipment.GetItemInSlot(EquipLocation.Weapon) as WeaponConfig;
			EquipWeapon(!weapon ? defaultWeapon : weapon);
			AttackSpeed = _currentWeaponConfig.AttackSpeed;
		}

		private Weapon SetupDefaultWeapon() => AttachWeapon(defaultWeapon);

		private void EquipWeapon(WeaponConfig weapon)
		{
			_currentWeaponConfig = weapon;
			_currentWeapon.Value = AttachWeapon(weapon);
			if (_currentWeapon.Value.ProjectileLocation)
			{
				_bodyParts.ProjectileLocation = _currentWeapon.Value.ProjectileLocation;
			}
			else
			{
				_bodyParts.RevertProjectileLocation();
			}

			OnWeaponChanged?.Invoke(_currentWeaponConfig);
		}

		private Weapon AttachWeapon(WeaponConfig weapon) => weapon.Spawn(_bodyParts, _animator);

		private void Attack()
		{
			_mover.RotateToTarget(.2f, _target.transform.position);
			if (!CanAttack) return;
			AttackAnimation();
		}

		private void AttackAnimation()
		{
			TimeSinceLastAttack = 0;
			_attackAnimationDone = false;
			_animator.ResetTrigger(StopAttackHash);
			_animator.SetTrigger(AttackHash);
		}

		private Health FindNewTargetInRange()
		{
			Health best = null;
			var bestDistance = Mathf.Infinity;
			foreach (var candidate in FindTargetsInRange())
			{
				float candidateDistance = Vector3.Distance(transform.position, candidate.transform.position);
				if (candidateDistance < bestDistance)
				{
					best = candidate;
					bestDistance = candidateDistance;
				}
			}

			return best;
		}

		private IEnumerable<Health> FindTargetsInRange()
		{
			Physics.SphereCastNonAlloc(transform.position, autoAttackRange, Vector3.up, _autoTargetResults);
			foreach (var hit in _autoTargetResults)
			{
				if (hit.transform == null) continue;
				if (!hit.transform.TryGetComponent(out Health health)) continue;
				if (health == null || health.IsDead || health.gameObject == gameObject) continue;
				yield return health;
			}
		}

		// Animation Event
		private void Hit()
		{
			var damage = _stats.GetStat(Stat.Damage);
			if (_currentWeapon.Value != null) _currentWeapon.Value.OnHit();

			if (_target == null) return;
			if (_currentWeaponConfig.HasProjectile())
			{
				_currentWeaponConfig.LaunchProjectile(_bodyParts.ProjectileLocation.position, _target, gameObject, damage);
			}
			else
			{
				_target.TakeDamage(gameObject, damage);
			}
		}

		// Animation Event (visuals only)
		private void OnHit()
		{
			if (_currentWeapon.Value != null) _currentWeapon.Value.OnHit();
		}

		private void StopAttack()
		{
			_animator.ResetTrigger(AttackHash);
			_animator.SetTrigger(StopAttackHash);
		}

		#endregion
	}
}