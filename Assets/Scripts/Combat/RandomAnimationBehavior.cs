using UnityEngine;
using Random = UnityEngine.Random;

namespace RPG.Combat
{
	public class RandomAnimationBehavior : StateMachineBehaviour
	{
		[SerializeField] private int amountOfAnimations;
		[SerializeField] private string animationId;
		[SerializeField] private bool onEnter;
		
		private int _animationId = -1;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (!onEnter) return;
			if (_animationId == -1) _animationId = Animator.StringToHash(animationId);
			var value = Random.Range(0, amountOfAnimations);
			animator.SetInteger(_animationId, value);
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (onEnter) return;
			if (_animationId == -1) _animationId = Animator.StringToHash(animationId);
			var value = Random.Range(0, amountOfAnimations);
			animator.SetInteger(_animationId, value);
		}
	}
}