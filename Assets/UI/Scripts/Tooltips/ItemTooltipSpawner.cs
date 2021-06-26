using UnityEngine;
using RPG.UI.Tooltips;

namespace RPG.UI.Inventories
{
	/// <summary>
	/// To be placed on a UI slot to spawn and show the correct item tooltip.
	/// </summary>
	[RequireComponent(typeof(IItemHolder))]
	public class ItemTooltipSpawner : TooltipSpawner
	{
		private IItemHolder _itemHolder;

		private void Awake() => _itemHolder = GetComponent<IItemHolder>();

		public override bool CanCreateTooltip() => _itemHolder?.GetItem();

		public override void UpdateTooltip(GameObject tooltip)
		{
			var itemTooltip = tooltip.GetComponent<ItemTooltip>();
			if (!itemTooltip) return;
			itemTooltip.Setup(_itemHolder.GetItem());
		}
	}
}