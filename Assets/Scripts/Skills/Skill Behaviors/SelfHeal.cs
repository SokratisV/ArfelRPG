using RPG.Attributes;
using UnityEngine;

namespace RPG.Skills.Behaviors
{
	[CreateAssetMenu(fileName = "Self Heal", menuName = "RPG/Skills/New Self Heal Behavior")]
	public class SelfHeal : SkillBehavior
	{
		[SerializeField] private float amountToHeal;

		private GameObject _user;
		private CustomTarget _target;

		public override void BehaviorStart(GameObject user, CustomTarget target)
		{
			_user = user;
			_target = target;
			if(!user.TryGetComponent(out Health health))
			{
				if(!health.IsDead && !(health.GetPercentage() >= 100.0f))
				{
					health.Heal(amountToHeal);
				}
			}

			OnStart?.Invoke(user, target);
		}

		public override void BehaviorUpdate()
		{
		}

		public override void BehaviorEnd()
		{
			OnEnd?.Invoke(_user, _target);
			OnEnd = null;
			OnStart = null;
			OnUpdate = null;
		}
	}
}