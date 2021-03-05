using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public class SingleTarget : TargetBehavior
	{
		public override bool RequireTarget() => true;

		public override GameObject[] GetTargets(GameObject user, GameObject target = null, Vector3? raycastPoint = null) => target == null? null:new[] {target};
	}
}