using System.Collections.Generic;
using RPG.Attributes;
using RPG.Movement;
using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public class Freeze : SkillBehavior
	{
		[Min(0)] [SerializeField] private float damage;
		[Range(0, 100)] [SerializeField] private float slowPercent;

		public override bool HasCastTime() => false;
		public override bool UseExtraAnimation() => false;
		public override int SkillAnimationNumber() => 1;

		public override void BehaviorStart(GameObject user, List<GameObject> targets, Vector3? point = null)
		{
			if(targets != null)
			{
				for(var i = targets.Count - 1;i >= 0;i--)
				{
					var target = targets[i];
					var mover = target.GetComponent<Mover>();
					mover.CurrentSpeed = mover.CurrentSpeed * slowPercent * 0.01f;
					var health = target.GetComponent<Health>();
					RemoveHealthFromList(health, targets);
					health.TakeDamage(user, damage);
				}
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
				foreach(var target in targets)
				{
					target.GetComponent<Mover>().RevertToOriginalSpeed();
				}
			}

			base.BehaviorEnd(user, targets, point);
		}
	}
}