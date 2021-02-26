using UnityEngine;
using RPG.UI.Dragging;
using RPG.Inventories;

namespace RPG.UI.Inventories
{
    /// <summary>
    /// An slot for the players equipment.
    /// </summary>
    public class EquipmentSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        [SerializeField] private InventoryItemIcon icon = null;
        [SerializeField] private EquipLocation equipLocation = EquipLocation.Weapon;

        private Equipment _playerEquipment;
       
        private void Awake() 
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            _playerEquipment = player.GetComponent<Equipment>();
            _playerEquipment.EquipmentUpdated += RedrawUI;
        }

        private void Start() => RedrawUI();

        public int MaxAcceptable(InventoryItem item)
        {
            var equipableItem = item as EquipableItem;
            if (equipableItem == null) return 0;
            if (equipableItem.AllowedEquipLocation != equipLocation) return 0;
            return GetItem() != null? 0:1;
        }

        public void AddItems(InventoryItem item, int number) => _playerEquipment.AddItem(equipLocation, (EquipableItem) item);

        public InventoryItem GetItem() => _playerEquipment.GetItemInSlot(equipLocation);

        public int GetNumber() => GetItem() != null? 1:0;

        public void RemoveItems(int number) => _playerEquipment.RemoveItem(equipLocation);

        private void RedrawUI() => icon.SetItem(_playerEquipment.GetItemInSlot(equipLocation));
    }
}