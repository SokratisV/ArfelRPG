using RPG.Shops;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Shops
{
	public class RowUI : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI itemName, availabilityField, priceField, quantityField;
		[SerializeField] private Image itemImage;

		private Shop _currentShop = null;
		private ShopItem _item = null;

		public void Setup(Shop currentShop, ShopItem item)
		{
			itemName.SetText(item.Name);
			itemImage.sprite = item.Icon;
			availabilityField.SetText($"{item.Availability}");
			priceField.SetText($"{item.Price:N1}");
			quantityField.SetText($"{item.Quantity}");
			_currentShop = currentShop;
			_item = item;
		}

		public void Add()
		{
			var quantity = 1;
			if (Input.GetKeyDown(KeyCode.LeftShift)) quantity = 10;
			_currentShop.AddToTransaction(_item.InventoryItem, quantity);
		}

		public void Remove()
		{
			var quantity = -1;
			if (Input.GetKeyDown(KeyCode.LeftShift)) quantity = -10;
			_currentShop.AddToTransaction(_item.InventoryItem, quantity);
		}
	}
}