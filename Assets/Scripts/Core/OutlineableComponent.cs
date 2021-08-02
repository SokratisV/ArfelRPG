using System.Collections;
using Core.Interfaces;
using UnityEngine;

namespace RPG.Core
{
	public class OutlineableComponent
	{
		private Coroutine _outlineRoutine = null;
		private IOutlineable _outline = null;
		private WaitForSeconds _waitForSeconds = new WaitForSeconds(GlobalValues.OutlineOffDelay);
		private bool _isMouseHoveringOver;

		public OutlineableComponent(GameObject obj, Color32 color)
		{
			_outline = new QuickOutlineable(obj) {OutlineColor = color, Enable = false};
		}

		public void ShowOutline(MonoBehaviour mono)
		{
			_isMouseHoveringOver = true;
			_outlineRoutine ??= _outlineRoutine.StartCoroutine(mono, TriggerOutline());
		}

		private IEnumerator TriggerOutline()
		{
			ToggleOutline(true);
			while (_isMouseHoveringOver)
			{
				_isMouseHoveringOver = false;
				yield return _waitForSeconds;
			}

			ToggleOutline(false);
		}

		private void ToggleOutline(bool toggle)
		{
			if (!toggle) _outlineRoutine = null;
			_outline.Enable = toggle;
		}
	}
}