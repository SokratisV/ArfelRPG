using System;
using System.Collections.Generic;
using UnityEditor;
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

		private static Dictionary<string, InventoryItem> ItemLookupCache;

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
			if(ItemLookupCache == null)
			{
				ItemLookupCache = new Dictionary<string, InventoryItem>();
				var itemList = Resources.LoadAll<InventoryItem>("");
				foreach(var item in itemList)
				{
					if(ItemLookupCache.ContainsKey(item.itemID))
					{
						Debug.LogError($"Looks like there's a duplicate RPG.UI.InventorySystem ID for objects: {ItemLookupCache[item.itemID]} and {item}");
						continue;
					}

					ItemLookupCache[item.itemID] = item;
				}
			}

			if(itemID == null || !ItemLookupCache.ContainsKey(itemID)) return null;
			return ItemLookupCache[itemID];
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

		public Pickup Pickup => pickup;

		public Sprite Icon => icon;

		public string ItemID => itemID;

		public bool IsStackable => stackable;

		public string DisplayName => displayName;

		public virtual string Description => description;

		public string RawDescription => description;
		
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			// Generate and save a new UUID if this is blank.
			if(string.IsNullOrWhiteSpace(itemID))
			{
				itemID = Guid.NewGuid().ToString();
			}
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			// Require by the ISerializationCallbackReceiver but we don't need
			// to do anything with it.
		}

#if UNITY_EDITOR

		public void SetDisplayName(string newDisplayName)
		{
			if(displayName == newDisplayName) return;
			SetUndo("Change Display Name");
			displayName = newDisplayName;
			Dirty();
		}

		public void SetDescription(string newDescription)
		{
			if(description == newDescription) return;
			SetUndo("Change Description");
			description = newDescription;
			Dirty();
		}

		public void SetIcon(Sprite newIcon)
		{
			if(icon == newIcon) return;
			SetUndo("Change Icon");
			icon = newIcon;
			Dirty();
		}

		public void SetPickup(Pickup newPickup)
		{
			if(pickup == newPickup) return;
			SetUndo("Change Pickup");
			pickup = newPickup;
			Dirty();
		}

		public void SetItemID(string newItemID)
		{
			if(itemID == newItemID) return;
			SetUndo("Change ItemID");
			itemID = newItemID;
			Dirty();
		}

		public void SetStackable(bool newStackable)
		{
			if(stackable == newStackable) return;
			SetUndo(stackable? "Set Not Stackable":"Set Stackable");
			stackable = newStackable;
			Dirty();
		}

		private bool _drawInventoryItem = true;
		protected GUIStyle FoldoutStyle;
		protected GUIStyle ContentStyle;

		public virtual void DrawCustomInspector()
		{
			ContentStyle = new GUIStyle {padding = new RectOffset(15, 15, 0, 0)};
			FoldoutStyle = new GUIStyle(EditorStyles.foldout) {fontStyle = FontStyle.Bold};
			_drawInventoryItem = EditorGUILayout.Foldout(_drawInventoryItem, "Inventory Item Data", FoldoutStyle);
			if(!_drawInventoryItem) return;
			EditorGUILayout.HelpBox($"{name}/{DisplayName}", MessageType.Info);
			SetItemID(EditorGUILayout.TextField("ItemID (clear to reset)", ItemID));
			SetDisplayName(EditorGUILayout.TextField("Display name", DisplayName));
			SetDescription(EditorGUILayout.TextField("Description", Description));
			SetIcon((Sprite)EditorGUILayout.ObjectField("Icon", Icon, typeof(Sprite), false));
			SetPickup((Pickup)EditorGUILayout.ObjectField("Pickup", Pickup, typeof(Pickup), false));
			SetStackable(EditorGUILayout.Toggle("Stackable", IsStackable));
		}

		protected void SetUndo(string message) => Undo.RecordObject(this, message);

		protected void Dirty() => EditorUtility.SetDirty(this);
#endif
	}
}