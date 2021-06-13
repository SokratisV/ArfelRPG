using UnityEngine;

namespace RPG.Core
{
	public class CameraController : MonoBehaviour
	{
		[SerializeField] private Transform target;
		
		public float RotationAdjustedSpeed { get; set; } = 3.5f;

		private void LateUpdate()
		{
			if (Input.GetMouseButton(1) && Time.timeScale > 0)
			{
				var position = target.position;
				transform.RotateAround(position, Vector3.up, Input.GetAxis("Mouse X") * RotationAdjustedSpeed);
			}
		}
	}
}