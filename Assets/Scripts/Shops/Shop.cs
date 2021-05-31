using System;
using System.Collections.Generic;
using RPG.Core;
using RPG.Inventories;
using RPG.Stats;
using UnityEngine;

namespace RPG.Shops
{
	public class Shop : MonoBehaviour, IRaycastable, IInteractable
	{
		[SerializeField] private string shopName;
		[Range(0, 100)] [SerializeField] private float sellingPercent = 80f;
		[SerializeField] private StockItemConfig[] stockConfig;

		public event Action OnChange;

		private bool _isBuyingMode = true;
		private ItemCategory _filter = ItemCategory.None;
		private Shopper _currentShopper = null;
		private Purse _purse = null;
		private Inventory _shopperInventory = null;
		private BaseStats _stats = null;
		private OutlineableComponent _outlineableComponent;
		private Dictionary<InventoryItem, int> _transaction = new Dictionary<InventoryItem, int>();
		private Dictionary<InventoryItem, int> _stock = new Dictionary<InventoryItem, int>();

		public string ShopName => shopName;

		public bool IsBuyingMode
		{
			get => _isBuyingMode;
			set
			{
				_isBuyingMode = value;
				OnChange?.Invoke();
			}
		}

		#region Unity

		private void Awake()
		{
			_outlineableComponent = new OutlineableComponent(gameObject, GlobalValues.ShopInteractableColor);
			foreach (var config in stockConfig)
			{
				_stock[config.item] = config.initialStock;
			}
		}

		private void Start()
		{
			_currentShopper = Shopper.GetPlayerShopper();
			_purse = _currentShopper.GetComponent<Purse>();
			_shopperInventory = _currentShopper.GetComponent<Inventory>();
			_stats = _currentShopper.GetComponent<BaseStats>();
		}

		#endregion

		#region Public

		public IEnumerable<ShopItem> GetFilteredItems()
		{
			foreach (var shopItem in GetAllItems())
			{
				if (_filter == ItemCategory.None || shopItem.InventoryItem.Category == _filter) yield return shopItem;
			}
		}

		public IEnumerable<ShopItem> GetAllItems()
		{
			var shopperLevel = GetShopperLevel();
			foreach (var stockItem in stockConfig)
			{
				if (stockItem.levelToUnlock > shopperLevel) continue;
				var price = GetPrice(stockItem);
				_transaction.TryGetValue(stockItem.item, out var quantity);
				var availability = GetAvailability(stockItem.item);
				yield return new ShopItem(stockItem.item, availability, price, quantity);
			}
		}

		public void ConfirmTransaction()
		{
			if (_shopperInventory == null || _purse == null) return;
			foreach (var shopItem in GetAllItems())
			{
				var item = shopItem.InventoryItem;
				var quantity = shopItem.QuantityInTransaction;
				var price = shopItem.Price;
				for (var i = 0; i < quantity; i++)
				{
					if (_isBuyingMode)
					{
						BuyItem(price, item);
					}
					else
					{
						SellItem(price, item);
					}
				}
			}

			OnChange?.Invoke();
		}

		public void SelectFilter(ItemCategory category)
		{
			_filter = category;
			OnChange?.Invoke();
		}

		public ItemCategory GetFilter() => _filter;
		
		public bool CanTransact()
		{
			if (IsTransactionEmpty()) return false;
			if (!HasSufficientFunds()) return false;
			if (!HasInventorySpace()) return false;
			return true;
		}

		public bool HasInventorySpace()
		{
			if (!_isBuyingMode) return true;
			if (_shopperInventory == null) return false;
			var flatItems = new List<InventoryItem>();
			foreach (var shopItem in GetAllItems())
			{
				var item = shopItem.InventoryItem;
				var quantity = shopItem.QuantityInTransaction;
				for (var i = 0; i < quantity; i++)
				{
					flatItems.Add(item);
				}
			}

			return _shopperInventory.HasSpaceFor(flatItems);
		}

		public float TransactionTotal()
		{
			float total = 0;
			foreach (var item in GetAllItems())
			{
				total += item.Price * item.QuantityInTransaction;
			}

			return total;
		}

		public void AddToTransaction(InventoryItem item, int quantity)
		{
			if (!_transaction.ContainsKey(item)) _transaction[item] = 0;
			int availability = GetAvailability(item);
			if (_transaction[item] + quantity > availability)
			{
				_transaction[item] = availability;
			}
			else
			{
				_transaction[item] += quantity;
			}

			if (_transaction[item] <= 0) _transaction.Remove(item);
			OnChange?.Invoke();
		}

		public bool HasSufficientFunds()
		{
			if (!_isBuyingMode) return true;
			if (_purse == null) return false;
			return _purse.Balance >= TransactionTotal();
		}

		#endregion

		#region Interface

		public CursorType GetCursorType() => CursorType.Shop;

		public bool HandleRaycast(GameObject player)
		{
			CheckPressedButtons(_currentShopper);
			return true;
		}

		public void ShowInteractivity() => _outlineableComponent.ShowOutline(this);

		public Transform GetTransform() => transform;

		public void Interact() => _currentShopper.SetActiveShop(this);

		public float InteractionDistance() => 1.5f;

		#endregion

		#region Private

		private int GetAvailability(InventoryItem item) => _isBuyingMode ? _stock[item] : CountItemsInInventory(item);

		private int CountItemsInInventory(InventoryItem item)
		{
			if (_shopperInventory == null) return 0;
			var total = 0;
			var inventorySize = _shopperInventory.GetSize();
			for (var i = 0; i < inventorySize; i++)
			{
				if (_shopperInventory.GetItemInSlot(i) == item)
				{
					total += _shopperInventory.GetNumberInSlot(i);
				}
			}

			return total;
		}

		private float GetPrice(StockItemConfig stockItem)
		{
			if (_isBuyingMode) return stockItem.item.Price * (1 - stockItem.discountPercentage / 100);
			return stockItem.item.Price * (sellingPercent / 100);
		}

		private void CheckPressedButtons(Shopper shopper)
		{
			if (Input.GetKey(KeyCode.LeftControl))
			{
				if (Input.GetMouseButtonDown(0))
				{
					shopper.QueueAction(new InteractableActionData(shopper, transform));
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

		private bool IsTransactionEmpty() => _transaction.Count == 0;

		private void SellItem(float price, InventoryItem item)
		{
			var slot = FindFirstItemSlot(item);
			if (slot == -1) return;
			AddToTransaction(item, -1);
			_shopperInventory.RemoveFromSlot(slot, 1);
			_stock[item]++;
			_purse.UpdateBalance(price);
		}

		private int FindFirstItemSlot(InventoryItem item)
		{
			var inventorySize = _shopperInventory.GetSize();
			for (var i = 0; i < inventorySize; i++)
			{
				if (_shopperInventory.GetItemInSlot(i) == item) return i;
			}

			return -1;
		}

		private void BuyItem(float price, InventoryItem item)
		{
			if (_purse.Balance < price) return;
			var success = _shopperInventory.AddToFirstEmptySlot(item, 1);
			if (success)
			{
				AddToTransaction(item, -1);
				_stock[item]--;
				_purse.UpdateBalance(-price);
			}
		}

		private int GetShopperLevel() => _stats == null ? 0 : _stats.GetLevel();

		#endregion
	}

	[Serializable]
	internal class StockItemConfig
	{
		public InventoryItem item;
		public int initialStock;
		[Range(0, 100)] public float discountPercentage;
		[Range(0, GlobalValues.MaxLevel)] public int levelToUnlock;
	}
}