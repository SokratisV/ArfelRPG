using System.Collections.Generic;
using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public class SelfTarget : TargetBehavior
	{
		public override float GetMinRange() => -1;

		public override TargetType TargetType() => Behaviors.TargetType.None;

		public override bool GetTargets(out List<GameObject> targets, GameObject user, GameObject initialTarget = null, Vector3? raycastPoint = null)
		{
			targets = user == null? null:targets = new List<GameObject> {user};
			return true;
		}
	}
}