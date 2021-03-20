using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Core
{
	public static class Helper
	{
		public static bool IsWithinDistance(Vector3 position1, Vector3 position2, float distance) => (position1 - position2).sqrMagnitude <= distance * distance;

		public static bool IsWithinDistance(Transform transform1, Transform transform2, float distance) => IsWithinDistance(transform1.position, transform2.position, distance);

		public static bool FloatEquals(float value1, float value2) => Math.Abs(value1 - value2) < .001f;

		public static Vector3 CalculateMaximumDistanceNavMeshPoint(NavMeshPath path, float distance)
		{
			float soFar = 0;
			Vector3 finalPoint = default;
			for(var i = 0;i < path.corners.Length - 1;i++)
			{
				var segmentDistance = (path.corners[i + 1] - path.corners[i]).magnitude;
				if(soFar + segmentDistance <= distance)
				{
					soFar += segmentDistance;
				}
				else
				{
					finalPoint = path.corners[i] + (path.corners[i + 1] - path.corners[i]).normalized * (distance - soFar);
					break;
				}
			}

			return finalPoint;
		}

		public static void DoAfterSeconds(Action action, float seconds, MonoBehaviour mono) => mono.StartCoroutine(DoAfterSeconds(action, seconds));

		private static IEnumerator DoAfterSeconds(Action action, float seconds)
		{
			yield return new WaitForSeconds(seconds);
			action?.Invoke();
		}

		private static readonly Dictionary<KeyCode, string> NiceKeyCodeNames = new Dictionary<KeyCode, string>()
		{
			{KeyCode.Alpha0, "0"},
			{KeyCode.Alpha1, "1"},
			{KeyCode.Alpha2, "2"},
			{KeyCode.Alpha3, "3"},
			{KeyCode.Alpha4, "4"}
		};

		public static string KeyCodeName(KeyCode keyCode) => NiceKeyCodeNames.TryGetValue(keyCode, out var name)? name:"";

		public static float GetPathLength(NavMeshPath path)
		{
			var total = 0f;
			if(path.corners.Length < 2) return total;
			for(var i = 0;i < path.corners.Length - 1;i++)
			{
				total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
			}

			return total;
		}

		public static LayerMask CharactersMask => LayerMask.GetMask("Characters");
	}
}