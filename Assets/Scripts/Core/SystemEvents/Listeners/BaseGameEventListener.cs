using Core.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Core.SystemEvents
{
	public abstract class BaseGameEventListener<T, E, UER> : MonoBehaviour,
		IGameEventListener<T> where E : BaseGameEvent<T> where UER : UnityEvent<T>
	{
		[SerializeField] private E gameEvent;

		public E GameEvent
		{
			get => gameEvent;
			set => gameEvent = value;
		}

		[SerializeField] private UER unityEventResponse;

		private void OnEnable() => GameEvent.RegisterListener(this);
		private void OnDisable() => GameEvent.UnregisterListener(this);
		public void RaiseEvent(T data) => unityEventResponse?.Invoke(data);
	}
}