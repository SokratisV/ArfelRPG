using System;
using System.Collections.Generic;
using System.Linq;
using RPG.Core;
using RPG.Stats;
using UnityEditor;
using UnityEngine;

namespace RPG.Inventories
{
	[CreateAssetMenu(fileName = "Equipable Item with Stats", menuName = "RPG/Inventory/Equipable Item With Stats")]
	public class StatsEquipableItem : EquipableItem, IModifierProvider
	{
		[SerializeField] private List<Modifier> additiveModifiers = new List<Modifier>();
		[SerializeField] private List<Modifier> percentageModifiers = new List<Modifier>();

		[Serializable]
		private struct Modifier
		{
			public Stat stat;
			public float value;
		}

		public IEnumerable<float> GetAdditiveModifiers(Stat stat) =>
			from modifier in additiveModifiers where modifier.stat == stat select modifier.value;

		public IEnumerable<float> GetPercentageModifiers(Stat stat) =>
			from modifier in percentageModifiers where modifier.stat == stat select modifier.value;

#if UNITY_EDITOR
		private void AddModifier(List<Modifier> modifierList)
		{
			SetUndo("Add Modifier");
			modifierList?.Add(new Modifier());
			Dirty();
		}

		private void RemoveModifier(List<Modifier> modifierList, int index)
		{
			SetUndo("Remove Modifier");
			modifierList?.RemoveAt(index);
			Dirty();
		}

		private void SetStat(List<Modifier> modifierList, int i, Stat stat)
		{
			if(modifierList[i].stat == stat) return;
			SetUndo("Change Modifier Stat");
			var mod = modifierList[i];
			mod.stat = stat;
			modifierList[i] = mod;
			Dirty();
		}

		private void SetValue(List<Modifier> modifierList, int i, float value)
		{
			if(Helper.FloatEquals(modifierList[i].value, value)) return;
			SetUndo("Change Modifier Value");
			var mod = modifierList[i];
			mod.value = value;
			modifierList[i] = mod;
			Dirty();
		}

		private void DrawModifierList(List<Modifier> modifierList)
		{
			var modifierToDelete = -1;
			var statLabel = new GUIContent("Stat");
			for(var i = 0;i < modifierList.Count;i++)
			{
				var modifier = modifierList[i];
				EditorGUILayout.BeginHorizontal();
				SetStat(modifierList, i, (Stat)EditorGUILayout.EnumPopup(statLabel, modifier.stat, IsStatSelectable, false));
				SetValue(modifierList, i, EditorGUILayout.IntSlider("Value", (int)modifier.value, -20, 100));
				if(GUILayout.Button("-"))
				{
					modifierToDelete = i;
				}

				EditorGUILayout.EndHorizontal();
			}

			if(modifierToDelete > -1)
			{
				RemoveModifier(modifierList, modifierToDelete);
			}

			if(GUILayout.Button("Add Modifier"))
			{
				AddModifier(modifierList);
			}
		}

		private bool _drawStatsEquipableItemData = true;
		private bool _drawAdditive = true;
		private bool _drawPercentage = true;

		public override void DrawCustomInspector()
		{
			base.DrawCustomInspector();
			_drawStatsEquipableItemData = EditorGUILayout.Foldout(_drawStatsEquipableItemData, "StatsEquipableItemData", FoldoutStyle);
			if(!_drawStatsEquipableItemData) return;
			EditorGUILayout.BeginVertical(ContentStyle);
			_drawAdditive = EditorGUILayout.Foldout(_drawAdditive, "Additive Modifiers");
			if(_drawAdditive)
			{
				DrawModifierList(additiveModifiers);
			}

			_drawPercentage = EditorGUILayout.Foldout(_drawPercentage, "Percentage Modifiers");
			if(_drawPercentage)
			{
				DrawModifierList(percentageModifiers);
			}
			EditorGUILayout.EndVertical();
		}

		private static bool IsStatSelectable(Enum candidate)
		{
			var stat = (Stat)candidate;
			return stat != Stat.ExperienceReward && stat != Stat.ExperienceToLevelUp;
		}
#endif
	}
}