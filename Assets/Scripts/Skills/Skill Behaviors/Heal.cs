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

		public override void BehaviorStart(GameObject user, GameObject[] targets, Vector3? point = null)
		{
			if(targets[0].TryGetComponent(out Health health))
			{
				if(!health.IsDead && !(health.GetPercentage() >= 100.0f))
				{
					health.Heal(amountToHeal);
				}
			}

			base.BehaviorStart(user, targets, point);
		}

		public override void BehaviorUpdate(GameObject user, GameObject[] targets, Vector3? point = null)
		{
		}
	}
}