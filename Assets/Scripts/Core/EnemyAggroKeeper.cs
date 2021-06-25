using UnityEngine;

namespace RPG.Core
{
	public class EnemyAggroKeeper : MonoBehaviour
	{
		public int NumberOfEnemiesInCombatWith { get; private set; }

		public void AdjustEnemyCount(bool toggle)
		{
			if (toggle) IncreaseEnemyCount();
			else DecreaseEnemyCount();
		}

		private void IncreaseEnemyCount() => NumberOfEnemiesInCombatWith++;

		private void DecreaseEnemyCount()
		{
			NumberOfEnemiesInCombatWith--;
			if (NumberOfEnemiesInCombatWith < 0)
			{
				Debug.LogError("Oh-oh. Negative amount of enemies exist.");
			}
		}
	}
}