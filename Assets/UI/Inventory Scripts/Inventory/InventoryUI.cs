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
		private Equipment _equipment;

		private void Awake()
		{
			_playerInventory = Inventory.GetPlayerInventory();
			_equipment = Equipment.GetPlayerEquipment();
			_playerInventory.InventoryUpdated += Redraw;
		}

		private void Start() => Redraw();

		private void Redraw()
		{
			foreach(Transform child in transform)
			{
				Destroy(child.gameObject);
			}

			for(var i = 0;i < _playerInventory.GetSize();i++)
			{
				var itemUI = Instantiate(inventoryItemPrefab, transform);
				itemUI.Setup(_playerInventory,_equipment, i);
			}
		}
	}
}