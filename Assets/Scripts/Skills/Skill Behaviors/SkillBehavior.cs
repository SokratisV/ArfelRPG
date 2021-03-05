using System;
using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public abstract class SkillBehavior : ScriptableObject
	{
		public Action<GameObject, GameObject[]> OnStart, OnUpdate, OnEnd;

		public virtual void BehaviorStart(GameObject user, GameObject[] targets) => OnStart?.Invoke(user, targets);

		public virtual void BehaviorUpdate(GameObject user, GameObject[] targets) => OnUpdate?.Invoke(user, targets);

		public virtual void BehaviorEnd(GameObject user, GameObject[] targets)
		{
			OnEnd?.Invoke(user, targets);
			OnEnd = null;
			OnStart = null;
			OnUpdate = null;
		}
	}
}