﻿using System.Collections;
using RPG.Combat;
using RPG.Movement;
using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public class SpreadShot : SkillBehavior
	{
		[SerializeField] private Projectile projectile;
		[Min(0)] [SerializeField] private float projectileSpeed;
		[Min(0)] [SerializeField] private float damage;
		[Range(1, 90)] [SerializeField] private float angleBetweenEachProjectile;
		[Range(0, 90)] [SerializeField] private float dynamicAngle;
		[Min(1)] [SerializeField] private int numberOfProjectiles;

		public override bool HasCastTime() => true;
		public override int SkillAnimationNumber() => 2;

		public override void BehaviorStart(SkillData data)
		{
			if (data.Point != null)
			{
				data.User.GetComponent<Mover>().RotateOverTime(.1f, data.Point.Value);
			}

			base.BehaviorStart(data);
		}

		public override void BehaviorEnd(SkillData data)
		{
			ExecuteBehavior(data.User, data.Point.Value);
			base.BehaviorEnd(data);
		}

		private void ExecuteBehavior(GameObject user, Vector3 point)
		{
			var direction = (point - user.transform.position).normalized;
			var bodyParts = user.GetComponent<BodyParts>();
			var angle = dynamicAngle > 0 ? dynamicAngle / numberOfProjectiles : angleBetweenEachProjectile;

			if (numberOfProjectiles % 2 == 0)
			{
				EvenNumberOfProjectiles(user, bodyParts, direction, angle);
			}
			else
			{
				OddNumberOfProjectiles(user, bodyParts, direction, angle);
			}
		}

		private void OddNumberOfProjectiles(GameObject user, BodyParts bodyParts, Vector3 direction, float angle)
		{
			var projectileInstance = Instantiate(projectile, bodyParts.ProjectileLocation.position, Quaternion.LookRotation(direction));
			projectileInstance.Setup(user, damage, projectileSpeed);
			for (var i = 1; i <= (numberOfProjectiles - 1) / 2; i++)
			{
				projectileInstance = Instantiate(projectile, bodyParts.ProjectileLocation.position, Quaternion.LookRotation(Quaternion.Euler(0, angle * i, 0) * direction));
				projectileInstance.Setup(user, damage, projectileSpeed);
			}

			for (var i = -1; i >= -(numberOfProjectiles - 1) / 2; i--)
			{
				projectileInstance = Instantiate(projectile, bodyParts.ProjectileLocation.position, Quaternion.LookRotation(Quaternion.Euler(0, angle * i, 0) * direction));
				projectileInstance.Setup(user, damage, projectileSpeed);
			}
		}

		private void EvenNumberOfProjectiles(GameObject user, BodyParts bodyParts, Vector3 direction, float angle)
		{
			for (var i = -numberOfProjectiles / 2; i <= numberOfProjectiles / 2; i++)
			{
				if (i == 0) continue;
				var projectileInstance = Instantiate(projectile, bodyParts.ProjectileLocation.position, Quaternion.LookRotation(Quaternion.Euler(0, angle * i, 0) * direction));
				projectileInstance.Setup(user, damage, projectileSpeed);
			}
		}

		public override IEnumerator BehaviorUpdate(SkillData data)
		{
			yield break;
		}
	}
}