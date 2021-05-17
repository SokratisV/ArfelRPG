using System;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Core;
using RPG.Inventories;
using RPG.Stats;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RPG.Combat
{
	[CreateAssetMenu(fileName = "Weapon", menuName = "RPG/Inventory/New Weapon", order = 0)]
	public class WeaponConfig : StatsEquipableItem
	{
		[SerializeField] private AnimatorOverrideController animatorOverride = null;
		[SerializeField] private Weapon equippedPrefab = null;
		[SerializeField] private float weaponRange = 2f, weaponDamage = 5f, percentageBonus = 0, attackSpeed;
		[SerializeField] private bool isRightHanded = true;
		[SerializeField] private TargetedProjectile projectile = null;
		[SerializeField] private string[] skillIds;

		public string[] SkillIds => skillIds;

		public float AttackSpeed => attackSpeed;

		public override string StatDescription
		{
			get
			{
				var result = base.StatDescription;
				result += projectile? "Ranged Weapon":"Melee Weapon";
				result += $"\nRange {weaponRange} meters";
				result += $"\nBase Damage {weaponDamage} points";
				if((int)percentageBonus != 0)
				{
					var bonus = percentageBonus > 0? "<color=#8888ff>bonus</color>":"<color=#ff8888>penalty</color>";
					result += $"\n{(int)percentageBonus} percent {bonus} to attack.";
				}

				return result;
			}
		}

		private const string WeaponName = "Weapon";
		private static Dictionary<Animator, Weapon> WeaponPerPlayer = new Dictionary<Animator, Weapon>();

		public bool HasProjectile() => projectile != null;

		public void LaunchProjectile(Vector3 position, Health target, GameObject instigator, float calculatedDamage)
		{
			var projectileInstance = Instantiate(projectile, position, Quaternion.identity);
			projectileInstance.SetTarget(target, instigator, calculatedDamage);
		}

		public Weapon Spawn(BodyParts bodyParts, Animator animator)
		{
			DestroyOldWeapon(animator);
			Weapon weapon = null;

			if(equippedPrefab != null)
			{
				var handTransform = GetHoldingHand(bodyParts.RightHand, bodyParts.LeftHand);
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

		private Transform GetHoldingHand(Transform rightHand, Transform leftHand)
		{
			var handTransform = isRightHanded? rightHand:leftHand;
			return handTransform;
		}

		public float GetRange() => weaponRange;

		private void OnDestroy() => WeaponPerPlayer = null;

		public override IEnumerable<float> GetAdditiveModifiers(Stat stat)
		{
			if(stat == Stat.Damage) yield return weaponDamage;
		}

		public override IEnumerable<float> GetPercentageModifiers(Stat stat)
		{
			if(stat == Stat.Damage) yield return percentageBonus;
		}

#if UNITY_EDITOR
		protected override bool IsLocationSelectable(Enum location)
		{
			var candidate = (EquipLocation)location;
			return candidate == EquipLocation.Weapon;
		}

		private void SetWeaponRange(float newWeaponRange)
		{
			if(Helper.FloatEquals(weaponRange, newWeaponRange)) return;
			SetUndo("Set Weapon Range");
			weaponRange = newWeaponRange;
			Dirty();
		}

		private void SetWeaponDamage(float newWeaponDamage)
		{
			if(Helper.FloatEquals(weaponDamage, newWeaponDamage)) return;
			SetUndo("Set Weapon Damage");
			weaponDamage = newWeaponDamage;
			Dirty();
		}

		private void SetPercentageBonus(float newPercentageBonus)
		{
			if(Helper.FloatEquals(percentageBonus, newPercentageBonus)) return;
			SetUndo("Set Percentage Bonus");
			percentageBonus = newPercentageBonus;
			Dirty();
		}

		private void SetIsRightHanded(bool newRightHanded)
		{
			if(isRightHanded == newRightHanded) return;
			SetUndo(newRightHanded? "Set as Right Handed":"Set as Left Handed");
			isRightHanded = newRightHanded;
			Dirty();
		}

		private void SetAnimatorOverride(AnimatorOverrideController newOverride)
		{
			if(newOverride == animatorOverride) return;
			SetUndo("Change AnimatorOverride");
			animatorOverride = newOverride;
			Dirty();
		}

		private void SetEquippedPrefab(Weapon newWeapon)
		{
			if(newWeapon == equippedPrefab) return;
			SetUndo("Set Equipped Prefab");
			equippedPrefab = newWeapon;
			Dirty();
		}

		private void SetProjectile(TargetedProjectile possibleProjectile)
		{
			if(possibleProjectile == null) return;
			if(!possibleProjectile.TryGetComponent(out TargetedProjectile newProjectile)) return;
			if(newProjectile == projectile) return;
			SetUndo("Set Projectile");
			projectile = newProjectile;
			Dirty();
		}

		private bool _drawWeaponConfig = true;

		public override void DrawCustomInspector()
		{
			base.DrawCustomInspector();
			FoldoutStyle = new GUIStyle(EditorStyles.foldout) {fontStyle = FontStyle.Bold};
			_drawWeaponConfig = EditorGUILayout.Foldout(_drawWeaponConfig, "Weapon Config Data", FoldoutStyle);
			if(!_drawWeaponConfig) return;
			SetEquippedPrefab((Weapon)EditorGUILayout.ObjectField("Equipped Prefab", equippedPrefab, typeof(Object), false));
			SetWeaponDamage(EditorGUILayout.Slider("Weapon Damage", weaponDamage, 0, 100));
			SetWeaponRange(EditorGUILayout.Slider("Weapon Range", weaponRange, 1, 40));
			SetPercentageBonus(EditorGUILayout.IntSlider("Percentage Bonus", (int)percentageBonus, -10, 100));
			SetIsRightHanded(EditorGUILayout.Toggle("Is Right Handed", isRightHanded));
			SetAnimatorOverride((AnimatorOverrideController)EditorGUILayout.ObjectField("Animator Override", animatorOverride, typeof(AnimatorOverrideController), false));
			SetProjectile((TargetedProjectile)EditorGUILayout.ObjectField("Projectile", projectile, typeof(TargetedProjectile), false));
		}
#endif
	}
}