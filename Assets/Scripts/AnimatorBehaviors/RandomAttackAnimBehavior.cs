namespace RPG.AnimatorBehaviors
{
    using UnityEngine;

    public class RandomAttackAnimBehavior : StateMachineBehaviour
    {
        [SerializeField] int amountOfAnimations;
        [SerializeField] float timeBetweenAttacks = 1f; //Same as Fighter, TODO: find a way to connect.

        override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
        {
            int value = Random.Range(0, amountOfAnimations);
            animator.SetInteger("attackAnimID", value);
            ChangeAnimationLength(animator);
        }
        //Kind of works, not sure why
        private void ChangeAnimationLength(Animator animator)
        {
            float length = animator.GetCurrentAnimatorStateInfo(0).length;
            if (length > timeBetweenAttacks)
            {
                animator.SetFloat("attackAnimSpeed", length / timeBetweenAttacks);
            }
            else { animator.SetFloat("attackAnimSpeed", 1f); }
        }
    }

}
