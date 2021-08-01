using System;
using RPG.Saving;
using UnityEngine;

namespace RPG.Inventories
{
	public class Purse : MonoBehaviour, ISaveable, IItemStore
	{
		public event Action<float> OnChange;
		[SerializeField] private float startingBalance = 400f;

		public float Balance { get; private set; } = 0;

		public void UpdateBalance(float amount)
		{
			Balance += amount;
			OnChange?.Invoke(amount);
		}
		
		public int AddItems(InventoryItem item, int number)
		{
			if (item is CurrencyItem)
			{
				UpdateBalance(item.Price * number);
				return number;
			}

			return 0;
		}

		private void Awake() => Balance = startingBalance;

		public object CaptureState() => Balance;

		public void RestoreState(object state) => Balance = (float) state;

	}
}