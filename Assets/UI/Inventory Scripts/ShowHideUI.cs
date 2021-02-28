using UnityEngine;

namespace RPG.UI
{
	public class ShowHideUI : MonoBehaviour
	{
		[SerializeField] private RectTransform uiContainer = null;
		[SerializeField] private KeyCode toggleKey = KeyCode.Escape;
		[SerializeField] private Vector2 _hiddenPosition;
		// [SerializeField] private LeanTweenType _tweenType;
		[SerializeField] private bool startState;
		
		private Vector2 _initialPosition;
		private Canvas _canvas;

		private void Awake()
		{
			Debug.Log("VAR");
			_canvas = GetComponent<Canvas>();
			_initialPosition = uiContainer.anchoredPosition;
			uiContainer.anchoredPosition = _hiddenPosition;
			_canvas.enabled = startState;
		}

		private void Update()
		{
			if(Input.GetKeyDown(toggleKey))
			{
				Toggle();
			}
		}

		[ContextMenu("Toggle")]
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