using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public abstract class TargetBehavior : ScriptableObject
	{
		//-1 is unlimited range
		[Range(-1, 20)] [SerializeField] protected float range;

		/// <summary>
		/// true = requires target, false = requires point, null = self cast
		/// </summary>
		public abstract bool? RequireTarget();

		public float GetRange() => range;

		//Return false when it only cares about the point
		public abstract bool GetTargets(out GameObject[] targets, GameObject user, GameObject initialTarget = null, Vector3? raycastPoint = null);
	}
}