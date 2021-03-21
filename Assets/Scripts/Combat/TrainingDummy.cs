using RPG.Attributes;

namespace RPG.Combat
{
	using UnityEngine;

	public class TrainingDummy : MonoBehaviour
	{
		[SerializeField] [Range(-1, 400)] private float aggroDistance;

		private Health _health;

		private void Awake()
		{
			_health = GetComponent<Health>();
			_health.LowestHealthValue = 1;
		}

		public void RestoreHealth()
		{
			if (_health.GetHealthPoints() <= 1)
			{
				_health.HealPercent(100);
			}
		}
	}
}