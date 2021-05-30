using RPG.Shops;
using TMPro;
using UnityEngine;

namespace RPG.UI.Shops
{
	public class ShopUI : MonoBehaviour
	{
		[SerializeField] private ShowHideUIOnButtonPress showHideUIOnButtonPress;
		[SerializeField] private TextMeshProUGUI shopName, totalField;
		[SerializeField] private Transform listRoot;
		[SerializeField] private RowUI rowPrefab;
		
		private Shopper _shopper;
		private Shop _currentShop = null;

		private void Start()
		{
			_shopper = Shopper.GetPlayerShopper();
			if (_shopper == null) return;
			_shopper.ActiveShopChange += ShopChanged;
			ShopChanged();
		}

		private void ShopChanged()
		{
			if (_currentShop != null) _currentShop.OnChange -= RefreshUI;
			_currentShop = _shopper.GetActiveShop();
			showHideUIOnButtonPress.Toggle(_currentShop != null);
			
			if (_currentShop == null) return;
			shopName.SetText(_currentShop.ShopName);

			_currentShop.OnChange += RefreshUI;
			RefreshUI();
		}

		private void RefreshUI()
		{
			foreach (Transform child in listRoot)
			{
				Destroy(child.gameObject);
			}

			foreach (var item in _currentShop.GetFilteredItems())
			{
				var row = Instantiate(rowPrefab, listRoot);
				row.Setup(_currentShop, item);
			}
			
			totalField.SetText($"{_currentShop.TransactionTotal():N2}");
		}
		public void ConfirmTransaction() => _currentShop.ConfirmTransaction();
	}
}