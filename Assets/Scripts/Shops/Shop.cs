using System;
using System.Collections.Generic;
using RPG.Core;
using RPG.Inventories;
using UnityEngine;

namespace RPG.Shops
{
	public class Shop : MonoBehaviour, IRaycastable, IInteractable
	{
		[SerializeField] private string shopName;
		[SerializeField] private StockItemConfig[] stockConfig;

		public event Action OnChange;

		private Shopper _currentShopper = null;
		private OutlineableComponent _outlineableComponent;
		private Dictionary<InventoryItem, int> _transaction = new Dictionary<InventoryItem, int>();

		#region Unity

		public string ShopName => shopName;

		private void Awake() => _outlineableComponent = new OutlineableComponent(gameObject, GlobalValues.ShopInteractableColor);

		private void Start() => _currentShopper = Shopper.GetPlayerShopper();

		#endregion

		#region Public

		public IEnumerable<ShopItem> GetFilteredItems() => GetAllitems();

		public IEnumerable<ShopItem> GetAllitems()
		{
			foreach (var stockItemConfig in stockConfig)
			{
				var price = stockItemConfig.item.Price * (1 - stockItemConfig.discountPercentage / 100);
				_transaction.TryGetValue(stockItemConfig.item, out var quantity);
				yield return new ShopItem(stockItemConfig.item, stockItemConfig.initialStock, price, quantity);
			}
		}

		public void ConfirmTransaction()
		{
			var shopperInventory = _currentShopper.GetComponent<Inventory>();
			var purse = _currentShopper.GetComponent<Purse>();
			if (shopperInventory == null || purse == null) return;
			
			foreach (var shopItem in GetAllitems())
			{
				var item = shopItem.InventoryItem;
				var quantity = shopItem.QuantityInTransaction;
				var price = shopItem.Price;
				for (var i = 0; i < quantity; i++)
				{
					if (purse.Balance < price) break;
					var success = shopperInventory.AddToFirstEmptySlot(item, 1);
					if (success)
					{
						AddToTransaction(item, -1);
						purse.UpdateBalance(-price);
					}
				}
			}
		}

		public void SelectFilter(ItemCategory category)
		{
		}

		public ItemCategory GetFilter() => ItemCategory.None;

		public void SelectMode(bool isBuying)
		{
		}

		public void SetShopper(Shopper shopper) => _currentShopper = shopper;

		public bool IsBuyingMode => true;
		public bool CanTransact => true;

		public float TransactionTotal()
		{
			float total = 0;
			foreach (var item in GetAllitems())
			{
				total += item.Price * item.QuantityInTransaction;
			}

			return total;
		}

		public void AddToTransaction(InventoryItem item, int quantity)
		{
			if (!_transaction.ContainsKey(item)) _transaction[item] = 0;
			_transaction[item] += quantity;
			if (_transaction[item] <= 0) _transaction.Remove(item);
			OnChange?.Invoke();
		}

		#endregion

		#region Interface

		public CursorType GetCursorType() => CursorType.Shop;

		public bool HandleRaycast(GameObject player)
		{
			CheckPressedButtons(_currentShopper);
			return true;
		}

		private void CheckPressedButtons(Shopper shopper)
		{
			if (Input.GetKey(KeyCode.LeftControl))
			{
				if (Input.GetMouseButtonDown(0))
				{
					shopper.QueueInteractAction(gameObject);
				}
			}
			else
			{
				if (Input.GetMouseButton(0))
				{
					shopper.StartInteractAction(this);
				}
			}
		}

		public void ShowInteractivity() => _outlineableComponent.ShowOutline(this);

		public Transform GetTransform() => transform;

		public void Interact() => _currentShopper.SetActiveShop(this);

		public float InteractionDistance() => 1.5f;

		#endregion
	}

	[Serializable]
	internal class StockItemConfig
	{
		public InventoryItem item;
		public int initialStock;
		[Range(0, 100)] public float discountPercentage;
	}
}