using System;
using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public abstract class SkillBehavior : ScriptableObject
	{
		public Action<GameObject, CustomTarget> OnStart, OnUpdate, OnEnd;
		public abstract void BehaviorStart(GameObject user, CustomTarget target);
		public abstract void BehaviorUpdate();
		public abstract void BehaviorEnd();
	}
}