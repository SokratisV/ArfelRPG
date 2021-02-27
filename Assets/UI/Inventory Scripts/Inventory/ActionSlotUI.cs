using RPG.Core;
using RPG.UI.Dragging;
using RPG.Inventories;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.UI.Inventories
{
	/// <summary>
	/// The UI slot for the player action bar.
	/// </summary>
	public class ActionSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>, IPointerClickHandler
	{
		[SerializeField] private InventoryItemIcon icon = null;
		[SerializeField] private int index = 0;

		private ActionStore _store;

		private void Awake()
		{
			_store = PlayerFinder.Player.GetComponent<ActionStore>();
			_store.StoreUpdated += UpdateIcon;
		}

		public void AddItems(InventoryItem item, int number) => _store.AddAction(item, index, number);

		public InventoryItem GetItem() => _store.GetAction(index);

		public int GetNumber() => _store.GetNumber(index);

		public int MaxAcceptable(InventoryItem item) => _store.MaxAcceptable(item, index);

		public void RemoveItems(int number) => _store.RemoveItems(index, number);

		private void UpdateIcon() => icon.SetItem(GetItem(), GetNumber());

		void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
		{
			// var shouldAct = eventData.button switch
			// {
			// 	PointerEventData.InputButton.Right => true,
			// 	PointerEventData.InputButton.Left when eventData.clickCount >= 2 => true,
			// 	_ => false
			// };
			//
			// if(shouldAct)
			// {
			// 	var item = GetItem();
			// 	if(item)
			// 	{
			// 		_playerEquipment.RemoveItem(equipLocation);
			// 		_playerInventory.AddToFirstEmptySlot(item, 1);
			// 	}
			// }
		}
	}
}