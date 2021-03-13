using System;
using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public abstract class SkillBehavior : ScriptableObject
	{
		[SerializeField] private float duration;
		[SerializeField] private bool canTargetSelf;
		[SerializeField] private bool moveInRangeBeforeCasting = true;

		public abstract bool UseExtraAnimation();
		public abstract int SkillAnimationNumber();
		public virtual float GetCastingRange() => 0;
		public bool CanTargetSelf => canTargetSelf;
		public bool MoveInRangeBefore => moveInRangeBeforeCasting;
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