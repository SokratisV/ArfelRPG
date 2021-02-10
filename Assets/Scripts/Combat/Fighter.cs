using System.Collections.Generic;
using GameDevTV.Utils;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        [SerializeField] private float timeBetweenAttacks = 1f; //When changing, change RandomAttackAnimBehavior as well
        [SerializeField] private Transform rightHandTransform = null;
        [SerializeField] private Transform leftHandTransform = null;
        [SerializeField] private WeaponConfig defaultWeapon = null;

        private WeaponConfig _currentWeaponConfig;
        private Mover _mover;
        private ActionScheduler _actionScheduler;
        private Animator _animator;
        private LazyValue<Weapon> _currentWeapon;
        private Health _target;
        private float _timeSinceLastAttack = Mathf.Infinity;
        private static readonly int StopAttackHash = Animator.StringToHash("stopAttack");
        private static readonly int AttackHash = Animator.StringToHash("attack");

        private void Start()
        {
            _currentWeapon.ForceInit();
        }

        private void Awake()
        {
            _currentWeaponConfig = defaultWeapon;
            _currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
            _animator = GetComponent<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _mover = GetComponent<Mover>();
        }

        private Weapon SetupDefaultWeapon()
        {
            return AttachWeapon(defaultWeapon);
        }

        public void EquipWeapon(WeaponConfig weapon)
        {
            _currentWeaponConfig = weapon;
            _currentWeapon.Value = AttachWeapon(weapon);
        }

        private Weapon AttachWeapon(WeaponConfig weapon)
        {
            return weapon.Spawn(rightHandTransform, leftHandTransform, _animator);
        }

        public Health GetTarget()
        {
            return _target;
        }

        public WeaponConfig GetWeaponConfig()
        {
            return _currentWeaponConfig;
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;

            if(_target == null)
            {
                return;
            }

            if(_target.IsDead())
            {
                Complete();
                _target = null;
                return;
            }

            if(GetComponent<TrainingDummy>()) return; //TODO: Remove
            if(!IsInRange(_target.transform))
            {
                _mover.MoveTo(_target.transform.position, 1f);
            }
            else
            {
                _mover.Cancel();
                AttackBehavior();
            }
        }

        private void AttackBehavior()
        {
            transform.LookAt(_target.transform);
            if(!(_timeSinceLastAttack > timeBetweenAttacks)) return;
            TriggerAttack();
            _timeSinceLastAttack = 0;
        }

        private void TriggerAttack()
        {
            _animator.ResetTrigger(StopAttackHash);
            _animator.SetTrigger(AttackHash);
        }

        // Animation Event
        private void Hit()
        {
            var damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            if(_currentWeapon.Value != null)
            {
                _currentWeapon.Value.OnHit();
            }

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

        private void Shoot()
        {
            Hit();
        }

        public bool CanAttack(GameObject target)
        {
            if(target == null) return false;
            if(!_mover.CanMoveTo(target.transform.position) && !IsInRange(target.transform)) return false;
            var targetToTest = target.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        private bool IsInRange(Transform targetTransform)
        {
            return Vector3.Distance(transform.position, targetTransform.position) < _currentWeaponConfig.GetRange();
        }

        public void Attack(GameObject combatTarget)
        {
            _actionScheduler.StartAction(this);
            _target = combatTarget.GetComponent<Health>();
        }

        public void QueueAttackAction(GameObject gameObject)
        {
            _actionScheduler.EnqueueAction(new FighterActionData(this, gameObject));
        }

        public void Cancel()
        {
            StopAttack();
            _mover.Cancel();
            _target = null;
        }

        private void StopAttack()
        {
            _animator.ResetTrigger(AttackHash);
            _animator.SetTrigger(StopAttackHash);
        }

        public object CaptureState()
        {
            return _currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            _currentWeaponConfig = Resources.Load<WeaponConfig>((string)state);
            EquipWeapon(_currentWeaponConfig);
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if(stat == Stat.Damage)
            {
                yield return _currentWeaponConfig.GetDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if(stat == Stat.Damage)
            {
                yield return _currentWeaponConfig.GetPercentageBonus();
            }
        }

        public void Complete()
        {
            _actionScheduler.CompleteAction();
        }

        public void ExecuteAction(IActionData data)
        {
            _target = ((FighterActionData)data).Target.GetComponent<Health>();
            if(_target == null)
            {
                Debug.Log("Null combat target (already dead).");
                _actionScheduler.CompleteAction();
            }
        }
    }
}