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
					var health = data.Targets[1].GetComponent<Health>();
					health.TakeDamage(data.Targets[0], damage);
					yield break;
				}

				yield return null;
			}
		}

		public override void OnAnimationEvent() => _trigger.Value = true;
	}
}