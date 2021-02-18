using System.Collections.Generic;
using System.Linq;
using RPG.Stats;
using UnityEngine;

namespace RPG.Inventories
{
	[CreateAssetMenu(fileName = "Equipable Item with Stats", menuName = "RPG/Inventory/Equipable Item With Stats")]
	public class StatsEquipableItem : EquipableItem, IModifierProvider
	{
		[SerializeField] private Modifier[] additiveModifiers;
		[SerializeField] private Modifier[] percentageModifiers;

		[System.Serializable]
		private struct Modifier
		{
			public Stat Stat;
			public float Value;
		}

		public IEnumerable<float> GetAdditiveModifiers(Stat stat) => 
			from modifier in additiveModifiers where modifier.Stat == stat select modifier.Value;

		public IEnumerable<float> GetPercentageModifiers(Stat stat) => 
			from modifier in percentageModifiers where modifier.Stat == stat select modifier.Value;
	}
}