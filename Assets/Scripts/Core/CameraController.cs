using UnityEngine;

namespace RPG.Core
{
	public class CameraController : MonoBehaviour
	{
		[SerializeField] private float rotationSpeed;
		[SerializeField] private Transform target;
		
		private void LateUpdate()
		{
			if(Input.GetMouseButton(1))
			{
				var position = target.position;
				transform.RotateAround(position, Vector3.up, Input.GetAxis("Mouse X") * rotationSpeed);
			}
		}
	}
}