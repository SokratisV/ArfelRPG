using UnityEngine;

namespace RPG.AnimatorBehaviors
{
    public class RandomAttackAnimBehavior : StateMachineBehaviour
    {
        [SerializeField] private int amountOfAnimations;
        [SerializeField] private float timeBetweenAttacks = 1f; //Same as Fighter, TODO: find a way to connect.
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
            if(length > timeBetweenAttacks)
            {
                animator.SetFloat(AttackAnimSpeed, length / timeBetweenAttacks);
            }
            else
            {
                animator.SetFloat(AttackAnimSpeed, 1f);
            }
        }
    }
}