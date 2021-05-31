using RPG.Shops;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Shops
{
	public class ShopUI : MonoBehaviour
	{
		[SerializeField] private ShowHideUIOnButtonPress showHideUIOnButtonPress;
		[SerializeField] private TextMeshProUGUI shopName, totalField, switchButtonText, confirmButtonText;
		[SerializeField] private Transform listRoot;
		[SerializeField] private RowUI rowPrefab;
		[SerializeField] private Button confirmButton, switchButton;
		[SerializeField] private FilterButtonUi[] filterButtons;

		private Shopper _shopper = null;
		private Shop _currentShop = null;
		private Color _originalTextColor;

		private void Start()
		{
			_originalTextColor = totalField.color;
			_shopper = Shopper.GetPlayerShopper();
			if (_shopper == null) return;
			_shopper.ActiveShopChange += ShopChanged;
			confirmButton.onClick.AddListener(ConfirmTransaction);
			switchButton.onClick.AddListener(SwitchMode);
			ShopChanged();
		}

		private void ShopChanged()
		{
			if (_currentShop != null) _currentShop.OnChange -= RefreshUI;
			_currentShop = _shopper.GetActiveShop();
			showHideUIOnButtonPress.Toggle(_currentShop != null);

			foreach (var button in filterButtons)
			{
				button.SetShop(_currentShop);
			}

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
			totalField.color = _currentShop.HasSufficientFunds() ? _originalTextColor : Color.red;
			confirmButton.interactable = _currentShop.CanTransact();
			switchButtonText.SetText(_currentShop.IsBuyingMode ? "Switch to Selling" : "Switch to Buying");
			confirmButtonText.SetText(_currentShop.IsBuyingMode ? "Buy" : "Sell");
		}

		public void ConfirmTransaction() => _currentShop.ConfirmTransaction();

		public void SwitchMode() => _currentShop.IsBuyingMode = !_currentShop.IsBuyingMode;
	}
}