using UnityEngine;

namespace RPG.Skills.Behaviors
{
	[CreateAssetMenu(fileName = "Self Target", menuName = "RPG/Skills/New Self Target Behavior")]
	public class SelfTarget : TargetBehavior
	{
		public override CustomTarget GetTarget(GameObject user)
		{
			return new CustomTarget(user, null, null, null);
		}
	}
}