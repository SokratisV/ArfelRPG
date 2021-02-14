using System;
using GameDevTV.Utils;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private float regenerationPercentage = 70;
        [SerializeField] private TakeDamageEvent takeDamage;
        [SerializeField] private UnityEvent onDie;

        private BaseStats _baseStats;

        //TODO Move everything to C# event
        public static event Action OnPlayerDeath;
        public event Action OnDeath;
        public event Action<GameObject> OnTakeDamage;

        [Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
        }

        private LazyValue<float> _healthPoints;

        public bool IsDead {get;private set;}
        private static readonly int DieHash = Animator.StringToHash("die");

        private void Awake()
        {
            _healthPoints = new LazyValue<float>(GetInitialHealth);
            _baseStats = GetComponent<BaseStats>();
        }

        private void OnEnable() => _baseStats.OnLevelUp += RestoreHealth;

        private void OnDisable() => _baseStats.OnLevelUp -= RestoreHealth;

        private void Start() => _healthPoints.ForceInit();

        private float GetInitialHealth() => _baseStats.GetStat(Stat.Health);

        internal void Heal(float healthToRestore) => _healthPoints.Value = Mathf.Min(_healthPoints.Value + healthToRestore, GetMaxHealthPoints());

        private void RestoreHealth()
        {
            var regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
            _healthPoints.Value = Mathf.Max(_healthPoints.Value, regenHealthPoints);
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            _healthPoints.Value = Mathf.Max(_healthPoints.Value - damage, 0);
            takeDamage.Invoke(damage);
            OnTakeDamage?.Invoke(instigator);
            if(_healthPoints.Value == 0)
            {
                onDie.Invoke();
                //TODO: Remove from health (add in different script only for player and call it through unity event?)
                if(tag.Equals("Player"))
                {
                    OnPlayerDeath?.Invoke();
                }
                else
                {
                    OnDeath?.Invoke();
                }

                //
                Die();
                AwardExperience(instigator);
            }
        }

        public float GetHealthPoints() => _healthPoints.Value;

        public float GetMaxHealthPoints() => _baseStats.GetStat(Stat.Health);

        private void AwardExperience(GameObject instigator)
        {
            if(instigator.TryGetComponent(out Experience experience))
                experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        public float GetPercentage() => 100 * GetFraction();

        public float GetFraction() => _healthPoints.Value / _baseStats.GetStat(Stat.Health);

        private void Die()
        {
            if(IsDead) return;
            GetComponent<Animator>().SetTrigger(DieHash);
            GetComponent<ActionScheduler>().CancelCurrentAction();
            IsDead = true;
        }

        public void RestoreState(object state)
        {
            _healthPoints.Value = (float)state;
            if(_healthPoints.Value == 0) Die();
        }

        public object CaptureState() => _healthPoints.Value;
    }
}