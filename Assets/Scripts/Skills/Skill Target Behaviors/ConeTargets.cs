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
		public override bool? RequireTarget() => false;

		public override bool GetTargets(out List<GameObject> targets, GameObject user, GameObject initialTarget = null, Vector3? raycastPoint = null)
		{
			targets = new List<GameObject>();
			if(raycastPoint != null)
			{
				var userPosition = user.transform.position;
				var direction = (raycastPoint.Value - userPosition).normalized;
				var colliders = Physics.OverlapSphere(userPosition, radius, Helper.CharactersMask);
				foreach(var collider in colliders)
				{
					var directionToCollider = (collider.transform.position - userPosition).normalized;
					var dotProduct = Vector3.Dot(directionToCollider, direction);
					if(dotProduct >= Mathf.Cos(angle))
					{
						targets.Add(collider.gameObject);
					}
				}
			}

			return false;
		}
	}
}