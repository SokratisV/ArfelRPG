using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Skills
{
	public class SkillData
	{
		public readonly GameObject User;
		public readonly GameObject InitialTarget;
		public readonly List<GameObject> Targets;
		public Vector3? Point;

		public SkillData(GameObject user, GameObject initialTarget, Vector3? point, List<GameObject> targets)
		{
			User = user;
			InitialTarget = initialTarget;
			Point = point;
			Targets = targets;
		}
	}
}