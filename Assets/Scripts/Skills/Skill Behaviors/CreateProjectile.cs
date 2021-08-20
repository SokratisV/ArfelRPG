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
		[SerializeField] private Projectile projectile = null;
		[SerializeField] private float damage;
		[SerializeField] private float castRange;

		public override bool HasCastTime() => true;
		public override bool UseExtraAnimation() => true;
		public override int SkillAnimationNumber() => 0;
		public override float GetCastingRange() => castRange;

		public override void BehaviorStart(SkillData data)
		{
			if(data.Targets[0] != null)
			{
				data.User.GetComponent<Mover>().RotateOverTime(.2f, data.Targets[0].transform.position);
			}

			base.BehaviorStart(data);
		}

		public override IEnumerator BehaviorUpdate(SkillData data)
		{
			yield break;
		}

		public override void BehaviorEnd(SkillData data)
		{
			if(data.Targets[0] != null)
			{
				ExecuteBehavior(data.User, data.Targets);
			}

			base.BehaviorEnd(data);
		}

		private void ExecuteBehavior(GameObject user, List<GameObject> targets)
		{
			var projectileInstance = Instantiate(projectile, user.GetComponent<BodyParts>().ProjectileLocation.position, Quaternion.identity);
			projectileInstance.Setup(targets[0].GetComponent<Health>(), user, damage);
		}
	}
}