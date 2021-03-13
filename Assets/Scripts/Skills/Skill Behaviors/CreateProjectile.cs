using RPG.Attributes;
using RPG.Combat;
using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public class CreateProjectile : SkillBehavior
	{
		[SerializeField] private Projectile projectile = null;
		[SerializeField] private float damage;
		[SerializeField] private float castRange;

		public override bool UseExtraAnimation() => false;
		public override int SkillAnimationNumber() => 0;
		public override float GetCastingRange() => castRange;

		public override void BehaviorStart(GameObject user, GameObject[] targets, Vector3? point = null)
		{
			if(targets[0] != null)
			{
				var projectileInstance = Instantiate(projectile);
				var transform = projectileInstance.transform;
				transform.position = user.transform.position;
				transform.rotation = user.transform.rotation;
				projectileInstance.SetTarget(targets[0].GetComponent<Health>(), user, damage);
			}


			base.BehaviorStart(user, targets, point);
		}
	}
}