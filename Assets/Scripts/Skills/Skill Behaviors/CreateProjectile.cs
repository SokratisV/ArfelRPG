using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Combat;
using RPG.Movement;
using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public class CreateProjectile : SkillBehavior
	{
		[SerializeField] private TargetedProjectile projectile = null;
		[SerializeField] private float damage;
		[SerializeField] private float castRange;

		public override bool HasCastTime() => true;
		public override bool UseExtraAnimation() => true;
		public override int SkillAnimationNumber() => 0;
		public override float GetCastingRange() => castRange;

		public override void BehaviorStart(GameObject user, List<GameObject> targets, Vector3? point = null)
		{
			if(targets[0] != null)
			{
				user.GetComponent<Mover>().RotateOverTime(.2f, targets[0].transform.position);
			}

			base.BehaviorStart(user, targets, point);
		}

		public override IEnumerator BehaviorUpdate(GameObject user, List<GameObject> targets, Vector3? point = null)
		{
			yield break;
		}

		public override void BehaviorEnd(GameObject user, List<GameObject> targets, Vector3? point = null)
		{
			if(targets[0] != null)
			{
				ExecuteBehavior(user, targets);
			}

			base.BehaviorEnd(user, targets, point);
		}

		private void ExecuteBehavior(GameObject user, List<GameObject> targets)
		{
			var projectileInstance = Instantiate(projectile, user.GetComponent<BodyParts>().ProjectileLocation.position, Quaternion.identity);
			projectileInstance.SetTarget(targets[0].GetComponent<Health>(), user, damage);
		}
	}
}