using RPG.Shops;
using RPG.Stats;
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
		private TraitStore _traitStore;
		private Shop _currentShop = null;
		private Color _originalTextColor;

		private void Start()
		{
			_originalTextColor = totalField.color;
			_shopper = Shopper.GetPlayerShopper();
			_traitStore = _shopper.GetComponent<TraitStore>();
			if (_shopper == null) return;
			_shopper.OnActiveShopChange += ShopChanged;
			_traitStore.OnStagedPointsChanged += UpdateUiForStatChange;
			confirmButton.onClick.AddListener(ConfirmTransaction);
			switchButton.onClick.AddListener(SwitchMode);
			ShopChanged();
		}

		private void ShopChanged()
		{
			if (_currentShop != null) _currentShop.OnChange -= UpdateUi;
			_currentShop = _shopper.GetActiveShop();
			showHideUIOnButtonPress.Toggle(_currentShop != null);

			foreach (var button in filterButtons)
			{
				button.SetShop(_currentShop);
			}

			if (_currentShop == null) return;
			shopName.SetText(_currentShop.ShopName);

			_currentShop.OnChange += UpdateUi;
			UpdateUi();
		}

		private void UpdateUiForStatChange(Trait _, int __)
		{
			if (_currentShop == null) return;
			UpdateUi();
		}

		private void UpdateUi()
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