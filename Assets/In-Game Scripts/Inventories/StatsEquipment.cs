using System.Collections.Generic;
using System.Linq;
using RPG.Stats;

namespace RPG.Inventories
{
	public class StatsEquipment : Equipment, IModifierProvider
	{
		public IEnumerable<float> GetAdditiveModifiers(Stat stat) => 
			GetAllPopulatedSlots().Select(GetItemInSlot).OfType<IModifierProvider>().SelectMany(item => item.GetAdditiveModifiers(stat));

		public IEnumerable<float> GetPercentageModifiers(Stat stat) => 
			GetAllPopulatedSlots().Select(GetItemInSlot).OfType<IModifierProvider>().SelectMany(item => item.GetPercentageModifiers(stat));
	}
}