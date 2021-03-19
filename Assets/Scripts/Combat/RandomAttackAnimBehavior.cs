using UnityEngine;
using Random = UnityEngine.Random;

namespace RPG.Combat
{
	public class RandomAttackAnimBehavior : StateMachineBehaviour
	{
		[SerializeField] private int amountOfAnimations;

		private static readonly int AttackAnimID = Animator.StringToHash("attackAnimID");

		public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
		{
			var value = Random.Range(0, amountOfAnimations);
			animator.SetInteger(AttackAnimID, value);
		}
	}
}