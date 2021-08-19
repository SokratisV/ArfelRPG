using System.Collections.Generic;
using UnityEngine;

namespace RPG.Skills.TargetFiltering
{
	public abstract class FilterStrategy : ScriptableObject
	{
		public abstract List<GameObject> Filter(List<GameObject> unfiltered);
	}
}