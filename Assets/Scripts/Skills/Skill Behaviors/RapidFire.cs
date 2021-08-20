using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Combat;
using RPG.Movement;
using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public class RapidFire : SkillBehavior
	{
		[SerializeField] private Projectile projectile = null;
		[SerializeField] private float damage;
		[SerializeField] private float castRange;
		[SerializeField] private int numberOfShots;
		// [SerializeField] private int numberOfBounces;

		public override bool HasCastTime() => true;
		public override bool UseExtraAnimation() => true;
		public override int SkillAnimationNumber() => 1;
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
			while(true)
			{
				ExecuteBehavior(data.User, data.Targets);
				yield return new WaitForSeconds(Duration / numberOfShots);
			}
		}

		private void ExecuteBehavior(GameObject user, List<GameObject> targets)
		{
			var projectileInstance = Instantiate(projectile, user.GetComponent<BodyParts>().ProjectileLocation.position, Quaternion.identity);
			projectileInstance.Setup(targets[0].GetComponent<Health>(), user, damage);
			// if(numberOfBounces > 0)
			// {
			// 	var colliders = Physics.OverlapSphere(targets[0].transform.position, castRange);
			// 	for(var i = 0;i < numberOfBounces;i++)
			// 	{
			// 		foreach(var collider in colliders)
			// 		{
			// 			if(collider.TryGetComponent(out Health health))
			// 			{
			// 				if(health.gameObject == user) continue;
			// 				projectileInstance = Instantiate(projectile, targets[0].transform.position, Quaternion.identity);
			// 				projectileInstance.SetTarget(health, user, damage);
			// 				break;
			// 			}
			// 		}
			// 	}
			// }
		}
	}
}