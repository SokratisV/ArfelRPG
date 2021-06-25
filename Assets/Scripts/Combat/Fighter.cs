using System;
using Core.Interfaces;
using RPG.AnimatorBehaviors;
using RPG.Utils;
using RPG.Attributes;
using RPG.Core;
using RPG.Movement;
using RPG.Inventories;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{
	public class Fighter : MonoBehaviour, IAction, ISaveable, ICombatActionable
	{
		public event Action OnActionComplete;
		public event Action<WeaponConfig> OnWeaponChanged;

		public float AttackSpeed
		{
			get => _attackSpeed;
			private set => _attackSpeed = value;
		}

		[SerializeField] private WeaponConfig defaultWeapon = null;

		private bool CanAttack => _timeSinceLastAttack >= AttackSpeed;
		private bool _attackAnimationDone = true;
		private float _timeSinceLastAttack = GlobalValues.DefaultAttackSpeed;
		private float _attackSpeed;
		private BodyParts _bodyParts;
		private Mover _mover;
		private Health _target, _bufferedTarget;
		private BaseStats _stats;
		private Animator _animator;
		private Equipment _equipment;
		private ActionScheduler _actionScheduler;
		private LazyValue<Weapon> _currentWeapon;
		private WeaponConfig _currentWeaponConfig;

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
			if (_target == null) return;
			if (_target.IsDead)
			{
				CompleteAction();
				_target = null;
				return;
			}

			_timeSinceLastAttack += Time.deltaTime;
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
			if (!_attackAnimationDone)
			{
				_bufferedTarget = combatTarget.GetComponent<Health>();
			}
			else
			{
				_target = combatTarget.GetComponent<Health>();
			}

			_actionScheduler.StartAction(this);
		}
		
		public void QueueAction(IActionData data) {}

		public void QueueExecution(GameObject obj) => _actionScheduler.EnqueueAction(new FighterActionData(this, obj));

		public void CancelAction()
		{
			StopAttack();
			_mover.CancelAction();
			_target = null;
			_bufferedTarget = null;
		}

		public object CaptureState() => _currentWeaponConfig.name;

		public void RestoreState(object state)
		{
			_currentWeaponConfig = Resources.Load<WeaponConfig>($"Equipables/{(string) state}");
			EquipWeapon(_currentWeaponConfig);
			AttackSpeed = _currentWeaponConfig.AttackSpeed;
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
			if (_bufferedTarget != null)
			{
				_target = _bufferedTarget;
				_bufferedTarget = null;
			}

			_mover.RotateOverTime(.2f, _target.transform.position);
			if (!CanAttack) return;
			AttackAnimation();
		}

		private void AttackAnimation()
		{
			_timeSinceLastAttack = 0;
			_attackAnimationDone = false;
			_animator.ResetTrigger(StopAttackHash);
			_animator.SetTrigger(AttackHash);
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