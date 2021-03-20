using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public abstract class SkillBehavior : ScriptableObject
	{
		[SerializeField] private float duration;
		[SerializeField] private bool canTargetSelf;
		[SerializeField] private bool moveInRangeBeforeCasting = true;

		//If true, duration means casting time
		public abstract bool HasCastTime();
		public abstract bool UseExtraAnimation();
		public abstract int SkillAnimationNumber();
		public virtual float GetCastingRange() => 0;
		public bool CanTargetSelf => canTargetSelf;
		public bool MoveInRangeBefore => moveInRangeBeforeCasting;
		public float Duration => duration;
		public Action<GameObject, List<GameObject>> OnStart, OnEnd;

		public virtual void BehaviorStart(GameObject user, List<GameObject> targets, Vector3? point = null) => OnStart?.Invoke(user, targets);

		public abstract IEnumerator BehaviorUpdate(GameObject user, List<GameObject> targets, Vector3? point = null);

		public virtual void BehaviorEnd(GameObject user, List<GameObject> targets, Vector3? point = null)
		{
			OnEnd?.Invoke(user, targets);
			OnEnd = null;
			OnStart = null;
		}

		protected static void RemoveHealthFromList(Health health, List<GameObject> list)
		{
			void RemoveFromList()
			{
				health.OnDeath -= RemoveFromList;
				list.Remove(health.gameObject);
			}
			health.OnDeath += RemoveFromList;
		}
	}
}