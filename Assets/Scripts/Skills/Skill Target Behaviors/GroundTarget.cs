﻿using System.Collections.Generic;
using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public class GroundTarget : TargetBehavior
	{
		public override TargetType TargetType() => Behaviors.TargetType.Point;

		public override List<GameObject> GetTargets(GameObject user, GameObject initialTarget = null, Vector3? raycastPoint = null, Vector3? direction = null)
		{
			var targets = new List<GameObject> {user};
			return targets;
		}
	}
}