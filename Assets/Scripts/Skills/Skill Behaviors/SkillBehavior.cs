using System;
using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public abstract class SkillBehavior : ScriptableObject
	{
		[SerializeField] private float duration;
		[SerializeField] private bool canTargetSelf;

		public bool CanTargetSelf => canTargetSelf;

		public float Duration => duration;

		public Action<GameObject, GameObject[]> OnStart, OnUpdate, OnEnd;

		public virtual void BehaviorStart(GameObject user, GameObject[] targets, Vector3? point = null) => OnStart?.Invoke(user, targets);

		public virtual void BehaviorUpdate(GameObject user, GameObject[] targets, Vector3? point = null) => OnUpdate?.Invoke(user, targets);

		public virtual void BehaviorEnd(GameObject user, GameObject[] targets, Vector3? point = null)
		{
			OnEnd?.Invoke(user, targets);
			OnEnd = null;
			OnStart = null;
			OnUpdate = null;
		}
	}
}