using System.Collections;
using UnityEditor.Graphs;
using UnityEngine;

namespace RPG.Core
{
	public class OutlineableComponent
	{
		private Coroutine _outlineRoutine = null;
		private Outline _outline = null;
		private WaitForSeconds _waitForSeconds = new WaitForSeconds(GlobalValues.OutlineOffDelay);
		private bool _isMouseHoveringOver;

		public OutlineableComponent(GameObject obj, Color32 color)
		{
			var outline = obj.GetComponentInChildren<Outline>();
			if(!outline) outline = obj.AddComponent<Outline>();

			_outline = outline;
			_outline.OutlineColor = color;
			_outline.enabled = false;
		}

		public void ShowOutline(MonoBehaviour mono)
		{
			_isMouseHoveringOver = true;
			_outlineRoutine ??= _outlineRoutine.StartCoroutine(mono, TriggerOutline());
		}

		private IEnumerator TriggerOutline()
		{
			ToggleOutline(true);
			while(_isMouseHoveringOver)
			{
				_isMouseHoveringOver = false;
				yield return _waitForSeconds;
			}

			ToggleOutline(false);
		}

		private void ToggleOutline(bool toggle)
		{
			if(!toggle) _outlineRoutine = null;
			_outline.enabled = toggle;
		}
	}
}