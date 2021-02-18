using System.Collections.Generic;
using RPG.Attributes;
using RPG.Inventories;
using UnityEngine;

namespace RPG.Combat
{
	[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/New Weapon", order = 0)]
	public class WeaponConfig : EquipableItem
	{
		[SerializeField] private AnimatorOverrideController animatorOverride = null;
		[SerializeField] private Weapon equippedPrefab = null;
		[SerializeField] private float weaponRange = 2f, weaponDamage = 5f, weaponPercentageBonus = 0;
		[SerializeField] private bool isRightHanded = true;
		[SerializeField] private Projectile projectile = null;

		private const string WeaponName = "Weapon";
		private static Dictionary<Animator, Weapon> WeaponPerPlayer = new Dictionary<Animator, Weapon>();

		public bool HasProjectile() => projectile != null;

		public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage)
		{
			var projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
			projectileInstance.SetTarget(target, instigator, calculatedDamage);
		}

		public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator)
		{
			DestroyOldWeapon(animator);
			Weapon weapon = null;

			if(equippedPrefab != null)
			{
				var handTransform = GetTransform(rightHand, leftHand);
				weapon = Instantiate(equippedPrefab, handTransform);
				weapon.gameObject.name = WeaponName;
				WeaponPerPlayer[animator] = weapon;
			}

			var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
			if(animatorOverride != null)
			{
				animator.runtimeAnimatorController = animatorOverride;
			}
			else if(overrideController != null)
			{
				animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
			}

			return weapon;
		}

		private void DestroyOldWeapon(Animator animator)
		{
			if(WeaponPerPlayer.TryGetValue(animator, out var weapon))
			{
				Destroy(weapon.gameObject);
			}
		}

		private Transform GetTransform(Transform rightHand, Transform leftHand)
		{
			var handTransform = isRightHanded? rightHand:leftHand;
			return handTransform;
		}

		public float GetDamage() => weaponDamage;

		public float GetRange() => weaponRange;

		public float GetPercentageBonus() => weaponPercentageBonus;

		private void OnDestroy() => WeaponPerPlayer = null;
	}
}