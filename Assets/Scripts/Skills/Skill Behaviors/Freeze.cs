using System.Collections;
using RPG.Attributes;
using RPG.Movement;
using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public class Freeze : SkillBehavior
	{
		[Min(0)] [SerializeField] private float damage;
		[Range(0, 100)] [SerializeField] private float slowPercent;

		public override bool HasCastTime() => false;
		public override int SkillAnimationNumber() => 1;

		public override void BehaviorStart(SkillData data)
		{
			data.Targets[0].GetComponent<Mover>().LockMovementFor(.5f);
			if (data.Targets != null)
			{
				for (var i = data.Targets.Count - 1; i >= 1; i--)
				{
					var target = data.Targets[i];
					var mover = target.GetComponent<Mover>();
					mover.CurrentSpeed = mover.CurrentSpeed * slowPercent * 0.01f;
					var health = target.GetComponent<Health>();
					RemoveHealthFromList(health, data.Targets);
					health.TakeDamage(data.Targets[0], damage);
				}
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
				foreach (var target in data.Targets)
				{
					if (target == data.Targets[0]) continue;
					target.GetComponent<Mover>().RevertToOriginalSpeed();
				}
			}

			base.BehaviorEnd(data);
		}
	}
}