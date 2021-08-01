using RPG.Core;
using UnityEngine;
using RPG.UI.Dragging;
using RPG.Inventories;
using UnityEngine.EventSystems;

namespace RPG.UI.Inventories
{
	/// <summary>
	/// An slot for the players equipment.
	/// </summary>
	public class EquipmentSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>, IPointerClickHandler
	{
		[SerializeField] private InventoryItemIcon icon = null;
		[SerializeField] private EquipLocation equipLocation = EquipLocation.Weapon;

		private Equipment _playerEquipment;
		private Inventory _playerInventory;

		private void Awake()
		{
			var player = PlayerFinder.Player;
			_playerEquipment = player.GetComponent<Equipment>();
			_playerInventory = player.GetComponent<Inventory>();
			_playerEquipment.EquipmentUpdated += RedrawUI;
		}

		private void Start() => RedrawUI();

		public int MaxAcceptable(InventoryItem item)
		{
			var equipableItem = item as EquipableItem;
			if (equipableItem == null) return 0;
			if (!equipableItem.CanEquip(equipLocation, _playerEquipment)) return 0;
			return GetItem() != null ? 0 : 1;
		}

		public void AddItems(InventoryItem item, int _) => _playerEquipment.AddItem(equipLocation, (EquipableItem) item);

		public InventoryItem GetItem() => _playerEquipment.GetItemInSlot(equipLocation);

		public int GetNumber() => GetItem() != null ? 1 : 0;

		public void RemoveItems(int _) => _playerEquipment.RemoveItem(equipLocation);

		private void RedrawUI() => icon.SetItem(_playerEquipment.GetItemInSlot(equipLocation));

		void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
		{
			var shouldAct = eventData.button switch
			{
				PointerEventData.InputButton.Right => true,
				PointerEventData.InputButton.Left when eventData.clickCount >= 2 => true,
				_ => false
			};

			if (shouldAct)
			{
				var item = GetItem();
				if (item)
				{
					_playerEquipment.RemoveItem(equipLocation);
					_playerInventory.AddToFirstEmptySlot(item, 1, false);
				}
			}
		}
	}
}