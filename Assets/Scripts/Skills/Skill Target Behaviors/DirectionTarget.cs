using System.Collections.Generic;
using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public class DirectionTarget : TargetBehavior
	{
		public override TargetType TargetType() => Behaviors.TargetType.Direction;

		public override List<GameObject> GetTargets(GameObject user, GameObject initialTarget = null, Vector3? raycastPoint = null, Vector3? direction = null)
		{
			return new List<GameObject> { user };
		}
	}
}