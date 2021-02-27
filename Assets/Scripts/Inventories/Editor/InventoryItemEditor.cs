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
			if(_selected.Rarity != null) _headerStyle.normal.textColor = _selected.Rarity.Color;
			Repaint();
		}

		private GUIStyle _previewStyle;
		private GUIStyle _descriptionStyle;
		private GUIStyle _headerStyle;
		private GUIStyle _statsStyle;

		private void OnEnable()
		{
			_previewStyle = new GUIStyle
			{
				normal = {background = EditorGUIUtility.Load("Assets/Asset Packs/Fantasy RPG UI/UI/Parts/Background_06.png") as Texture2D},
				padding = new RectOffset(0, 0, 150, 250),
				border = new RectOffset(0, 0, 0, 0)
			};
		}

		private bool _stylesInitialized = false;

		private void OnGUI()
		{
			if(!_selected)
			{
				EditorGUILayout.HelpBox("No InventoryItem Selected", MessageType.Error);
				return;
			}

			if(!_stylesInitialized)
			{
				_descriptionStyle = new GUIStyle(GUI.skin.label)
				{
					richText = true,
					wordWrap = true,
					stretchHeight = true,
					fontSize = 20,
					alignment = TextAnchor.MiddleCenter,
					padding = new RectOffset(25, 25, 0, 0)
				};
				_headerStyle = new GUIStyle(_descriptionStyle) {fontSize = 25};
				_statsStyle = new GUIStyle(_descriptionStyle) {fontSize = 15};
				_stylesInitialized = true;
			}

			var rect = new Rect(0, 0, position.width * .65f, position.height);
			DrawInspector(rect);
			rect.x = rect.width;
			rect.width /= 2.0f;
			DrawPreviewTooltip(rect);
		}

		private Vector2 _scrollPosition;

		private void DrawInspector(Rect rect)
		{
			GUILayout.BeginArea(rect);
			_scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
			_selected.DrawCustomInspector();
			GUILayout.EndScrollView();
			GUILayout.EndArea();
		}

		private void DrawPreviewTooltip(Rect rect)
		{
			GUILayout.BeginArea(rect, _previewStyle);
			if(_selected.Icon != null)
			{
				var iconSize = Mathf.Min(rect.width * .33f, rect.height * .33f);
				var texRect = GUILayoutUtility.GetRect(iconSize, iconSize);
				GUI.DrawTexture(texRect, _selected.Icon.texture, ScaleMode.ScaleToFit);
			}

			EditorGUILayout.LabelField(_selected.DisplayName, _headerStyle);
			EditorGUILayout.LabelField(_selected.RawDescription, _descriptionStyle);
			EditorGUILayout.LabelField(_selected.StatDescription, _statsStyle);
			GUILayout.EndArea();
		}
	}
}