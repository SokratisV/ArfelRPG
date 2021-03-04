﻿using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public abstract class TargetBehavior : ScriptableObject
	{
		public abstract bool RequireTarget();
		public abstract GameObject[] GetTargets(GameObject user, GameObject target = null, Vector3? raycastPoint = null);
	}
}