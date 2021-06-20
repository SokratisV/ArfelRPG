using System.Collections.Generic;
using Core.Interfaces;
using UnityEngine;

namespace RPG.Core.SystemEvents
{
	public abstract class BaseGameEvent<T> : ScriptableObject
	{
		private readonly List<IGameEventListener<T>> _listeners = new List<IGameEventListener<T>>();

		public void RegisterListener(IGameEventListener<T> listener)
		{
			if (_listeners.Contains(listener)) return;
			_listeners.Add(listener);
		}

		public void UnregisterListener(IGameEventListener<T> listener)
		{
			if (!_listeners.Contains(listener)) return;
			_listeners.Remove(listener);
		}

		public void Raise(T data)
		{
			for (var i = _listeners.Count - 1; i >= 0; --i)
			{
				_listeners[i].RaiseEvent(data);
			}
		}
	}
}