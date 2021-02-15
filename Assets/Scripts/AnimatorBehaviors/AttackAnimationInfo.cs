using System;
using UnityEngine;

namespace RPG.AnimatorBehaviors
{
	public class AttackAnimationInfo : StateMachineBehaviour
	{
		public event Action OnAnimationComplete;

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateExit(animator, stateInfo, layerIndex);
			OnAnimationComplete?.Invoke();
		}
	}
}