using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

namespace RPG.Stats
{
	[CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
	public class Progression : ScriptableObject
	{
		[SerializeField] private ProgressionDictionary progressionDictionary = new ProgressionDictionary();

		public float GetStat(Stat stat, CharacterClass characterClass, int level)
		{
			var levels = progressionDictionary[characterClass][stat];
			if(levels.levels.Length < level) return 0;

			return levels.levels[level - 1];
		}

		public int GetLevels(Stat stat, CharacterClass characterClass)
		{
			var levels = progressionDictionary[characterClass][stat];
			return levels.levels.Length - 1;
		}

		[System.Serializable]
		public class ProgressionStats
		{
			public float[] levels;
		}

		[System.Serializable]
		public class ProgressionStatDictionary : SerializableDictionaryBase<Stat, ProgressionStats>
		{
		}

		[System.Serializable]
		private class ProgressionDictionary : SerializableDictionaryBase<CharacterClass, ProgressionStatDictionary>
		{
		}
	}
}