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

		private void UpdateUi() => _button.interactable = _currentShop.GetFilter() != category;

		public void SetShop(Shop currentShop)
		{
			if (_currentShop != null) _currentShop.OnChange -= UpdateUi;
			_currentShop = currentShop;
			if (_currentShop != null)
			{
				_currentShop.OnChange += UpdateUi;
				UpdateUi();
			}
		}

		private void SelectFilter() => _currentShop.SelectFilter(category);
	}
}