using System.Collections;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public class Thunderclap : SkillBehavior
	{
		[Min(0)] [SerializeField] private float damage;

		public override bool HasCastTime() => true;
		public override int SkillAnimationNumber() => 2;
		public override bool AdjustAnimationSpeed => false;

		public override void BehaviorStart(SkillData data)
		{
			if (data.Targets != null)
			{
				for (var i = data.Targets.Count - 1; i >= 0; i--)
				{
					var target = data.Targets[i];
					var health = target.GetComponent<Health>();
					RemoveHealthFromList(health, data.Targets);
					health.TakeDamage(data.User, damage);
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