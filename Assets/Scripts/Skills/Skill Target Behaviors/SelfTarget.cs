using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public class SelfTarget : TargetBehavior
	{
		public override bool RequireTarget() => false;

		public override GameObject[] GetTargets(GameObject user, GameObject target = null, Vector3? _ = null) => user == null? null:new[] {user};
	}
}