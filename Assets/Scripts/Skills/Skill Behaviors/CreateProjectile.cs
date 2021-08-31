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
			if (data.Targets.Count >= 2)
			{
				data.Targets[0].GetComponent<Mover>().RotateToTarget(.2f, data.Targets[1].transform.position);
			}
			else if (data.Direction.HasValue)
			{
				data.Targets[0].GetComponent<Mover>().RotateToDirection(.2f, data.Direction.Value);
			}
			else if (data.Point.HasValue)
			{
				data.Targets[0].GetComponent<Mover>().RotateToDirection(.2f, data.Point.Value);
			}

			base.BehaviorStart(data);
		}

		public override IEnumerator BehaviorUpdate(SkillData data)
		{
			yield break;
		}

		public override void BehaviorEnd(SkillData data)
		{
			ExecuteBehavior(data.Targets[0], data);
			base.BehaviorEnd(data);
		}

		private void ExecuteBehavior(GameObject user, SkillData data)
		{
			var projectileInstance = Instantiate(projectile, user.GetComponent<BodyParts>().ProjectileLocation.position, Quaternion.identity);
			if (data.Targets.Count >= 2)
			{
				projectileInstance.Setup(data.Targets[1].GetComponent<Health>(), user, damage);
			}
			else if (data.Direction.HasValue)
			{
				projectileInstance.Setup(data.Direction.Value, user, damage);
			}
			else if (data.Point.HasValue)
			{
				var direction = data.Point.Value - data.Targets[0].transform.position;
				direction.y = 0;
				projectileInstance.Setup(direction.normalized, user, damage);
			}
		}
	}
}