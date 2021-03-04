﻿using RPG.Attributes;
using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public class SelfHeal : SkillBehavior
	{
		[SerializeField] private float amountToHeal;

		public override void BehaviorStart(GameObject user, GameObject[] targets)
		{
			if(user.TryGetComponent(out Health health))
			{
				if(!health.IsDead && !(health.GetPercentage() >= 100.0f))
				{
					health.Heal(amountToHeal);
				}
			}

			OnStart?.Invoke(user, targets);
		}

		public override void BehaviorUpdate(GameObject user, GameObject[] targets)
		{
		}

		public override void BehaviorEnd(GameObject user, GameObject[] targets)
		{
			OnEnd?.Invoke(user, targets);
			OnEnd = null;
			OnStart = null;
			OnUpdate = null;
		}
	}
}