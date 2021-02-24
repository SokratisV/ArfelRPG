using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RPG.Saving;
using UnityEngine.SceneManagement;

namespace RPG.Inventories
{
	/// <summary>
	/// To be placed on anything that wishes to drop pickups into the world.
	/// Tracks the drops for saving and restoring.
	/// </summary>
	public class ItemDropper : MonoBehaviour, ISaveable
	{
		private List<Pickup> _droppedItems = new List<Pickup>();
		private List<DropRecord> _otherSceneDroppedItems = new List<DropRecord>();

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
			public int sceneBuildIndex;
		}

		object ISaveable.CaptureState()
		{
			RemoveDestroyedDrops();
			var droppedItemsList = new List<DropRecord>();
			var sceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
			foreach(var pickup in _droppedItems)
			{
				var droppedItem = new DropRecord
				{
					itemID = pickup.GetItem().GetItemID(),
					position = new SerializableVector3(pickup.transform.position),
					number = pickup.GetNumber(),
					sceneBuildIndex = sceneBuildIndex
				};
				droppedItemsList.Add(droppedItem);
			}

			droppedItemsList.AddRange(_otherSceneDroppedItems);
			return droppedItemsList;
		}

		void ISaveable.RestoreState(object state)
		{
			return;
			var droppedItemsList = (DropRecord[])state;
			var sceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
			_otherSceneDroppedItems.Clear();
			foreach(var item in droppedItemsList)
			{
				if(item.sceneBuildIndex != sceneBuildIndex)
				{
					_otherSceneDroppedItems.Add(item);
					continue;
				}

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