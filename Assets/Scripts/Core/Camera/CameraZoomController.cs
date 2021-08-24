using Cinemachine;
using UnityEngine;

namespace RPG.Core
{
	public class CameraZoomController : MonoBehaviour
	{
		private float _zoom = 18f;
		private CinemachineFramingTransposer _cmCam;

		public float ZoomSpeed { get; set; } = 3;

		private void Awake()
		{
			_cmCam = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
			_zoom = _cmCam.m_CameraDistance;
		}

		private void LateUpdate()
		{
			if (Time.timeScale > 0)
			{
				var mouseScroll = Input.mouseScrollDelta.y;
				if(mouseScroll < 0 || mouseScroll > 0)
				{
					_zoom -= mouseScroll * ZoomSpeed;
					_zoom = Mathf.Clamp(_zoom, 5f, 20f);
					_cmCam.m_CameraDistance = _zoom;
				}
			}
		}
	}
}