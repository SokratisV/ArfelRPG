using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
	public class AreaEventManager : MonoBehaviour
	{
		public static event Action<Areas> OnEnterArea;
		[SerializeField] private Areas currentArea = Areas.None;
		private Stack<Areas> _areasCurrentlyIn;

		private void Awake() => _areasCurrentlyIn = new Stack<Areas>();

		private void Start() => OnEnterArea?.Invoke(currentArea);

		public void EnterNewArea(Areas area)
		{
			_areasCurrentlyIn.Push(currentArea);
			currentArea = area;
			OnEnterArea?.Invoke(area);
		}

		public void ExitArea(Areas area)
		{
			if(_areasCurrentlyIn.Peek() == area)
			{
				_areasCurrentlyIn.Pop();
				return;
			}

			var previousArea = _areasCurrentlyIn.Pop();
			currentArea = previousArea;
			OnEnterArea?.Invoke(previousArea);
		}
	}
}