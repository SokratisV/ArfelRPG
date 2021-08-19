using System.Collections.Generic;
using UnityEngine;

namespace RPG.Skills.TargetFiltering
{
	[CreateAssetMenu(menuName = "RPG/Skills/Create TagFilter", fileName = "TagFilter")]
	public class TagFilter : FilterStrategy
	{
		[SerializeField] private string tag;

		public override List<GameObject> Filter(List<GameObject> unfiltered)
		{
			var filteredObjects = new List<GameObject>();
			foreach (var gameObject in unfiltered)
			{
				if (gameObject.CompareTag(tag)) filteredObjects.Add(gameObject);
			}

			return filteredObjects.Count > 0 ? filteredObjects : unfiltered;
		}
	}
}