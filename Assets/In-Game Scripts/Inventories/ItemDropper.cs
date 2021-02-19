using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RPG.Saving;

namespace RPG.Inventories
{
	/// <summary>
	/// To be placed on anything that wishes to drop pickups into the world.
	/// Tracks the drops for saving and restoring.
	/// </summary>
	public class ItemDropper : MonoBehaviour, ISaveable
	{
		private List<Pickup> _droppedItems = new List<Pickup>();

		/// <summary>
		/// Create a pickup at the current position.
		/// </summary>
		/// <param name="item">The item type for the pickup.</param>
		/// <param name="number">
		/// The number of items contained in the pickup. Only used if the item
		/// is stackable.
		/// </param>
		public void DropItem(InventoryItem item, int number) => SpawnPickup(item, GetDropLocation(), number);

		/// <summary>
		/// Create a pickup at the current position.
		/// </summary>
		/// <param name="item">The item type for the pickup.</param>
		public void DropItem(InventoryItem item) => SpawnPickup(item, GetDropLocation(), 1);

		/// <summary>
		/// Override to set a custom method for locating a drop.
		/// </summary>
		/// <returns>The location the drop should be spawned.</returns>
		protected virtual Vector3 GetDropLocation() => transform.position;

		public void SpawnPickup(InventoryItem item, Vector3 spawnLocation, int number)
		{
			var pickup = item.SpawnPickup(spawnLocation, number);
			_droppedItems.Add(pickup);
		}

		[System.Serializable]
		private struct DropRecord
		{
			public string itemID;
			public SerializableVector3 position;
			public int number;
		}

		object ISaveable.CaptureState()
		{
			RemoveDestroyedDrops();
			var droppedItemsList = new DropRecord[_droppedItems.Count];
			for(var i = 0;i < droppedItemsList.Length;i++)
			{
				droppedItemsList[i].itemID = _droppedItems[i].GetItem().GetItemID();
				droppedItemsList[i].position = new SerializableVector3(_droppedItems[i].transform.position);
				droppedItemsList[i].number = _droppedItems[i].GetNumber();
			}

			return droppedItemsList;
		}

		void ISaveable.RestoreState(object state)
		{
			var droppedItemsList = (DropRecord[])state;
			foreach(var item in droppedItemsList)
			{
				var pickupItem = InventoryItem.GetFromID(item.itemID);
				var position = item.position.ToVector();
				var number = item.number;
				SpawnPickup(pickupItem, position, number);
			}
		}

		/// <summary>
		/// Remove any drops in the world that have subsequently been picked up.
		/// </summary>
		private void RemoveDestroyedDrops()
		{
			var newList = _droppedItems.Where(item => item != null).ToList();
			_droppedItems = newList;
		}
	}
}