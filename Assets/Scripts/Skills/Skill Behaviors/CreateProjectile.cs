using System.Collections.Generic;
using RPG.Attributes;
using RPG.Combat;
using RPG.Movement;
using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public class CreateProjectile : SkillBehavior
	{
		[SerializeField] private Projectile projectile = null;
		[SerializeField] private float damage;
		[SerializeField] private float castRange;

		public override bool HasCastTime() => true;
		public override bool UseExtraAnimation() => false;
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

		public override void BehaviorUpdate(GameObject user, List<GameObject> targets, Vector3? point = null)
		{
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
			var projectileInstance = Instantiate(projectile);
			var transform = projectileInstance.transform;
			transform.position = user.transform.position;
			transform.rotation = user.transform.rotation;
			projectileInstance.SetTarget(targets[0].GetComponent<Health>(), user, damage);
		}
	}
}