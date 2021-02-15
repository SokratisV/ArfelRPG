using System;
using System.Collections;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using RPG.Attributes;

namespace RPG.Movement
{
	[SelectionBase]
	public class Mover : MonoBehaviour, IAction, ISaveable
	{
		public event Action OnActionComplete;

		private float _distanceBeforeReachingDestination;
		private NavMeshAgent _navMeshAgent;
		private Animator _animator;
		private ActionScheduler _actionScheduler;
		private Health _health;
		[SerializeField] private float maxSpeed = 6f;
		[SerializeField] private float maxNavPathLength = 40f;
		private Coroutine _selfUpdateRoutine;
		private static readonly int ForwardSpeed = Animator.StringToHash("forwardSpeed");

		private void Awake()
		{
			_navMeshAgent = GetComponent<NavMeshAgent>();
			_animator = GetComponent<Animator>();
			_actionScheduler = GetComponent<ActionScheduler>();
			_health = GetComponent<Health>();
		}

		private void OnEnable()
		{
			Health.OnPlayerDeath += DisableMover;
			_health.OnDeath += DisableMover;
			_selfUpdateRoutine = _selfUpdateRoutine.StartCoroutine(this, UpdateMover());
		}

		private void OnDisable()
		{
			Health.OnPlayerDeath -= DisableMover;
			_health.OnDeath -= DisableMover;
			DisableMover();
		}

		private void DisableMover()
		{
			_navMeshAgent.enabled = false;
			_selfUpdateRoutine.StopCoroutine(this);
		}

		private IEnumerator UpdateMover()
		{
			while(true)
			{
				CheckIfDestinationIsReached();
				UpdateAnimator();
				yield return null;
			}
		}

		private void CheckIfDestinationIsReached()
		{
			if(!_navMeshAgent.isStopped)
			{
				if(Helper.IsWithinDistance(transform.position, _navMeshAgent.destination, _distanceBeforeReachingDestination))
				{
					CompleteAction();
				}
			}
		}

		private void UpdateAnimator()
		{
			var velocity = _navMeshAgent.velocity;
			var localVelocity = transform.InverseTransformDirection(velocity);
			var speed = localVelocity.z;
			_animator.SetFloat(ForwardSpeed, speed);
		}

		public void MoveWithoutAction(Vector3 destination, float speedFraction = 1f)
		{
			_navMeshAgent.destination = destination;
			_navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
			_navMeshAgent.isStopped = false;
		}

		public bool CanMoveTo(Vector3 destination)
		{
			var path = new NavMeshPath();
			var hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
			if(!hasPath) return false;
			if(path.status != NavMeshPathStatus.PathComplete) return false;
			return!(GetPathLength(path) > maxNavPathLength);
		}

		private float GetPathLength(NavMeshPath path)
		{
			var total = 0f;
			if(path.corners.Length < 2) return total;
			for(var i = 0;i < path.corners.Length - 1;i++)
			{
				total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
			}

			return total;
		}

		public void CancelAction() => _navMeshAgent.isStopped = true;

		public IAction Move(Vector3 destination, float speedFraction = 1f, float withinDistance = 0f)
		{
			_actionScheduler.StartAction(this);
			_distanceBeforeReachingDestination = withinDistance;
			MoveWithoutAction(destination, speedFraction);
			return this;
		}

		public void QueueMoveAction(Vector3 destination, float speedFraction = 1f, float withinDistance = 0f) => _actionScheduler.EnqueueAction(new MoverActionData(this, destination, speedFraction, withinDistance));

		public object CaptureState() => new SerializableVector3(transform.position);

		public void RestoreState(object state)
		{
			var position = (SerializableVector3)state;
			_navMeshAgent.enabled = false;
			transform.position = position.ToVector();
			_navMeshAgent.enabled = true;
		}

		public void CompleteAction()
		{
			_navMeshAgent.isStopped = true;
			_actionScheduler.CompleteAction();
			OnActionComplete?.Invoke();
		}

		public void ExecuteAction(IActionData data)
		{
			var destination = ((MoverActionData)data).Destination;
			var speed = ((MoverActionData)data).Speed;
			_distanceBeforeReachingDestination = ((MoverActionData)data).StopDistance;
			MoveWithoutAction(destination, speed);
		}
	}
}