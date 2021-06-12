using UnityEngine;

namespace RPG.Core
{
	public class CameraController : MonoBehaviour
	{
		[SerializeField] private Transform target;

		public float RotationSpeed { get; set; } = 10;

		private void LateUpdate()
		{
			if(Input.GetMouseButton(1) && Time.timeScale > 0)
			{
				var position = target.position;
				transform.RotateAround(position, Vector3.up, Input.GetAxis("Mouse X") * RotationSpeed);
			}
		}
	}
}