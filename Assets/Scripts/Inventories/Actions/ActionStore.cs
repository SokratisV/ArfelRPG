using System;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;
using RPG.Saving;

namespace RPG.Inventories
{
	/// <summary>
	/// Provides the storage for an action bar. The bar has a finite number of
	/// slots that can be filled and actions in the slots can be "used".
	/// 
	/// This component should be placed on the GameObject tagged "Player".
	/// </summary>
	public class ActionStore : MonoBehaviour, ISaveable
	{
		private Dictionary<int, DockedItemSlot> _dockedItems = new Dictionary<int, DockedItemSlot>();

		private class DockedItemSlot
		{
			public ActionItem Item;
			public int Number;
		}

		public static ActionStore GetPlayerActions() => PlayerFinder.Player.GetComponent<ActionStore>();

		/// <summary>
		/// Broadcasts when the items in the slots are added/removed.
		/// </summary>
		public event Action StoreUpdated;

		/// <summary>
		/// Get the action at the given index.
		/// </summary>
		public ActionItem GetAction(int index) => _dockedItems.ContainsKey(index)? _dockedItems[index].Item:null;

		/// <summary>
		/// Get the number of items left at the given index.
		/// </summary>
		/// <returns>
		/// Will return 0 if no item is in the index or the item has
		/// been fully consumed.
		/// </returns>
		public int GetNumber(int index) => _dockedItems.ContainsKey(index)? _dockedItems[index].Number:0;

		/// <summary>
		/// Add an item to the given index.
		/// </summary>
		/// <param name="item">What item should be added.</param>
		/// <param name="index">Where should the item be added.</param>
		/// <param name="number">How many items to add.</param>
		public void AddAction(InventoryItem item, int index = -1, int number = 1)
		{
			if(index > GlobalValues.ActionBarCount) return;
			if(index >= 0) AddToKnownIndex(item, index, number);
			else AddToFirstAvailableSlot(item, number);
			StoreUpdated?.Invoke();
		}

		private void AddToFirstAvailableSlot(InventoryItem item, int number)
		{
			for(var i = 0;i < GlobalValues.ActionBarCount;i++)
			{
				if(_dockedItems.ContainsKey(i)) continue;
				AddToKnownIndex(item, i, number);
				return;
			}
		}

		private void AddToKnownIndex(InventoryItem item, int index, int number)
		{
			if(_dockedItems.ContainsKey(index))
			{
				if(ReferenceEquals(item, _dockedItems[index].Item))
				{
					_dockedItems[index].Number += number;
				}
			}
			else
			{
				var slot = new DockedItemSlot {Item = item as ActionItem, Number = number};
				_dockedItems[index] = slot;
			}
		}

		/// <summary>
		/// Use the item at the given slot. If the item is consumable one
		/// instance will be destroyed until the item is removed completely.
		/// </summary>
		/// <param name="user">The character that wants to use this action.</param>
		/// <returns>False if the action could not be executed.</returns>
		public bool Use(int index, GameObject user)
		{
			if(index > GlobalValues.ActionBarCount) return false;
			if(!_dockedItems.ContainsKey(index)) return false;
			_dockedItems[index].Item.Use(user);
			if(_dockedItems[index].Item.IsConsumable)
			{
				RemoveItems(index, 1);
			}

			return true;
		}

		/// <summary>
		/// Remove a given number of items from the given slot.
		/// </summary>
		public void RemoveItems(int index, int number)
		{
			if(index > GlobalValues.ActionBarCount) return;
			if(_dockedItems.ContainsKey(index))
			{
				_dockedItems[index].Number -= number;
				if(_dockedItems[index].Number <= 0)
				{
					_dockedItems.Remove(index);
				}

				StoreUpdated?.Invoke();
			}
		}

		/// <summary>
		/// What is the maximum number of items allowed in this slot.
		/// 
		/// This takes into account whether the slot already contains an item
		/// and whether it is the same type. Will only accept multiple if the
		/// item is consumable.
		/// </summary>
		/// <returns>Will return int.MaxValue when there is not effective bound.</returns>
		public int MaxAcceptable(InventoryItem item, int index)
		{
			var actionItem = item as ActionItem;
			if(!actionItem) return 0;

			if(_dockedItems.ContainsKey(index) && !ReferenceEquals(item, _dockedItems[index].Item))
				return 0;

			if(actionItem.IsConsumable)
				return int.MaxValue;

			return _dockedItems.ContainsKey(index)? 0:1;
		}

		[Serializable]
		private struct DockedItemRecord
		{
			public string itemID;
			public int number;
		}

		object ISaveable.CaptureState()
		{
			var state = new Dictionary<int, DockedItemRecord>();
			foreach(var pair in _dockedItems)
			{
				var record = new DockedItemRecord {itemID = pair.Value.Item.ItemID, number = pair.Value.Number};
				state[pair.Key] = record;
			}

			return state;
		}

		void ISaveable.RestoreState(object state)
		{
			var stateDict = (Dictionary<int, DockedItemRecord>)state;
			foreach(var pair in stateDict)
			{
				AddAction(InventoryItem.GetFromID(pair.Value.itemID), pair.Key, pair.Value.number);
			}
		}
	}
}