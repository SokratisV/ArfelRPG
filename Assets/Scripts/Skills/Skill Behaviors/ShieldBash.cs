using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Core;
using UnityEngine;

namespace RPG.Skills.Behaviors
{
	[CreateAssetMenu(menuName = "Create ShieldBash", fileName = "ShieldBash", order = 0)]
	public class ShieldBash : SkillBehavior
	{
		[Min(0)] [SerializeField] private float damage;
		[SerializeField] private float bashRange;

		//State - breaks the stateless paradigm I've set so far, rip :(
		private Trigger _trigger;

		public override bool HasCastTime() => true;
		public override bool UseExtraAnimation() => false;
		public override int SkillAnimationNumber() => 0;
		public override float GetCastingRange() => bashRange;

		public override IEnumerator BehaviorUpdate(GameObject user, List<GameObject> targets, Vector3? point = null)
		{
			// yield return new WaitUntil(() => _trigger.Value);

			while (true)
			{
				if (_trigger.Value)
				{
					if (targets[0] != null)
					{
						var health = targets[0].GetComponent<Health>();
						health.TakeDamage(user, damage);
						yield break;
					}
				}
				else yield return null;
			}
		}

		public override void OnAnimationEvent()
		{
			_trigger.Value = true;
		}
	}
}