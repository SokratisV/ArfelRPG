using RPG.Core;
using RPG.Stats;
using UnityEngine;

namespace RPG.Inventories
{
	public class RandomDropper : ItemDropper
	{
		[Tooltip("How far pickups will be scattered")] [SerializeField]
		private float scatterDistance = 1;

		[SerializeField] private DropLibrary dropLibrary;
		private BaseStats _baseStats;
		private const int Attempts = 30;

		private void Awake() => _baseStats = GetComponent<BaseStats>();

		protected override Vector3 GetDropLocation()
		{
			for(var i = 0;i < Attempts;i++)
			{
				if (Helper.RandomPointAroundNavMesh(out var outcome, transform.position, scatterDistance))
				{
					return outcome;
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