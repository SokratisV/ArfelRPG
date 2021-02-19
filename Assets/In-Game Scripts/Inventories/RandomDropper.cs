using UnityEngine;
using UnityEngine.AI;

namespace RPG.Inventories
{
	public class RandomDropper : ItemDropper
	{
		[Tooltip("How far pickups will be scattered")] [SerializeField]
		private float scatterDistance = 1;

		[SerializeField] private InventoryItem[] dropLibrary;
		[SerializeField] private int numberOfDrops = 2;

		private const int Attempts = 30;

		protected override Vector3 GetDropLocation()
		{
			for(var i = 0;i < Attempts;i++)
			{
				var randomPoint = transform.position + Random.insideUnitSphere * scatterDistance;
				if(NavMesh.SamplePosition(randomPoint, out var hit, 0.1f, NavMesh.AllAreas))
				{
					return hit.position;
				}
			}

			return transform.position;
		}

		public void RandomDrop()
		{
			for(var i = 0;i < numberOfDrops;i++)
			{
				var item = dropLibrary[Random.Range(0, dropLibrary.Length)];
				DropItem(item, 1);
			}
		}
	}
}