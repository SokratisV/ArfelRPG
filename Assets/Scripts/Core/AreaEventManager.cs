using System.Collections.Generic;
using RPG.Core.SystemEvents;
using UnityEngine;

namespace RPG.Core
{
	public class AreaEventManager : MonoBehaviour
	{
		[SerializeField] private AreasEvent areasEvent;
		private Areas _currentArea = Areas.None;
		private Stack<Areas> _areasCurrentlyIn;

		private void Awake() => _areasCurrentlyIn = new Stack<Areas>();

		private void Start() => areasEvent.Raise(_currentArea);

		public void EnterNewArea(Areas area)
		{
			_areasCurrentlyIn.Push(_currentArea);
			_currentArea = area;
			areasEvent.Raise(_currentArea);
		}

		public void ExitArea(Areas area)
		{
			if (_areasCurrentlyIn.Peek() == area)
			{
				_areasCurrentlyIn.Pop();
				return;
			}

			var previousArea = _areasCurrentlyIn.Pop();
			_currentArea = previousArea;
			areasEvent.Raise(_currentArea);
		}
	}
}