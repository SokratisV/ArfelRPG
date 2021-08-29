using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public class SelfAoeTarget : TargetBehavior
	{
		[Min(0)] [SerializeField] private float radius;
		public override TargetType TargetType() => Behaviors.TargetType.None;

		public override bool GetTargets(out List<GameObject> targets, GameObject user, GameObject initialTarget = null, Vector3? raycastPoint = null)
		{
			targets = new List<GameObject>();
			var colliders = Physics.OverlapSphere(user.transform.position, radius);
			foreach(var collider in colliders)
			{
				if(collider.TryGetComponent(out CombatTarget ai))
				{
					targets.Add(ai.gameObject);
				}
			}
			
			return false;
		}
	}
}