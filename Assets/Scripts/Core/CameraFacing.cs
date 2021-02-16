using UnityEngine;

namespace RPG.Core
{
	public class CameraFacing : MonoBehaviour
	{
		private Transform _mainCamera;

		private void Start() => _mainCamera = Camera.main.transform;

		private void LateUpdate() => transform.forward = _mainCamera.forward;
	}
}