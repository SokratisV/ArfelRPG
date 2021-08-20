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
			return unfiltered.Where(gameObject => gameObject.CompareTag(tag)).ToList();
		}
	}
}