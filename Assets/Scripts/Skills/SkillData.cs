using System.Collections.Generic;
using UnityEngine;

namespace RPG.Skills
{
	public class SkillData
	{
		public readonly List<GameObject> Targets;
		public Vector3? Point, Direction;

		public SkillData(Vector3? point, Vector3? direction, List<GameObject> targets)
		{
			Direction = direction;
			Point = point;
			Targets = targets;
		}
	}
}