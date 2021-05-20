using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Combat;
using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public class MultipleTargetedProjectiles : SkillBehavior
	{
		[SerializeField] private TargetedProjectile projectile = null;
		[SerializeField] private float projectileSpeed;
		[SerializeField] private float damage;

		public override bool HasCastTime() => false;
		public override int SkillAnimationNumber() => 2;

		public override void BehaviorStart(GameObject user, List<GameObject> targets, Vector3? point = null)
		{
			if(targets != null)
			{
				ExecuteBehavior(user, targets);
			}

			base.BehaviorStart(user, targets, point);
		}

		public override IEnumerator BehaviorUpdate(GameObject user, List<GameObject> targets, Vector3? point = null)
		{
			yield break;
		}

		private void ExecuteBehavior(GameObject user, List<GameObject> targets)
		{
			foreach(var target in targets)
			{
				if(target == user) continue;
				var projectileInstance = Instantiate(projectile, user.GetComponent<BodyParts>().ProjectileLocation.position, Quaternion.identity);
				projectileInstance.SetTarget(target.GetComponent<Health>(), user, damage, projectileSpeed);
			}
		}
	}
}