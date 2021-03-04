using System;
using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public abstract class SkillBehavior : ScriptableObject
	{
		public Action<GameObject, GameObject[]> OnStart, OnUpdate, OnEnd;
		public abstract void BehaviorStart(GameObject user, GameObject[] targets);
		public abstract void BehaviorUpdate(GameObject user, GameObject[] targets);
		public abstract void BehaviorEnd(GameObject user, GameObject[] targets);
	}
}