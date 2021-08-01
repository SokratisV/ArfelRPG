using RPG.Attributes;
using RPG.Core;
using UnityEditor;
using UnityEngine;

namespace RPG.Inventories
{
	[CreateAssetMenu(fileName = "New Healing Spell", menuName = "RPG/Actions/New HealingSpell")]
	public class HealingSpell : ActionItem
	{
		[SerializeField] private float amountToHeal;
		[SerializeField] private bool isPercentage;

		public override string StatDescription
		{
			get
			{
				var result = RawDescription + "\n";
				var spell = IsConsumable? "potion":"spell";
				var percent = isPercentage? "percent of your Max Health":"Health Points.";
				result += $"This {spell} will restore {(int)amountToHeal} {percent}";
				return result;
			}
		}

		public override bool CanUse(GameObject user)
		{
			if(!user.TryGetComponent(out Health health)) return false;
			return!health.IsDead && !(health.GetPercentage() >= 100.0f);
		}

		public override bool Use(GameObject user)
		{
			base.Use(user);
			if(!user.TryGetComponent(out Health health)) return false;
			if(health.IsDead) return false;
			health.Heal(user, isPercentage? health.GetMaxHealthPoints() * amountToHeal / 100.0f:amountToHeal);
			return true;
		}

#if UNITY_EDITOR
		private void SetAmountToHeal(float value)
		{
			if(Helper.FloatEquals(amountToHeal, value)) return;
			SetUndo("Change Amount To Heal");
			amountToHeal = value;
			Dirty();
		}

		private void SetIsPercentage(bool value)
		{
			if(isPercentage == value) return;
			SetUndo(value? "Set as Percentage Heal":"Set as Absolute Heal");
			isPercentage = value;
		}

		private bool _drawHealingData = true;

		public override void DrawCustomInspector()
		{
			base.DrawCustomInspector();
			_drawHealingData = EditorGUILayout.Foldout(_drawHealingData, "HealingSpell Data");
			if(!_drawHealingData) return;
			EditorGUILayout.BeginVertical(ContentStyle);
			SetAmountToHeal(EditorGUILayout.IntSlider("Amount to Heal", (int)amountToHeal, 1, 100));
			SetIsPercentage(EditorGUILayout.Toggle("Is Percentage", isPercentage));
			EditorGUILayout.EndVertical();
		}
#endif
	}
}