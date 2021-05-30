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

		private Shopper _shopper = null;
		private OutlineableComponent _outlineableComponent;
		private Dictionary<InventoryItem, int> _transaction = new Dictionary<InventoryItem, int>();

		#region Unity

		public string ShopName => shopName;

		private void Awake() => _outlineableComponent = new OutlineableComponent(gameObject, GlobalValues.ShopInteractableColor);

		private void Start() => _shopper = Shopper.GetPlayerShopper();

		#endregion

		#region Public

		public IEnumerable<ShopItem> GetFilteredItems()
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
		}

		public void SelectFilter(ItemCategory category)
		{
		}

		public ItemCategory GetFilter() => ItemCategory.None;

		public void SelectMode(bool isBuying)
		{
		}

		public bool IsBuyingMode => true;
		public bool CanTransact => true;
		public float TransactionTotal() => 0;

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
			CheckPressedButtons(_shopper);
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

		public void Interact() => _shopper.SetActiveShop(this);

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