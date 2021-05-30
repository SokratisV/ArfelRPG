using System;
using UnityEngine;

namespace RPG.Inventories
{
	public class Purse : MonoBehaviour
	{
		public event Action<float> OnChange;
		[SerializeField] private float startingBalance = 400f;

		private float _balance = 0;

		public float Balance => _balance;
		public void UpdateBalance(float amount)
		{
			_balance += amount;
			OnChange?.Invoke(amount);
		}

		private void Awake() => _balance = startingBalance;
	}
}