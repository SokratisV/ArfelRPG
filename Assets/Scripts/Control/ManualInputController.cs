using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
	public class ManualInputController
	{
		private readonly Transform _character;
		private readonly NavMeshAgent _agent;
		private readonly Mover _mover;
		private Vector3 _inputValue;
		private Vector3 _newPosition;
		private const float Speed = 150;

		public ManualInputController(Mover mover)
		{
			_mover = mover;
			_agent = mover.MeshAgent;
			_character = _agent.transform;
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
			_newPosition = _character.position + _inputValue * (Time.deltaTime * Speed);
			if (NavMesh.SamplePosition(_newPosition, out var hit, .3f, NavMesh.AllAreas))
			{
				if ((_character.position - hit.position).magnitude >= .2f)
				{
					_mover.Move(_newPosition);
				}
			}
		}
	}
}