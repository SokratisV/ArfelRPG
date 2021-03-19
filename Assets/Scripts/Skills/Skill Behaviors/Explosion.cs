using System.Collections.Generic;
using RPG.Attributes;
using RPG.Movement;
using UnityEngine;

namespace RPG.Skills.Behaviors
{
	[CreateAssetMenu(menuName = "RPG/Skills/Explosion")]
	public class Explosion : SkillBehavior
	{
		[Min(0)] [SerializeField] private float damage;
		[SerializeField] private float castRange;

		public override float GetCastingRange() => castRange;
		public override bool HasCastTime() => true;
		public override bool UseExtraAnimation() => true;
		public override int SkillAnimationNumber() => 2;

		public override void BehaviorStart(GameObject user, List<GameObject> targets, Vector3? point = null)
		{
			if(point != null)
			{
				user.GetComponent<Mover>().RotateOverTime(.2f, point.Value);
			}

			base.BehaviorStart(user, targets, point);
		}

		public override void BehaviorUpdate(GameObject user, List<GameObject> targets, Vector3? point = null)
		{
		}

		public override void BehaviorEnd(GameObject user, List<GameObject> targets, Vector3? point = null)
		{
			if(targets != null)
			{
				for(var i = targets.Count - 1;i >= 0;i--)
				{
					var target = targets[i];
					var health = target.GetComponent<Health>();
					RemoveHealthFromList(health, targets);
					health.TakeDamage(user, damage);
				}
			}

			base.BehaviorEnd(user, targets, point);
		}
	}
}