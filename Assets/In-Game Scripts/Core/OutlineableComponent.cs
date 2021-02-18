using System.Collections;
using UnityEngine;

namespace RPG.Core
{
	public class OutlineableComponent
	{
		private Coroutine _outlineRoutine = null;
		private Outline _outline = null;
		private WaitForSeconds _waitForSeconds = new WaitForSeconds(GlobalValues.OutlineOffDelay);
		private bool _isMouseHoveringOver;

		public OutlineableComponent(GameObject obj)
		{
			var outline = obj.GetComponentInChildren<Outline>();
			if(!outline)
			{
				outline = obj.AddComponent<Outline>();
				outline.OutlineColor = GlobalValues.PickupColor;
				outline.enabled = false;
			}

			_outline = outline;
			_outline.enabled = false;
		}

		public void ShowOutline(MonoBehaviour mono)
		{
			_isMouseHoveringOver = true;
			_outlineRoutine ??= _outlineRoutine.StartCoroutine(mono, DisableOutline());
		}

		private IEnumerator DisableOutline()
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