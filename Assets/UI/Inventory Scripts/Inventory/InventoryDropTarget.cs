using RPG.Core;
using UnityEngine;
using RPG.UI.Dragging;
using RPG.Inventories;

namespace RPG.UI.Inventories
{
	/// <summary>
	/// Handles spawning pickups when item dropped into the world.
	/// 
	/// Must be placed on the root canvas where items can be dragged. Will be
	/// called if dropped over empty space. 
	/// </summary>
	public class InventoryDropTarget : MonoBehaviour, IDragDestination<InventoryItem>
	{
		public void AddItems(InventoryItem item, int number)
		{
			var player = PlayerFinder.Player;;
			player.GetComponent<ItemDropper>().DropItem(item, number);
		}

		public int MaxAcceptable(InventoryItem item) => int.MaxValue;
	}
}