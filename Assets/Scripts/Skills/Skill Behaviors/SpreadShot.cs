using System.Collections;
using RPG.Combat;
using RPG.Movement;
using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public class SpreadShot : SkillBehavior
	{
		[SerializeField] private Projectile projectile;
		[SerializeField, Min(1)] private float distanceBeforeDestroy;
		[SerializeField, Min(0)] private float projectileSpeed;
		[SerializeField, Min(0)] private float damage;
		[SerializeField, Range(1, 90)] private float angleBetweenEachProjectile;
		[SerializeField, Range(0, 90)] private float dynamicAngle;
		[SerializeField, Min(1)] private int numberOfProjectiles;

		public override bool HasCastTime() => true;
		public override int SkillAnimationNumber() => 2;

		public override float[] SpecialFloats()
		{
			var floats = new float[2];
			if (numberOfProjectiles % 2 == 0)
			{
				floats[0] = numberOfProjectiles * angleBetweenEachProjectile;
			}
			else
			{
				floats[0] = (numberOfProjectiles - 1) * angleBetweenEachProjectile;
			}

			floats[1] = distanceBeforeDestroy;
			return floats;
		}

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
			var lifeTime = distanceBeforeDestroy / projectileSpeed;
			projectileInstance.Setup(user, damage, projectileSpeed, lifeTime);
			for (var i = 1; i <= (numberOfProjectiles - 1) / 2; i++)
			{
				projectileInstance = Instantiate(projectile, bodyParts.ProjectileLocation.position, Quaternion.LookRotation(Quaternion.Euler(0, angle * i, 0) * direction));
				projectileInstance.Setup(user, damage, projectileSpeed, lifeTime);
			}

			for (var i = -1; i >= -(numberOfProjectiles - 1) / 2; i--)
			{
				projectileInstance = Instantiate(projectile, bodyParts.ProjectileLocation.position, Quaternion.LookRotation(Quaternion.Euler(0, angle * i, 0) * direction));
				projectileInstance.Setup(user, damage, projectileSpeed, lifeTime);
			}
		}

		private void EvenNumberOfProjectiles(GameObject user, BodyParts bodyParts, Vector3 direction, float angle)
		{
			var lifeTime = distanceBeforeDestroy / projectileSpeed;
			for (var i = -numberOfProjectiles / 2; i <= numberOfProjectiles / 2; i++)
			{
				if (i == 0) continue;
				var projectileInstance = Instantiate(projectile, bodyParts.ProjectileLocation.position, Quaternion.LookRotation(Quaternion.Euler(0, angle * i, 0) * direction));
				projectileInstance.Setup(user, damage, projectileSpeed, lifeTime);
			}
		}

		public override IEnumerator BehaviorUpdate(SkillData data)
		{
			yield break;
		}
	}
}