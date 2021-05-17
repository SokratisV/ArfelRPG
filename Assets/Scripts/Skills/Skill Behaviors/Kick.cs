using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public class Kick : SkillBehavior
	{
		[Min(0)] [SerializeField] private float damage;
		[Min(0)] [SerializeField] private float kickRange;

		public override bool HasCastTime() => true;
		public override bool UseExtraAnimation() => false;
		public override int SkillAnimationNumber() => 3;
		public override float GetCastingRange() => kickRange;

		public override void BehaviorEnd(GameObject user, List<GameObject> targets, Vector3? point = null)
		{
			if (targets[0] != null)
			{
				var health = targets[0].GetComponent<Health>();
				health.TakeDamage(user, damage);
			}
			
			base.BehaviorStart(user, targets, point);
		}

		public override IEnumerator BehaviorUpdate(GameObject user, List<GameObject> targets, Vector3? point = null)
		{
			yield break;
		}
	}
}