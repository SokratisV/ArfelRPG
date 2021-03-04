using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public abstract class TargetBehavior : ScriptableObject
	{
		public abstract CustomTarget GetTarget(GameObject user);
	}

	public struct CustomTarget
	{
		public GameObject Target;
		public Vector3? Area;
		public Vector3? Direction;
		public float? Radius;

		public CustomTarget(GameObject target, Vector3? area, Vector3? direction, float? radius)
		{
			Target = target;
			Area = area;
			Direction = direction;
			Radius = radius;
		}
	}
}