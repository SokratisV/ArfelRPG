using UnityEngine;
using RPG.Inventories;
using RPG.UI.Dragging;

namespace RPG.UI.Inventories
{
    public class InventorySlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        [SerializeField] private InventoryItemIcon icon = null;

        private int _index;
        private InventoryItem _item;
        private Inventory _inventory;

        public void Setup(Inventory inventory, int index)
        {
            _inventory = inventory;
            _index = index;
            icon.SetItem(inventory.GetItemInSlot(index), inventory.GetNumberInSlot(index));
        }

        public int MaxAcceptable(InventoryItem item) => _inventory.HasSpaceFor(item)? int.MaxValue:0;

        public void AddItems(InventoryItem item, int number) => _inventory.AddItemToSlot(_index, item, number);

        public InventoryItem GetItem() => _inventory.GetItemInSlot(_index);

        public int GetNumber() => _inventory.GetNumberInSlot(_index);

        public void RemoveItems(int number) => _inventory.RemoveFromSlot(_index, number);
    }
}