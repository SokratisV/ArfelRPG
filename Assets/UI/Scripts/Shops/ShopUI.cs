using RPG.Shops;
using TMPro;
using UnityEngine;

namespace RPG.UI.Shops
{
	public class ShopUI : MonoBehaviour
	{
		[SerializeField] private ShowHideUIOnButtonPress showHideUIOnButtonPress;
		[SerializeField] private TextMeshProUGUI shopName;
		
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
			_currentShop = _shopper.GetActiveShop();
			showHideUIOnButtonPress.Toggle(_currentShop != null);
			
			if (_currentShop == null) return;
			shopName.SetText(_currentShop.ShopName);
		}

		public void Close()
		{
			_shopper.SetActiveShop(null);
		}
	}
}