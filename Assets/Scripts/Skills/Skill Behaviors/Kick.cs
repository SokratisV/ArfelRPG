﻿using System.Collections;
using RPG.Attributes;
using RPG.Core;
using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public class Kick : SkillBehavior
	{
		[Min(0)] [SerializeField] private float damage;
		[Min(0)] [SerializeField] private float kickRange;

		private Trigger _trigger;

		public override bool HasCastTime() => true;
		public override int SkillAnimationNumber() => 3;
		public override float GetCastingRange() => kickRange;
		public override bool AdjustAnimationSpeed => false;

		public override IEnumerator BehaviorUpdate(SkillData data)
		{
			while (true)
			{
				if (_trigger.Value)
				{
					if (data.Targets[0] != null)
					{
						var health = data.Targets[0].GetComponent<Health>();
						health.TakeDamage(data.User, damage);
						yield break;
					}
				}
				else yield return null;
			}
		}

		public override void OnAnimationEvent() => _trigger.Value = true;
	}
}