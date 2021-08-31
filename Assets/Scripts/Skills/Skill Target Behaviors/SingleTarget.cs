using System.Collections.Generic;
using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public class SingleTarget : TargetBehavior
	{
		public override TargetType TargetType() => Behaviors.TargetType.Single;

		public override List<GameObject> GetTargets(GameObject user, GameObject initialTarget = null, Vector3? raycastPoint = null, Vector3? direction = null)
		{
			var targets = new List<GameObject> {user};
			if (initialTarget) targets.Add(initialTarget);
			return targets;
		}
	}
}