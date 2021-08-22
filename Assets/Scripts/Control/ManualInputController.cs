using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
	public class ManualInputController
	{
		private readonly Transform _character;
		private readonly Mover _mover;
		private Vector3 _inputValue;
		private Vector3 _newPosition;
		private readonly Transform _cameraTransform;
		private const float Speed = 150;

		public ManualInputController(Mover mover, Camera mainCam)
		{
			_mover = mover;
			_character = mover.MeshAgent.transform;
			_cameraTransform = mainCam.transform;
		}

		public void Update()
		{
			UpdateMoveVector();
			Move();
		}

		private void UpdateMoveVector()
		{
			_inputValue.x = Input.GetAxis("Horizontal");
			_inputValue.z = Input.GetAxis("Vertical");
		}

		private void Move()
		{
			if (_inputValue.magnitude < .01f) return;
			_newPosition = _character.position + MoveDirection() * (Time.deltaTime * Speed);
			if (NavMesh.SamplePosition(_newPosition, out var hit, .3f, NavMesh.AllAreas))
			{
				if ((_character.position - hit.position).magnitude >= .2f)
				{
					_mover.Move(_newPosition);
				}
			}
		}

		private Vector3 MoveDirection()
		{
			var camF = _cameraTransform.forward;
			var camR = _cameraTransform.right;
			camF.y = 0;
			camR.y = 0;
			var moveDirection = camF * _inputValue.z + camR * _inputValue.x;
			return moveDirection;
		}
	}
}