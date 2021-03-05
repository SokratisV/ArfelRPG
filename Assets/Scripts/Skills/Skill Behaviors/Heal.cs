using RPG.Attributes;
using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public class Heal : SkillBehavior
	{
		[SerializeField] private float amountToHeal;

		public override void BehaviorStart(GameObject user, GameObject[] targets, Vector3? point = null)
		{
			if(targets[0].TryGetComponent(out Health health))
			{
				if(!health.IsDead && !(health.GetPercentage() >= 100.0f))
				{
					health.Heal(amountToHeal);
				}
			}

			base.BehaviorStart(user, targets, point);
		}
	}
}