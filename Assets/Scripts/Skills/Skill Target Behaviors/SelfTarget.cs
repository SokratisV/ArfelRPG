using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public class SelfTarget : TargetBehavior
	{
		public override float GetMinRange() => -1;
		
		public override bool? RequireTarget() => null;

		public override bool GetTargets(out GameObject[] targets, GameObject user, GameObject initialTarget = null, Vector3? raycastPoint = null)
		{
			targets = user == null? null:targets = new[] {user};
			return true;
		}
	}
}