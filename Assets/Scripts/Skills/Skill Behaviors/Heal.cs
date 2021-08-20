using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public class Heal : SkillBehavior
	{
		[SerializeField] private float amountToHeal;
		[SerializeField] private float castRange;

		public override bool HasCastTime() => true;
		public override bool UseExtraAnimation() => true;
		public override int SkillAnimationNumber() => 3;

		public override float GetCastingRange() => castRange;

		public override void BehaviorStart(SkillData data)
		{
			if(data.Targets[0].TryGetComponent(out Health health))
			{
				if(!health.IsDead && !(health.GetPercentage() >= 100.0f))
				{
					health.Heal(data.User, amountToHeal);
				}
			}

			base.BehaviorStart(data);
		}

		public override IEnumerator BehaviorUpdate(SkillData data)
		{
			yield break;
		}
	}
}