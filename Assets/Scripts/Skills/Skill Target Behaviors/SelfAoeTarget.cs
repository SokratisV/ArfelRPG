using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public class SelfAoeTarget : TargetBehavior
	{
		[Min(0)] [SerializeField] private float radius;
		public override TargetType TargetType() => Behaviors.TargetType.None;

		public override List<GameObject> GetTargets(GameObject user, GameObject initialTarget = null, Vector3? raycastPoint = null, Vector3? direction = null)
		{
			var targets = new List<GameObject> {user};
			var colliders = Physics.OverlapSphere(user.transform.position, radius);
			foreach (var collider in colliders)
			{
				if (collider.TryGetComponent(out CombatTarget ai))
				{
					targets.Add(ai.gameObject);
				}
			}

			return targets;
		}
	}
}