using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController overrideController = null;
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] bool isRightHanded = true;

        public float WeaponDamage { get => weaponDamage; set => weaponDamage = value; }
        public float WeaponRange { get => weaponRange; set => weaponRange = value; }

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (equippedPrefab != null)
            {
                Transform handTransform;
                if (isRightHanded) handTransform = rightHand;
                else handTransform = leftHand;
                Instantiate(equippedPrefab, handTransform);
            }
            if (overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController;
            }
        }
    }
}
