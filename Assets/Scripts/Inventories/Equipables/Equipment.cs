using System;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.Inventories
{
	/// <summary>
	/// Provides a store for the items equipped to a player. Items are stored by
	/// their equip locations.
	/// 
	/// This component should be placed on the GameObject tagged "Player".
	/// </summary>
	public class Equipment : MonoBehaviour, ISaveable
	{
		private Dictionary<EquipLocation, EquipableItem> _equippedItems = new Dictionary<EquipLocation, EquipableItem>();
		
		/// <summary>
		/// Broadcasts when the items in the slots are added/removed.
		/// </summary>
		public event Action EquipmentUpdated;
		
		/// <summary>
		/// Convenience for getting the player's equipment.
		/// </summary>
		public static Equipment GetPlayerEquipment()
		{
			var player = GameObject.FindWithTag("Player");
			return player.GetComponent<Equipment>();
		}
		
		/// <summary>
		/// Return the item in the given equip location.
		/// </summary>
		public EquipableItem GetItemInSlot(EquipLocation equipLocation) => !_equippedItems.ContainsKey(equipLocation)? null:_equippedItems[equipLocation];

		/// <summary>
		/// Add an item to the given equip location. Do not attempt to equip to
		/// an incompatible slot.
		/// </summary>
		public void AddItem(EquipLocation slot, EquipableItem item)
		{
			Debug.Assert(item.AllowedEquipLocation == slot);
			_equippedItems[slot] = item;
			EquipmentUpdated?.Invoke();
		}

		/// <summary>
		/// Remove the item for the given slot.
		/// </summary>
		public void RemoveItem(EquipLocation slot)
		{
			_equippedItems.Remove(slot);
			EquipmentUpdated?.Invoke();
		}

		/// <summary>
		/// Enumerate through all the sltos that currently contain items.
		/// </summary>
		public IEnumerable<EquipLocation> GetAllPopulatedSlots() => _equippedItems.Keys;
		
		object ISaveable.CaptureState()
		{
			var equippedItemsForSerialization = new Dictionary<EquipLocation, string>();
			foreach(var pair in _equippedItems)
			{
				equippedItemsForSerialization[pair.Key] = pair.Value.ItemID;
			}

			return equippedItemsForSerialization;
		}

		void ISaveable.RestoreState(object state)
		{
			_equippedItems = new Dictionary<EquipLocation, EquipableItem>();

			var equippedItemsForSerialization = (Dictionary<EquipLocation, string>)state;

			foreach(var pair in equippedItemsForSerialization)
			{
				var item = (EquipableItem)InventoryItem.GetFromID(pair.Value);
				if(item != null)
				{
					_equippedItems[pair.Key] = item;
				}
			}
		}
	}
}