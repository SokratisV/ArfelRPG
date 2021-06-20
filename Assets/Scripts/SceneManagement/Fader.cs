using System;
using System.Collections;
using RPG.Core;
using UnityEngine;

namespace RPG.SceneManagement
{
	public class Fader : MonoBehaviour
	{
		private CanvasGroup _canvasGroup;
		private Coroutine _currentActiveFade = null;
		private bool _hasFadedOut;

		private void Awake() => _canvasGroup = GetComponent<CanvasGroup>();

		public void FadeOutImmediate() => _canvasGroup.alpha = 1;

		//Called by FloatEvent
		public void Fade(float time) => Fade(_hasFadedOut ? 0 : 1, time);

		public Coroutine FadeOutRoutine(float time) => Fade(1, time);
		public Coroutine FadeInRoutine(float time) => Fade(0, time);

		private Coroutine Fade(float target, float time)
		{
			_currentActiveFade = _currentActiveFade.StartCoroutine(this, FadeRoutine(target, time));
			return _currentActiveFade;
		}

		private IEnumerator FadeRoutine(float target, float time)
		{
			while(!Mathf.Approximately(_canvasGroup.alpha, target))
			{
				_canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, target, Time.unscaledDeltaTime / time);
				yield return null;
			}

			_hasFadedOut = !_hasFadedOut;
		}
	}
}