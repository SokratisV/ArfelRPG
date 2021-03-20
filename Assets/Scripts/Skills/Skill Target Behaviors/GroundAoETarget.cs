using System.Collections.Generic;
using RPG.Core;
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
				var colliders = Physics.OverlapSphere(raycastPoint.Value, radius, Helper.CharactersMask);
				foreach(var collider in colliders)
				{
					targets.Add(collider.gameObject);
				}
			}

			return false;
		}
	}
}