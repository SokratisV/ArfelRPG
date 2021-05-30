using System;
using System.Collections.Generic;
using RPG.Core;
using RPG.Inventories;
using UnityEngine;

namespace RPG.Shops
{
	public class Shop : MonoBehaviour, IRaycastable, IInteractable
	{
		[Serializable]
		public class ShopItem
		{
			public InventoryItem item;
			public int availability;
			public float price;
			public int quantityInTransaction;
		}

		[SerializeField] private string shopName;

		public event Action OnChange;

		private Shopper _shopper = null;
		private OutlineableComponent _outlineableComponent;

		#region Unity

		public string ShopName => shopName;

		private void Awake() => _outlineableComponent = new OutlineableComponent(gameObject, GlobalValues.ShopInteractableColor);

		private void Start() => _shopper = Shopper.GetPlayerShopper();

		#endregion

		#region Public

		public IEnumerable<ShopItem> GetFilteredItems()
		{
			return null;
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
}