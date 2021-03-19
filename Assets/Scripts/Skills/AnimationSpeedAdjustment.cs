using UnityEngine;

namespace RPG.Skills
{
	public class AnimationSpeedAdjustment : StateMachineBehaviour
	{
		private static SkillUser SkillUser;
		private static readonly int SkillAnimSpeed = Animator.StringToHash("skillAnimSpeed");
		private static readonly int ExtraSkillAnimation = Animator.StringToHash("skillExtra");

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if(SkillUser == null) SkillUser = animator.GetComponent<SkillUser>();

			if(SkillUser.HasCastTime)
			{
				var number = stateInfo.length / SkillUser.SelectedSkillDuration;
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