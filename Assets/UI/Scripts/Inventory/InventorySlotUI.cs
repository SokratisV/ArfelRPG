using UnityEngine;
using RPG.Inventories;
using RPG.UI.Dragging;
using UnityEngine.EventSystems;

namespace RPG.UI.Inventories
{
	public class InventorySlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>, IPointerClickHandler
	{
		[SerializeField] private InventoryItemIcon icon = null;

		private int _index;
		private Inventory _playerInventory;
		private ActionStore _playerActions;
		private Equipment _playerEquipment;

		public void Setup(Inventory inventory, Equipment equipment, ActionStore store, int index)
		{
			_playerInventory = inventory;
			_playerEquipment = equipment;
			_playerActions = store;
			_index = index;
			icon.SetItem(inventory.GetItemInSlot(index), inventory.GetNumberInSlot(index));
		}

		public int MaxAcceptable(InventoryItem item) => _playerInventory.HasSpaceFor(item)? int.MaxValue:0;

		public void AddItems(InventoryItem item, int number) => _playerInventory.AddItemToSlot(_index, item, number);

		public InventoryItem GetItem() => _playerInventory.GetItemInSlot(_index);

		public int GetNumber() => _playerInventory.GetNumberInSlot(_index);

		public void RemoveItems(int number) => _playerInventory.RemoveFromSlot(_index, number);

		void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
		{
			var shouldAct = eventData.button switch
			{
				PointerEventData.InputButton.Right => true,
				PointerEventData.InputButton.Left when eventData.clickCount >= 2 => true,
				_ => false
			};

			if(shouldAct)
			{
				var item = GetItem();
				if(item is EquipableItem equipableItem)
				{
					RemoveItems(1);
					var previousItem = _playerEquipment.GetItemInSlot(equipableItem.AllowedEquipLocation);
					if(previousItem) AddItems(previousItem, 1);
					_playerEquipment.AddItem(equipableItem.AllowedEquipLocation, equipableItem);
				}
				else if(item is ActionItem actionItem)
				{
					RemoveItems(1);
					_playerActions.AddAction(actionItem);
				}
			}
		}
	}
}