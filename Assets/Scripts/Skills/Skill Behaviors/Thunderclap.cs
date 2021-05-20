using System.Collections;
using System.Collections.Generic;
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

		public override void BehaviorStart(GameObject user, List<GameObject> targets, Vector3? point = null)
		{
			if (targets != null)
			{
				for (var i = targets.Count - 1; i >= 0; i--)
				{
					var target = targets[i];
					var health = target.GetComponent<Health>();
					RemoveHealthFromList(health, targets);
					health.TakeDamage(user, damage);
				}
			}

			base.BehaviorStart(user, targets, point);
		}

		public override IEnumerator BehaviorUpdate(GameObject user, List<GameObject> targets, Vector3? point = null)
		{
			yield break;
		}
	}
}