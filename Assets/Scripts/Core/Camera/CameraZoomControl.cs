using Cinemachine;
using UnityEngine;

namespace RPG.Core
{
	public class CameraZoomControl : MonoBehaviour
	{
		[SerializeField] private float zoomSpeed = 3f;

		private float _zoom = 18f;
		private CinemachineFramingTransposer _cmCam;

		private void Awake()
		{
			_cmCam = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
			_zoom = _cmCam.m_CameraDistance;
		}

		private void LateUpdate()
		{
			var mouseScroll = Input.mouseScrollDelta.y;
			if(mouseScroll < 0 || mouseScroll > 0)
			{
				_zoom -= mouseScroll * zoomSpeed;
				_zoom = Mathf.Clamp(_zoom, 5f, 20f);
				_cmCam.m_CameraDistance = _zoom;
			}
		}
	}
}