using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RPG.UI
{
	public class ShowHideUIOnButtonPress : MonoBehaviour
	{
		public UnityEvent<bool> ActionOnToggle;

		[SerializeField] private RectTransform uiContainer = null;
		[SerializeField] private KeyCode toggleKey = KeyCode.Escape, alternateToggle = KeyCode.None;
		[SerializeField] private Vector2 visiblePosition;
		[SerializeField] private float tweenTime = .2f;
		[SerializeField] private bool disableRaycastingOnHide = false;
		[SerializeField] private bool changeSortingOrder = true;
		[SerializeField] private bool toggleOnStart;


		private static int _sortingOrder;
		private Vector2 _initialPosition;
		private Canvas _canvas;
		private GraphicRaycaster _raycaster;

		private void Awake()
		{
			_canvas = GetComponent<Canvas>();
			_canvas.enabled = false;
			_raycaster = GetComponent<GraphicRaycaster>();
			_initialPosition = uiContainer.anchoredPosition;
			if (toggleOnStart) Toggle(true);
		}

		private void Update()
		{
			if (Input.GetKeyDown(toggleKey) || Input.GetKeyDown(alternateToggle))
			{
				Toggle(!_canvas.enabled);
			}
		}

		[Button(ButtonSizes.Large, ButtonStyle.Box, Name = "Toggle")]
		public void Toggle() => Toggle(!_canvas.enabled);

		public void Toggle(bool toggle)
		{
			if (toggle == _canvas.enabled) return;
			ActionOnToggle?.Invoke(toggle);
			if (!toggle)
			{
				LeanTween.cancel(uiContainer);
				ToggleRaycaster();
				LeanTween.move(uiContainer, _initialPosition, tweenTime).setEaseInOutExpo().setOnComplete(ToggleCanvas).setIgnoreTimeScale(true);
			}
			else
			{
				LeanTween.cancel(uiContainer);
				ToggleCanvas();
				LeanTween.move(uiContainer, visiblePosition, tweenTime).setEaseInOutExpo().setIgnoreTimeScale(true).setOnComplete(ToggleRaycaster);
			}
		}

		private void ToggleCanvas()
		{
			_canvas.enabled = !_canvas.enabled;
			if (!changeSortingOrder) return;
			if (_canvas.enabled) _canvas.sortingOrder = ++_sortingOrder;
			else _canvas.sortingOrder = 0;
		}

		private void ToggleRaycaster()
		{
			if (!disableRaycastingOnHide) return;
			_raycaster.enabled = _canvas.enabled;
		}

#if UNITY_EDITOR
		[Button(ButtonSizes.Large)]
		private void SaveVisiblePosition() => visiblePosition = uiContainer.anchoredPosition;
#endif
	}
}