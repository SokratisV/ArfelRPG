using System;
using UnityEngine;

namespace RPG.Core
{
	public static class Helper
	{
		public static bool IsWithinDistance(Vector3 position1, Vector3 position2, float distance)
		{
			return(position1 - position2).sqrMagnitude <= distance * distance;
		}

		public static bool IsWithinDistance(Transform transform1, Transform transform2, float distance)
		{
			return IsWithinDistance(transform1.position, transform2.position, distance);
		}
		
		public static bool FloatEquals(float value1, float value2) => Math.Abs(value1 - value2) < .001f;
	}
}