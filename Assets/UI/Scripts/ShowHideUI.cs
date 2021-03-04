using UnityEngine;

namespace RPG.UI
{
	public class ShowHideUI : MonoBehaviour
	{
		[SerializeField] private bool toggleOnEnable;
		[SerializeField] private RectTransform uiContainer = null;
		[SerializeField] private KeyCode toggleKey = KeyCode.Escape;
		[SerializeField] private Vector2 hiddenPosition;
		[SerializeField] private float tweenTime = .2f;

		[SerializeField] private bool startState;
		// [SerializeField] private LeanTweenType _tweenType;

		private Vector2 _initialPosition;
		private Canvas _canvas;

		private void Awake()
		{
			_canvas = GetComponent<Canvas>();
			_initialPosition = uiContainer.anchoredPosition;
			Toggle(startState);
		}

		private void OnEnable()
		{
			if(toggleOnEnable) Toggle(true);
		}

		private void OnDisable()
		{
			if(toggleOnEnable) Toggle(false);
		}

		private void Update()
		{
			if(Input.GetKeyDown(toggleKey))
			{
				Toggle(!_canvas.enabled);
			}
		}

		[ContextMenu("Toggle")]
		public void Toggle()
		{
			Toggle(!_canvas.enabled);
		}

		public void Toggle(bool toggle)
		{
			if(toggle == _canvas.enabled) return;
			if(!toggle)
			{
				LeanTween.cancel(uiContainer);
				LeanTween.move(uiContainer, hiddenPosition, tweenTime).setEaseInOutExpo().setOnComplete(ToggleCanvas);
			}
			else
			{
				ToggleCanvas();
				LeanTween.cancel(uiContainer);
				LeanTween.move(uiContainer, _initialPosition, tweenTime).setEaseInOutExpo();
			}
		}

		private void ToggleCanvas() => _canvas.enabled = !_canvas.enabled;
	}
}