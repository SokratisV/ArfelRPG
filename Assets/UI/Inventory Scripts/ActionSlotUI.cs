using RPG.UI.Dragging;
using RPG.Inventories;
using UnityEngine;

namespace RPG.UI.Inventories
{
    /// <summary>
    /// The UI slot for the player action bar.
    /// </summary>
    public class ActionSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        [SerializeField] private InventoryItemIcon icon = null;
        [SerializeField] private int index = 0;

        private ActionStore _store;

        private void Awake()
        {
            _store = GameObject.FindGameObjectWithTag("Player").GetComponent<ActionStore>();
            _store.StoreUpdated += UpdateIcon;
        }

        public void AddItems(InventoryItem item, int number) => _store.AddAction(item, index, number);

        public InventoryItem GetItem() => _store.GetAction(index);

        public int GetNumber() => _store.GetNumber(index);

        public int MaxAcceptable(InventoryItem item) => _store.MaxAcceptable(item, index);

        public void RemoveItems(int number) => _store.RemoveItems(index, number);

        private void UpdateIcon() => icon.SetItem(GetItem(), GetNumber());
    }
}
