using System;
using UnityEditor;
using UnityEngine;

namespace RPG.Inventories
{
	/// <summary>
	/// An inventory item that can be equipped to the player. Weapons could be a
	/// subclass of this.
	/// </summary>
	[CreateAssetMenu(fileName = "Equipable Item", menuName = "RPG/Inventory/New Equipable Item")]
	public class EquipableItem : InventoryItem
	{
		[Tooltip("Where are we allowed to put this item.")] [SerializeField]
		protected EquipLocation allowedEquipLocation = EquipLocation.Weapon;

		public EquipLocation AllowedEquipLocation => allowedEquipLocation;

#if UNITY_EDITOR
		protected virtual bool IsLocationSelectable(Enum location)
		{
			var candidate = (EquipLocation)location;
			return candidate != EquipLocation.Weapon;
		}

		private void SetAllowedEquipLocation(EquipLocation newLocation)
		{
			if(allowedEquipLocation == newLocation) return;
			SetUndo("Change Equip Location");
			allowedEquipLocation = newLocation;
			Dirty();
		}

		private bool _drawInventoryItem = true;

		public override void DrawCustomInspector()
		{
			base.DrawCustomInspector();
			FoldoutStyle = new GUIStyle(EditorStyles.foldout) {fontStyle = FontStyle.Bold};
			_drawInventoryItem = EditorGUILayout.Foldout(_drawInventoryItem, "Equipable Item Data", FoldoutStyle);
			if(!_drawInventoryItem) return;
			SetAllowedEquipLocation((EquipLocation)EditorGUILayout.EnumPopup(new GUIContent("Equip Location"), allowedEquipLocation, IsLocationSelectable, false));
		}
#endif
	}
}