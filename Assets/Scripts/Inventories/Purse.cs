using System;
using RPG.Saving;
using UnityEngine;

namespace RPG.Inventories
{
	public class Purse : MonoBehaviour, ISaveable
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
		
		public object CaptureState() => _balance;

		public void RestoreState(object state) => _balance = (float) state;
	}
}