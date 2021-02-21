﻿using UnityEngine;

namespace RPG.Inventories
{
	/// <summary>
	/// To be placed at the root of a Pickup prefab. Contains the data about the
	/// pickup such as the type of item and the number.
	/// </summary>
	public class Pickup : MonoBehaviour
	{
		private InventoryItem _item;
		private int _number = 1;
		private Inventory _inventory;

		private void Awake()
		{
			var player = GameObject.FindGameObjectWithTag("Player");
			_inventory = player.GetComponent<Inventory>();
		}

		// PUBLIC

		/// <summary>
		/// Set the vital data after creating the prefab.
		/// </summary>
		/// <param name="item">The type of item this prefab represents.</param>
		/// <param name="number">The number of items represented.</param>
		public void Setup(InventoryItem item, int number)
		{
			_item = item;
			if(!item.IsStackable)
			{
				number = 1;
			}

			_number = number;
		}

		public InventoryItem GetItem() => _item;

		public int GetNumber() => _number;

		public void PickupItem()
		{
			var foundSlot = _inventory.AddToFirstEmptySlot(_item, _number);
			if(foundSlot)
			{
				Destroy(gameObject);
			}
		}

		public bool CanBePickedUp() => _inventory.HasSpaceFor(_item);
	}
}