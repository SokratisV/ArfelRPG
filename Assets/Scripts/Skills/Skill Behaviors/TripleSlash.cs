using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Core;
using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public class TripleSlash : SkillBehavior
	{
		[Min(0)] [SerializeField] private float damage;
		[SerializeField] private float attackRange;
		private Trigger _trigger;

		public override bool HasCastTime() => true;
		public override float GetCastingRange() => attackRange;
		public override int SkillAnimationNumber() => 1;

		public override IEnumerator BehaviorUpdate(GameObject user, List<GameObject> targets, Vector3? point = null)
		{
			while (true)
			{
				if (_trigger.Value)
				{
					if (targets[0] != null)
					{
						var health = targets[0].GetComponent<Health>();
						health.TakeDamage(user, damage);
						yield return null;
					}
				}
				else yield return null;
			}
		}

		public override void OnAnimationEvent() => _trigger.Value = true;
	}
}