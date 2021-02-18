using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{
	/// <summary>
	/// A ScriptableObject that represents any item that can be put in an
	/// inventory.
	/// </summary>
	/// <remarks>
	/// In practice, you are likely to use a subclass such as `ActionItem` or
	/// `EquipableItem`.
	/// </remarks>
	public abstract class InventoryItem : ScriptableObject, ISerializationCallbackReceiver
	{
		// CONFIG DATA
		[Tooltip("Auto-generated UUID for saving/loading. Clear this field if you want to generate a new one.")] [SerializeField]
		private string itemID = null;

		[Tooltip("Item name to be displayed in UI.")] [SerializeField]
		private string displayName = null;

		[Tooltip("Item description to be displayed in UI.")] [SerializeField] [TextArea]
		private string description = null;

		[Tooltip("The UI icon to represent this item in the inventory.")] [SerializeField]
		private Sprite icon = null;

		[Tooltip("The prefab that should be spawned when this item is dropped.")] [SerializeField]
		private Pickup pickup = null;

		[Tooltip("If true, multiple items of this type can be stacked in the same inventory slot.")] [SerializeField]
		private bool stackable = false;

		// STATE
		private static Dictionary<string, InventoryItem> itemLookupCache;

		// PUBLIC

		/// <summary>
		/// Get the inventory item instance from its UUID.
		/// </summary>
		/// <param name="itemID">
		/// String UUID that persists between game instances.
		/// </param>
		/// <returns>
		/// Inventory item instance corresponding to the ID.
		/// </returns>
		public static InventoryItem GetFromID(string itemID)
		{
			if(itemLookupCache == null)
			{
				itemLookupCache = new Dictionary<string, InventoryItem>();
				var itemList = Resources.LoadAll<InventoryItem>("");
				foreach(var item in itemList)
				{
					if(itemLookupCache.ContainsKey(item.itemID))
					{
						Debug.LogError($"Looks like there's a duplicate GameDevTV.UI.InventorySystem ID for objects: {itemLookupCache[item.itemID]} and {item}");
						continue;
					}

					itemLookupCache[item.itemID] = item;
				}
			}

			if(itemID == null || !itemLookupCache.ContainsKey(itemID)) return null;
			return itemLookupCache[itemID];
		}

		/// <summary>
		/// Spawn the pickup gameobject into the world.
		/// </summary>
		/// <param name="position">Where to spawn the pickup.</param>
		/// <param name="number">How many instances of the item does the pickup represent.</param>
		/// <returns>Reference to the pickup object spawned.</returns>
		public Pickup SpawnPickup(Vector3 position, int number)
		{
			var pickup = Instantiate(this.pickup);
			pickup.transform.position = position;
			pickup.Setup(this, number);
			return pickup;
		}

		public Sprite GetIcon() => icon;

		public string GetItemID() => itemID;

		public bool IsStackable => stackable;

		public string GetDisplayName => displayName;

		public string GetDescription => description;

		// PRIVATE

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			// Generate and save a new UUID if this is blank.
			if(string.IsNullOrWhiteSpace(itemID))
			{
				itemID = System.Guid.NewGuid().ToString();
			}
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			// Require by the ISerializationCallbackReceiver but we don't need
			// to do anything with it.
		}
	}
}