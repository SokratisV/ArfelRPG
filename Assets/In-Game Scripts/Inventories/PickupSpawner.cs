using UnityEngine;
using RPG.Saving;

namespace RPG.Inventories
{
	/// <summary>
	/// Spawns pickups that should exist on first load in a level. This
	/// automatically spawns the correct prefab for a given inventory item.
	/// </summary>
	public class PickupSpawner : MonoBehaviour, ISaveable
	{
		[SerializeField] private InventoryItem item = null;
		[SerializeField] private int number = 1;

		private void Awake() => SpawnPickup();

		/// <summary>
		/// Returns the pickup spawned by this class if it exists.
		/// </summary>
		/// <returns>Returns null if the pickup has been collected.</returns>
		public Pickup GetPickup() => GetComponentInChildren<Pickup>();

		/// <summary>
		/// True if the pickup was collected.
		/// </summary>
		public bool IsCollected() => GetPickup() == null;

		private void SpawnPickup()
		{
			var spawnedPickup = item.SpawnPickup(transform.position, number);
			spawnedPickup.transform.SetParent(transform);
		}

		private void DestroyPickup()
		{
			if(GetPickup())
			{
				Destroy(GetPickup().gameObject);
			}
		}

		object ISaveable.CaptureState() => IsCollected();

		void ISaveable.RestoreState(object state)
		{
			var shouldBeCollected = (bool)state;
			switch(shouldBeCollected)
			{
				case true when!IsCollected():
					DestroyPickup();
					break;
				case false when IsCollected():
					SpawnPickup();
					break;
			}
		}
	}
}