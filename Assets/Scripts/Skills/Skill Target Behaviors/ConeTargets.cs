using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public class ConeTargets : TargetBehavior
	{
		[Min(0)] [SerializeField] private float radius;
		[Range(0, 180)] [SerializeField] private float angle;
		public override float GetRadius() => radius;
		public override TargetType TargetType() => Behaviors.TargetType.Direction;

		public override List<GameObject> GetTargets(GameObject user, GameObject initialTarget = null, Vector3? raycastPoint = null, Vector3? direction = null)
		{
			var targets = new List<GameObject> {user};
			if (initialTarget) targets.Add(initialTarget);
			if (raycastPoint == null) return targets;
			
			var userPosition = user.transform.position;
			var dir = (raycastPoint.Value - userPosition).normalized;
			var colliders = Physics.OverlapSphere(userPosition, radius, Helper.CharactersMask);
			foreach (var collider in colliders)
			{
				var directionToCollider = (collider.transform.position - userPosition).normalized;
				var dotProduct = Vector3.Dot(directionToCollider, dir);
				if (dotProduct >= Mathf.Cos(angle))
				{
					targets.Add(collider.gameObject);
				}
			}

			return targets;
		}
	}
}