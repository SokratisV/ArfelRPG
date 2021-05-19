using UnityEngine;

namespace RPG.Skills
{
	public class AnimationSpeedAdjustment : StateMachineBehaviour
	{
		private static SkillUser _skillUser;
		private static readonly int SkillAnimSpeed = Animator.StringToHash("skillAnimSpeed");
		private static readonly int ExtraSkillAnimation = Animator.StringToHash("skillExtra");

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if(_skillUser == null) _skillUser = animator.GetComponent<SkillUser>();

			if(_skillUser.ShouldChangeAnimationSpeed)
			{
				var number = stateInfo.length / _skillUser.SelectedSkillDuration;
				animator.SetFloat(SkillAnimSpeed, number);
			}
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			animator.SetFloat(SkillAnimSpeed, 1f);
			animator.SetBool(ExtraSkillAnimation, false);
		}
	}
}