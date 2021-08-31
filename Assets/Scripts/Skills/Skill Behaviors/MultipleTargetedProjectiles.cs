using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Combat;
using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public class MultipleTargetedProjectiles : SkillBehavior
	{
		[SerializeField] private Projectile projectile = null;
		[SerializeField] private float projectileSpeed;
		[SerializeField] private float damage;

		public override bool HasCastTime() => false;
		public override int SkillAnimationNumber() => 2;

		public override void BehaviorStart(SkillData data)
		{
			if(data.Targets != null)
			{
				ExecuteBehavior(data.Targets[0], data.Targets);
			}

			base.BehaviorStart(data);
		}

		public override IEnumerator BehaviorUpdate(SkillData data)
		{
			yield break;
		}

		private void ExecuteBehavior(GameObject user, List<GameObject> targets)
		{
			foreach(var target in targets)
			{
				if(target == user) continue;
				var projectileInstance = Instantiate(projectile, user.GetComponent<BodyParts>().ProjectileLocation.position, Quaternion.identity);
				projectileInstance.Setup(target.GetComponent<Health>(), user, damage, projectileSpeed);
			}
		}
	}
}