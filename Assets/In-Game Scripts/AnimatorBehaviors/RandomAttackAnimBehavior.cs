using UnityEngine;
using Random = UnityEngine.Random;

namespace RPG.AnimatorBehaviors
{
	public class RandomAttackAnimBehavior : StateMachineBehaviour
	{
		[SerializeField] private int amountOfAnimations;
		public float TimeBetweenAttacks {get;set;}
		private static readonly int AttackAnimID = Animator.StringToHash("attackAnimID");
		private static readonly int AttackAnimSpeed = Animator.StringToHash("attackAnimSpeed");

		public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
		{
			var value = Random.Range(0, amountOfAnimations);
			animator.SetInteger(AttackAnimID, value);
			ChangeAnimationLength(animator);
		}

		//Kind of works, not sure why
		private void ChangeAnimationLength(Animator animator)
		{
			var length = animator.GetCurrentAnimatorStateInfo(0).length;
			if(length > TimeBetweenAttacks)
			{
				animator.SetFloat(AttackAnimSpeed, length / TimeBetweenAttacks);
			}
			else
			{
				animator.SetFloat(AttackAnimSpeed, 1f);
			}
		}
	}
}