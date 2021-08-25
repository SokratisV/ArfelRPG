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
		[SerializeField] private IndicatorType indicatorType;

		public virtual float[] SpecialFloats() => new float[0];
		public IndicatorType IndicatorType => indicatorType;
		//If true, duration means casting time
		public abstract bool HasCastTime();
		public virtual bool UseExtraAnimation() => false;
		public abstract int SkillAnimationNumber();
		public virtual float GetCastingRange() => 0;
		public bool CanTargetSelf => canTargetSelf;
		public bool MoveInRangeBefore => moveInRangeBeforeCasting;
		public float Duration => duration;
		public Action<SkillData> OnStart, OnEnd;
		public virtual bool AdjustAnimationSpeed => true;

		public virtual void BehaviorStart(SkillData data) => OnStart?.Invoke(data);

		public abstract IEnumerator BehaviorUpdate(SkillData data);

		public virtual void BehaviorEnd(SkillData data)
		{
			OnEnd?.Invoke(data);
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

		public virtual void OnAnimationEvent()
		{
		}
	}
}