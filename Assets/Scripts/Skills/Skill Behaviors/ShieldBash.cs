using System.Collections;
using RPG.Attributes;
using RPG.Core;
using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public class ShieldBash : SkillBehavior
	{
		[Min(0)] [SerializeField] private float damage;
		[SerializeField] private float bashRange;
		private Trigger _trigger;

		public override bool HasCastTime() => true;
		public override int SkillAnimationNumber() => 0;
		public override float GetCastingRange() => bashRange;

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