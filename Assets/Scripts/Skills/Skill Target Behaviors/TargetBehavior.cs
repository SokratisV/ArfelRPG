using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public abstract class TargetBehavior : ScriptableObject
	{
		/// <summary>
		/// true = requires target, false = requires point, null = self cast
		/// </summary>
		public abstract bool? RequireTarget();

		//Return false when it only cares about the point
		public abstract bool GetTargets( out GameObject[] targets, GameObject user, GameObject initialTarget = null, Vector3? raycastPoint = null);
	}
}