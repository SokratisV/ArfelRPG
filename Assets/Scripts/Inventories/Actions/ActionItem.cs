using UnityEditor;
using UnityEngine;

namespace RPG.Inventories
{
	/// <summary>
	/// An inventory item that can be placed in the action bar and "Used".
	/// </summary>
	/// <remarks>
	/// This class should be used as a base. Subclasses must implement the `Use`
	/// method.
	/// </remarks>
	public class ActionItem : InventoryItem
	{
		[Tooltip("Does an instance of this item get consumed every time it's used.")] [SerializeField]
		private bool consumable;

		/// <summary>
		/// Trigger the use of this item. Override to provide functionality.
		/// </summary>
		/// <param name="user">The character that is using this action.</param>
		public virtual void Use(GameObject user) => Debug.Log("Using action: " + this);

		public virtual bool CanUse(GameObject user) => true;

		public bool IsConsumable => consumable;

#if UNITY_EDITOR
		private void SetIsConsumable(bool value)
		{
			if(consumable == value) return;
			SetUndo(value? "Set Consumable":"Set Not Consumable");
			consumable = value;
			Dirty();
		}

		private bool _drawActionItem = true;

		public override void DrawCustomInspector()
		{
			base.DrawCustomInspector();
			_drawActionItem = EditorGUILayout.Foldout(_drawActionItem, "Action Item Data");
			if(!_drawActionItem) return;
			EditorGUILayout.BeginVertical(ContentStyle);
			SetIsConsumable(EditorGUILayout.Toggle("Is Consumable", consumable));
			EditorGUILayout.EndVertical();
		}
#endif
	}
}