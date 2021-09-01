using System.Collections.Generic;
using UnityEngine;

namespace RPG.Skills
{
	public class SkillData
	{
		public readonly List<GameObject> Targets;
		public Vector3? Point, Direction;
		public readonly bool CanBeForceCancelled;

		public SkillData(Vector3? point, Vector3? direction, List<GameObject> targets, bool canBeForceCancelled = false)
		{
			Direction = direction;
			Point = point;
			Targets = targets;
			CanBeForceCancelled = canBeForceCancelled;
		}
	}
}