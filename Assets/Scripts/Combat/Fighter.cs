using System;
using RPG.Utils;
using RPG.AnimatorBehaviors;
using RPG.Attributes;
using RPG.Core;
using RPG.Movement;
using RPG.Inventories;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{
	public class Fighter : MonoBehaviour, IAction, ISaveable
	{
		public event Action OnActionComplete;

		[SerializeField] private float timeBetweenAttacks = 1f;
		[SerializeField] private Transform rightHandTransform = null;
		[SerializeField] private Transform leftHandTransform = null;
		[SerializeField] private WeaponConfig defaultWeapon = null;

		private WeaponConfig _currentWeaponConfig;
		private Equipment _equipment;
		private Mover _mover;
		private ActionScheduler _actionScheduler;
		private Animator _animator;
		private LazyValue<Weapon> _currentWeapon;
		private Health _target;
		private BaseStats _stats;

		private float _timeSinceLastAttack = Mathf.Infinity;

		// private bool _isCurrentAnimationDone = true;
		private static readonly int StopAttackHash = Animator.StringToHash("stopAttack");
		private static readonly int AttackHash = Animator.StringToHash("attack");

		private void Start()
		{
			_currentWeapon.ForceInit();
			var attackSpeedBehaviors = _animator.GetBehaviours<RandomAttackAnimBehavior>();
			foreach(var behaviour in attackSpeedBehaviors)
			{
				behaviour.TimeBetweenAttacks = timeBetweenAttacks;
			}
		}

		private void Awake()
		{
			_currentWeaponConfig = defaultWeapon;
			_currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
			_animator = GetComponent<Animator>();
			_equipment = GetComponent<Equipment>();
			_actionScheduler = GetComponent<ActionScheduler>();
			_mover = GetComponent<Mover>();
			_stats = GetComponent<BaseStats>();
			//TODO: sometimes breaks and event is never fired
			// var attackListenerBehavior = _animator.GetBehaviour<AttackAnimationInfo>();
			// attackListenerBehavior.OnAnimationComplete += () => _isCurrentAnimationDone = true;
			if(_equipment) _equipment.EquipmentUpdated += UpdateWeapon;
		}

		[ContextMenu(nameof(UpdateWeapon))]
		private void UpdateWeapon()
		{
			var weapon = _equipment.GetItemInSlot(EquipLocation.Weapon) as WeaponConfig;
			EquipWeapon(!weapon? defaultWeapon:weapon);
		}

		private Weapon SetupDefaultWeapon() => AttachWeapon(defaultWeapon);

		private void EquipWeapon(WeaponConfig weapon)
		{
			_currentWeaponConfig = weapon;
			_currentWeapon.Value = AttachWeapon(weapon);
		}

		private Weapon AttachWeapon(WeaponConfig weapon) => weapon.Spawn(rightHandTransform, leftHandTransform, _animator);

		public Health GetTarget() => _target;

		public WeaponConfig GetWeaponConfig() => _currentWeaponConfig;

		private void Update()
		{
			_timeSinceLastAttack += Time.deltaTime;
			if(_target == null) return;
			if(_target.IsDead)
			{
				CompleteAction();
				_target = null;
				return;
			}

			if(_mover.IsInRange(_target.transform, _currentWeaponConfig.GetRange()))
			{
				_mover.CancelAction();
				Attack();
			}
			else
			{
				// if(_isCurrentAnimationDone)
				_mover.MoveWithoutAction(_target.transform.position);
			}
		}

		private void Attack()
		{
			transform.LookAt(_target.transform);
			if(!(_timeSinceLastAttack > timeBetweenAttacks)) return;
			AttackAnimation();
		}

		private void AttackAnimation()
		{
			// _isCurrentAnimationDone = false;
			_timeSinceLastAttack = 0;
			_animator.ResetTrigger(StopAttackHash);
			_animator.SetTrigger(AttackHash);
		}

		// Animation Event
		private void Hit()
		{
			var damage = _stats.GetStat(Stat.Damage);
			if(_currentWeapon.Value != null) _currentWeapon.Value.OnHit();

			if(_target == null) return;
			if(_currentWeaponConfig.HasProjectile())
			{
				_currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, _target, gameObject, damage);
			}
			else
			{
				_target.TakeDamage(gameObject, damage);
			}
		}

		private void Shoot() => Hit();

		public bool CanAttack(GameObject target)
		{
			if(target == null) return false;
			if(!_mover.CanMoveTo(target.transform.position) && !_mover.IsInRange(target.transform, _currentWeaponConfig.GetRange())) return false;
			var health = target.GetComponent<Health>();
			return health != null && !health.IsDead;
		}

		public void StartAttackAction(GameObject combatTarget)
		{
			_target ??= combatTarget.GetComponent<Health>();
			_actionScheduler.StartAction(this);
		}

		public void QueueAttackAction(GameObject obj) => _actionScheduler.EnqueueAction(new FighterActionData(this, obj));

		public void CancelAction()
		{
			StopAttack();
			_mover.CancelAction();
			_target = null;
		}

		private void StopAttack()
		{
			_animator.ResetTrigger(AttackHash);
			_animator.SetTrigger(StopAttackHash);
		}

		public object CaptureState() => _currentWeaponConfig.name;

		public void RestoreState(object state)
		{
			_currentWeaponConfig = Resources.Load<WeaponConfig>($"Equipables/{(string)state}");
			EquipWeapon(_currentWeaponConfig);
		}

		public void CompleteAction()
		{
			OnActionComplete?.Invoke();
			_actionScheduler.CompleteAction();
		}

		public void ExecuteAction(IActionData data) => _target = ((FighterActionData)data).Target.GetComponent<Health>();
	}
}