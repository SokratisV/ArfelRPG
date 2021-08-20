using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Movement;
using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public class Explosion : SkillBehavior
	{
		[Min(0)] [SerializeField] private float damage;
		[SerializeField] private float castRange;

		public override float GetCastingRange() => castRange;
		public override bool HasCastTime() => true;
		public override bool UseExtraAnimation() => true;
		public override int SkillAnimationNumber() => 2;

		public override void BehaviorStart(SkillData data)
		{
			if (data.Point != null)
			{
				data.User.GetComponent<Mover>().RotateOverTime(.2f, data.Point.Value);
			}

			base.BehaviorStart(data);
		}

		public override IEnumerator BehaviorUpdate(SkillData data)
		{
			yield break;
		}

		public override void BehaviorEnd(SkillData data)
		{
			if (data.Targets != null)
			{
				for (var i = data.Targets.Count - 1; i >= 0; i--)
				{
					var target = data.Targets[i];
					if (target == data.User)
					{
						data.Targets[i] = null;
						continue;
					}
					if (target.TryGetComponent(out Health health))
					{
						RemoveHealthFromList(health, data.Targets);
						health.TakeDamage(data.User, damage);
					}
				}
			}

			base.BehaviorEnd(data);
		}
	}
}