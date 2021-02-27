using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Inventories
{
	[CreateAssetMenu(fileName = "Drop Library", menuName = "RPG/Inventory/New Drop Library", order = 0)]
	public class DropLibrary : ScriptableObject
	{
		[SerializeField] private DropConfig[] potentialDrops;
		[SerializeField] private float[] dropChancePercentage;
		[SerializeField] private int[] minDrops;
		[SerializeField] private int[] maxDrops;

		[System.Serializable]
		private class DropConfig
		{
			public InventoryItem item;
			public float[] relativeChance;
			public int[] minNumber;
			public int[] maxNumber;

			public int GetRandomNumber(int level) => !item.IsStackable? 1:Random.Range(GetByLevel(minNumber, level), GetByLevel(maxNumber, level));
		}

		public struct Dropped
		{
			public InventoryItem Item;
			public int Number;
		}

		public IEnumerable<Dropped> GetRandomDrops(int level)
		{
			if(!ShouldRandomDrop(level))
			{
				yield break;
			}

			for(var i = 0;i < GetRandomNumberOfDrops(level);i++)
			{
				yield return GetRandomDrop(level);
			}
		}

		private Dropped GetRandomDrop(int level)
		{
			var drop = SelectRandomItem(level);
			var result = new Dropped {Item = drop.item, Number = drop.GetRandomNumber(level)};
			return result;
		}

		private bool ShouldRandomDrop(int level) => Random.Range(0,100) < GetByLevel(dropChancePercentage, level);

		private int GetRandomNumberOfDrops(int level) => Random.Range(GetByLevel(minDrops, level), GetByLevel(maxDrops, level));

		private DropConfig SelectRandomItem(int level)
		{
			var totalChance = GetTotalChance(level);
			var randomRoll = Random.Range(0, totalChance);
			float chanceTotal = 0;
			foreach(var drop in potentialDrops)
			{
				chanceTotal += GetByLevel(drop.relativeChance, level);
				if(chanceTotal > randomRoll)
				{
					return drop;
				}
			}

			return null;
		}

		private float GetTotalChance(int level) => potentialDrops.Sum(drop => GetByLevel(drop.relativeChance, level));

		private static T GetByLevel<T>(T[] values, int level)
		{
			if(values.Length == 0) return default;
			if(level > values.Length) return values[values.Length - 1];
			return level <= 0? default:values[level - 1];
		}
	}
}