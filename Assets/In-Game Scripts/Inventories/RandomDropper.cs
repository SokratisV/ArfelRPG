using System;
using RPG.Stats;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace RPG.Inventories
{
	public class RandomDropper : ItemDropper
	{
		[Tooltip("How far pickups will be scattered")] [SerializeField]
		private float scatterDistance = 1;

		[SerializeField] private DropLibrary dropLibrary;
		[SerializeField] private int numberOfDrops = 2;

		private BaseStats _baseStats;
		private const int Attempts = 30;

		private void Awake() => _baseStats = GetComponent<BaseStats>();

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
			var item = dropLibrary.GetRandomDrops(_baseStats.GetLevel());
			foreach(var dropped in item)
			{
				DropItem(dropped.Item, dropped.Number);
			}
		}
	}
}