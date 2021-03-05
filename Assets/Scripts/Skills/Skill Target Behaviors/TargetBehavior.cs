using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public abstract class TargetBehavior : ScriptableObject
	{
		public abstract bool RequireTarget();

		//Should return null if it can't find the target it needs
		public abstract GameObject[] GetTargets(GameObject user, GameObject target = null, Vector3? raycastPoint = null);
	}
}