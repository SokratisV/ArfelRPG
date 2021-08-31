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

		public override bool HasCastTime() => true;
		public override bool UseExtraAnimation() => true;
		public override int SkillAnimationNumber() => 1;
		public override float GetCastingRange() => castRange;

		public override void BehaviorStart(SkillData data)
		{
			data.Targets[0].GetComponent<Mover>().RotateToTarget(.2f, data.Targets[1].transform.position);
			base.BehaviorStart(data);
		}

		public override IEnumerator BehaviorUpdate(SkillData data)
		{
			while (true)
			{
				ExecuteBehavior(data.Targets[0], data.Targets[1]);
				yield return new WaitForSeconds(Duration / numberOfShots);
			}
		}

		private void ExecuteBehavior(GameObject user, GameObject target)
		{
			var projectileInstance = Instantiate(projectile, user.GetComponent<BodyParts>().ProjectileLocation.position, Quaternion.identity);
			projectileInstance.Setup(target.GetComponent<Health>(), user, damage);
		}
	}
}