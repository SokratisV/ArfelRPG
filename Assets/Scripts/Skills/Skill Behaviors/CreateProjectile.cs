using RPG.Attributes;
using RPG.Combat;
using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public class CreateProjectile : SkillBehavior
	{
		[SerializeField] private Projectile projectile = null;
		[SerializeField] private float damage;

		public override void BehaviorStart(GameObject user, GameObject[] targets)
		{
			var projectileInstance = Instantiate(projectile);
			var transform = projectileInstance.transform;
			transform.position = user.transform.position;
			transform.rotation = user.transform.rotation;
			if(targets[0] != null)
			{
				projectileInstance.SetTarget(targets[0].GetComponent<Health>(), user, damage);
			}
			else
			{
				
			}
		}

		public override void BehaviorUpdate(GameObject user, GameObject[] targets)
		{
			throw new System.NotImplementedException();
		}

		public override void BehaviorEnd(GameObject user, GameObject[] targets)
		{
			throw new System.NotImplementedException();
		}
	}
}