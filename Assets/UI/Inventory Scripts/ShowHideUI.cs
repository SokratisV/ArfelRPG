using UnityEngine;

namespace RPG.UI
{
	public class ShowHideUI : MonoBehaviour
	{
		[SerializeField] private RectTransform uiContainer = null;
		[SerializeField] private KeyCode toggleKey = KeyCode.Escape;
		[SerializeField] private Vector2 _hiddenPosition;
		[SerializeField] private LeanTweenType _tweenType;

		private Vector2 _initialPosition;
		private Canvas _canvas;

		private void Awake()
		{
			_canvas = GetComponent<Canvas>();
			_initialPosition = uiContainer.anchoredPosition;
			uiContainer.anchoredPosition = _hiddenPosition;
			_canvas.enabled = false;
		}

		private void Update()
		{
			if(Input.GetKeyDown(toggleKey))
			{
				Toggle();
			}
		}

		public void Toggle()
		{
			if(_canvas.enabled)
			{
				LeanTween.cancel(uiContainer);
				LeanTween.move(uiContainer, _hiddenPosition, .3f).setEaseInOutExpo().setOnComplete(ToggleCanvas);
			}
			else
			{
				ToggleCanvas();
				LeanTween.cancel(uiContainer);
				LeanTween.move(uiContainer, _initialPosition, .3f).setEaseInOutExpo();
			}
		}

		private void ToggleCanvas() => _canvas.enabled = !_canvas.enabled;
	}
}