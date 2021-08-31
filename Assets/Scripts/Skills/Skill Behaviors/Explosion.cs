using System.Collections;
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
				data.Targets[0].GetComponent<Mover>().RotateToTarget(.2f, data.Point.Value);
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
				for (var i = data.Targets.Count - 1; i >= 1; i--)
				{
					var target = data.Targets[i];
					if (target == data.Targets[0]) continue;
					if (target.TryGetComponent(out Health health))
					{
						RemoveHealthFromList(health, data.Targets);
						health.TakeDamage(data.Targets[0], damage);
					}
				}
			}

			base.BehaviorEnd(data);
		}
	}
}