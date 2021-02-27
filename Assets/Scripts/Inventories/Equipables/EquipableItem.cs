using System;
using UnityEditor;
using UnityEngine;

namespace RPG.Inventories
{
	/// <summary>
	/// An inventory item that can be equipped to the player. Weapons could be a
	/// subclass of this.
	/// </summary>
	[CreateAssetMenu(menuName = "RPG/Equipable Item")]
	public class EquipableItem : InventoryItem
	{
		[Tooltip("Where are we allowed to put this item.")] [SerializeField]
		protected EquipLocation allowedEquipLocation = EquipLocation.Weapon;

		public EquipLocation AllowedEquipLocation => allowedEquipLocation;

#if UNITY_EDITOR
		public virtual bool IsLocationSelectable(Enum location)
		{
			var candidate = (EquipLocation)location;
			return candidate != EquipLocation.Weapon;
		}

		public void SetAllowedEquipLocation(EquipLocation newLocation)
		{
			if(allowedEquipLocation == newLocation) return;
			SetUndo("Change Equip Location");
			allowedEquipLocation = newLocation;
			Dirty();
		}

		protected bool DrawInventoryItem = true;

		public override void DrawCustomInspector()
		{
			base.DrawCustomInspector();
			FoldoutStyle = new GUIStyle(EditorStyles.foldout) {fontStyle = FontStyle.Bold};
			DrawInventoryItem = EditorGUILayout.Foldout(DrawInventoryItem, "Equipable Item Data", FoldoutStyle);
			if(!DrawInventoryItem) return;
			SetAllowedEquipLocation((EquipLocation)EditorGUILayout.EnumPopup(new GUIContent("Equip Location"), allowedEquipLocation, IsLocationSelectable, false));
		}
#endif
	}
}