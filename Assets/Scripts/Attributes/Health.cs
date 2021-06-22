using System;
using RPG.Core;
using RPG.Core.SystemEvents;
using RPG.Utils;
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
		[SerializeField] private VoidEvent playerDeath;

		private BaseStats _baseStats;
		public bool IsInvulnerable { get; set; }
		public float LowestHealthValue { get; set; } = 0;
		public bool IsDead => _healthPoints.Value <= 0;

		public event Action OnDeath;
		public event Action<GameObject, float> OnHealthChange;

		[Serializable]
		public class TakeDamageEvent : UnityEvent<float>
		{
		}

		private LazyValue<float> _healthPoints;

		private bool _wasDeadLastFrame;
		private static readonly int DieHash = Animator.StringToHash("die");

		private void Awake()
		{
			_healthPoints = new LazyValue<float>(GetInitialHealth);
			_baseStats = GetComponent<BaseStats>();
		}

		private void OnEnable() => _baseStats.OnLevelUp += RestoreHealthOnLevelUp;

		private void OnDisable() => _baseStats.OnLevelUp -= RestoreHealthOnLevelUp;

		private void Start() => _healthPoints.ForceInit();

		private float GetInitialHealth() => _baseStats.GetStat(Stat.Health);

		public void Heal(GameObject instigator, float healthToRestore)
		{
			_healthPoints.Value = Mathf.Min(_healthPoints.Value + healthToRestore, GetMaxHealthPoints());
			UpdateState();
			OnHealthChange?.Invoke(instigator, healthToRestore);
		}

		public void HealPercent(GameObject instigator, float percent)
		{
			var maxHealthPoints = GetMaxHealthPoints();
			var healValue = maxHealthPoints * percent * 0.01f;
			Heal(instigator, healValue);
		}

		private void RestoreHealthOnLevelUp() => Heal(gameObject, _baseStats.GetStat(Stat.Health) * (regenerationPercentage / 100));

		[ContextMenu("Take 30 damage")]
		private void TakeTestDamage() => TakeDamage(null, 30);

		[ContextMenu("Heal 40 points")]
		public void HealthTest() => Heal(gameObject, 40);
		
		[ContextMenu("Perish")]
		private void TestDeath() => TakeDamage(null, _healthPoints.Value);

		public void TakeDamage(GameObject instigator, float damage)
		{
			if (IsInvulnerable) return;
			_healthPoints.Value = Mathf.Max(_healthPoints.Value - damage, LowestHealthValue);
			takeDamage.Invoke(damage);
			OnHealthChange?.Invoke(instigator, damage);
			if (IsDead)
			{
				AwardExperience(instigator);
				if (tag.Equals("Player"))
				{
					playerDeath.Raise();
				}

				OnDeath?.Invoke();
			}

			UpdateState();
		}

		public float GetHealthPoints() => _healthPoints.Value;

		public float GetMaxHealthPoints() => _baseStats.GetStat(Stat.Health);

		private void AwardExperience(GameObject instigator)
		{
			if (instigator == null) return;
			if (instigator.TryGetComponent(out Experience experience))
				experience.GainExperience(_baseStats.GetStat(Stat.ExperienceReward));
		}

		public float GetPercentage() => 100 * GetFraction();

		public float GetFraction() => _healthPoints.Value / _baseStats.GetStat(Stat.Health);

		private void UpdateState()
		{
			if (!_wasDeadLastFrame && IsDead)
			{
				GetComponent<Animator>().SetTrigger(DieHash);
				GetComponent<ActionScheduler>().CancelCurrentAction();
			}

			if (_wasDeadLastFrame && !IsDead)
			{
				GetComponent<Animator>().Rebind();
			}

			_wasDeadLastFrame = IsDead;
		}

		public void RestoreState(object state)
		{
			_healthPoints.Value = (float) state;
			UpdateState();
		}

		public object CaptureState() => _healthPoints.Value;
	}
}