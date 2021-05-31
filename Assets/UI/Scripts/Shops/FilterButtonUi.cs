using RPG.Inventories;
using RPG.Shops;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Shops
{
	public class FilterButtonUi : MonoBehaviour
	{
		[SerializeField] private ItemCategory category = ItemCategory.None;
		private Button _button;
		private Shop _currentShop;

		private void Awake()
		{
			_button = GetComponent<Button>();
			_button.onClick.AddListener(SelectFilter);
		}

		private void RefreshUi() => _button.interactable = _currentShop.GetFilter() != category;

		public void SetShop(Shop currentShop)
		{
			if (_currentShop != null) _currentShop.OnChange -= RefreshUi;
			_currentShop = currentShop;
			if (_currentShop != null)
			{
				_currentShop.OnChange += RefreshUi;
				RefreshUi();
			}
		}

		private void SelectFilter() => _currentShop.SelectFilter(category);
	}
}