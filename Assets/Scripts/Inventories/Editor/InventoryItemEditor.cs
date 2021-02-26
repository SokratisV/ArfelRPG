using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RPG.Inventories.Editor
{
	public class InventoryItemEditor : EditorWindow
	{
		private InventoryItem _selected;

		[MenuItem("Window/InventoryItem Editor")]
		public static void ShowEditorWindow() => GetWindow(typeof(InventoryItemEditor), false, "InventoryItem");

		public static void ShowEditorWindow(InventoryItem candidate)
		{
			var window = GetWindow(typeof(InventoryItemEditor), false, "InventoryItem") as InventoryItemEditor;
			if(candidate)
			{
				window.OnSelectionChange();
			}
		}

		[OnOpenAsset(1)]
		public static bool OnOpenAsset(int instanceID, int line)
		{
			var candidate = EditorUtility.InstanceIDToObject(instanceID) as InventoryItem;
			if(candidate != null)
			{
				ShowEditorWindow(candidate);
				return true;
			}

			return false;
		}

		private void OnSelectionChange()
		{
			var candidate = EditorUtility.InstanceIDToObject(Selection.activeInstanceID) as InventoryItem;
			if(candidate == null) return;
			_selected = candidate;
			Repaint();
		}

		private void OnGUI()
		{
			if(!_selected)
			{
				EditorGUILayout.HelpBox("No InventoryItem Selected", MessageType.Error);
				return;
			}
			
			_selected.DrawCustomInspector();
		}
	}
}