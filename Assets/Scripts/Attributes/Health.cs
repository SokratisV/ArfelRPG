using System;
using RPG.Core;
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
		[SerializeField] private UnityEvent onDie;

		private BaseStats _baseStats;
		public bool IsInvulnerable { get; set; }
		public float LowestHealthValue { get; set; } = 0;

		//TODO Move everything to C# event
		public static event Action OnPlayerDeath;
		public event Action OnDeath;
		public event Action<GameObject, float> OnTakeDamage;
		public event Action<float> OnHealthChange;

		[Serializable]
		public class TakeDamageEvent : UnityEvent<float>
		{
		}

		private LazyValue<float> _healthPoints;

		public bool IsDead { get; private set; }

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

		public void Heal(float healthToRestore)
		{
			_healthPoints.Value = Mathf.Min(_healthPoints.Value + healthToRestore, GetMaxHealthPoints());
			OnHealthChange?.Invoke(healthToRestore);
		}

		/// <summary>
		/// Heal for x % of max HP
		/// </summary>
		/// <param name="percent"></param>
		public void HealPercent(float percent)
		{
			var maxHealthPoints = GetMaxHealthPoints();
			var healValue = maxHealthPoints * percent * 0.01f;
			Heal(healValue);
		}

		private void RestoreHealthOnLevelUp() => Heal(_baseStats.GetStat(Stat.Health) * (regenerationPercentage / 100));

		[ContextMenu("Take 30 damage")]
		public void TakeTestDamage() => TakeDamage(null, 30);

		[ContextMenu("Heal 40 points")]
		public void HealthTest() => Heal(40);

		public void TakeDamage(GameObject instigator, float damage)
		{
			if (IsInvulnerable) return;
			_healthPoints.Value = Mathf.Max(_healthPoints.Value - damage, LowestHealthValue);
			takeDamage.Invoke(damage);
			OnTakeDamage?.Invoke(instigator, damage);
			OnHealthChange?.Invoke(damage);
			if (_healthPoints.Value == 0)
			{
				Die();
				AwardExperience(instigator);
				onDie.Invoke();
				//TODO: Remove from health (add in different script only for player and call it through unity event?)
				if (tag.Equals("Player"))
				{
					OnPlayerDeath?.Invoke();
				}
				else
				{
					OnDeath?.Invoke();
				}
			}
		}

		public float GetHealthPoints() => _healthPoints.Value;

		public float GetMaxHealthPoints() => _baseStats.GetStat(Stat.Health);

		private void AwardExperience(GameObject instigator)
		{
			if (instigator.TryGetComponent(out Experience experience))
				experience.GainExperience(_baseStats.GetStat(Stat.ExperienceReward));
		}

		public float GetPercentage() => 100 * GetFraction();

		public float GetFraction() => _healthPoints.Value / _baseStats.GetStat(Stat.Health);

		private void Die()
		{
			if (IsDead) return;
			GetComponent<Animator>().SetTrigger(DieHash);
			GetComponent<ActionScheduler>().CancelCurrentAction();
			IsDead = true;
		}

		public void RestoreState(object state)
		{
			_healthPoints.Value = (float) state;
			if (_healthPoints.Value == 0) Die();
		}

		public object CaptureState() => _healthPoints.Value;
	}
}