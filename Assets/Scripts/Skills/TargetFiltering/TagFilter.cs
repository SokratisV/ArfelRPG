using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Skills.TargetFiltering
{
	[CreateAssetMenu(menuName = "RPG/Skills/Create TagFilter", fileName = "TagFilter")]
	public class TagFilter : FilterStrategy
	{
		[SerializeField] private string tag;

		public override List<GameObject> Filter(List<GameObject> unfiltered)
		{
			var filtered = new List<GameObject> {unfiltered[0]};
			foreach (var gameObject in unfiltered)
			{
				if (gameObject.CompareTag(tag)) filtered.Add(gameObject);
			}

			return filtered;
		}
	}
}