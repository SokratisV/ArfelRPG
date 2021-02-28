using UnityEngine;

namespace RPG.UI
{
	public class RotateTween : MonoBehaviour
	{
		[SerializeField] private float degrees;
		[SerializeField] [Range(0, 1f)] private float timeItTakes;

		[SerializeField] private RectTransform rectTransform;

		private float _initialRotationValue;
		private bool _isInInitialState = false;

		private void Awake()
		{
			rectTransform = GetComponent<RectTransform>();
			_initialRotationValue = rectTransform.rotation.z;
		}

		public void OnClick()
		{
			LeanTween.rotateZ(gameObject, !_isInInitialState? degrees:_initialRotationValue, timeItTakes);
			_isInInitialState = !_isInInitialState;
		}
	}
}