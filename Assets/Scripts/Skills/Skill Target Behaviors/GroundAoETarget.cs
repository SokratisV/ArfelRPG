using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public class GroundAoETarget : TargetBehavior
	{
		[Min(0)] [SerializeField] private float radius;

		public override float GetRadius() => radius;
		public override bool? RequireTarget() => false;

		public override bool GetTargets(out List<GameObject> targets, GameObject user, GameObject initialTarget = null, Vector3? raycastPoint = null)
		{
			targets = new List<GameObject>();
			if(raycastPoint != null)
			{
				var colliders = Physics.OverlapSphere(raycastPoint.Value, radius);
				foreach(var collider in colliders)
				{
					if(collider.TryGetComponent(out CombatTarget ai))
					{
						targets.Add(ai.gameObject);
					}
				}
			}
			
			return false;
		}
	}
}