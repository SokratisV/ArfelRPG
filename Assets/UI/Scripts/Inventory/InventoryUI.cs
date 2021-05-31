using UnityEngine;
using RPG.Inventories;

namespace RPG.UI.Inventories
{
	/// <summary>
	/// To be placed on the root of the inventory UI. Handles spawning all the
	/// inventory slot prefabs.
	/// </summary>
	public class InventoryUI : MonoBehaviour
	{
		[SerializeField] private InventorySlotUI inventoryItemPrefab = null;

		private Inventory _playerInventory;
		private ActionStore _playerActions;
		private Equipment _equipment;

		private void Awake()
		{
			_playerInventory = Inventory.GetPlayerInventory();
			_equipment = Equipment.GetPlayerEquipment();
			_playerActions = ActionStore.GetPlayerActions();
			_playerInventory.InventoryUpdated += Redraw;
		}

		private void Start() => Redraw();

		private void Redraw()
		{
			foreach(Transform child in transform)
			{
				Destroy(child.gameObject);
			}

			var inventorySize = _playerInventory.GetSize();
			for(var i = 0;i < inventorySize;i++)
			{
				var itemUI = Instantiate(inventoryItemPrefab, transform);
				itemUI.Setup(_playerInventory, _equipment, _playerActions, i);
			}
		}
	}
}