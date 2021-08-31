using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public class GroundAoETarget : TargetBehavior
	{
		[Min(0)] [SerializeField] private float radius;

		public override float GetRadius() => radius;
		public override TargetType TargetType() => Behaviors.TargetType.Point;

		public override List<GameObject> GetTargets(GameObject user, GameObject initialTarget = null, Vector3? raycastPoint = null, Vector3? direction = null)
		{
			var targets = new List<GameObject> { user };
			if (initialTarget) targets.Add(initialTarget);
			if (raycastPoint != null)
			{
				var colliders = Physics.OverlapSphere(raycastPoint.Value, radius, Helper.CharactersMask);
				foreach (var collider in colliders)
				{
					if (collider.gameObject == user) continue;
					targets.Add(collider.gameObject);
				}
			}

			return targets;
		}
	}
}