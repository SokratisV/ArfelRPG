using System;
using System.Collections;
using RPG.Attributes;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;

namespace RPG.Movement
{
	[SelectionBase]
	[RequireComponent(typeof(Rigidbody))]
	public class Mover : MonoBehaviour, IAction, ISaveable
	{
		public event Action OnActionComplete;
		public bool IsMoving => !_navMeshAgent.isStopped;

		[SerializeField] private float maxSpeed = 6f;

		private float _distanceBeforeReachingDestination;
		private Health _health;
		private Animator _animator;
		private NavMeshAgent _navMeshAgent;
		private Coroutine _selfUpdateRoutine;
		private ActionScheduler _actionScheduler;
		private static readonly int ForwardSpeed = Animator.StringToHash("forwardSpeed");

		#region Unity

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

		#endregion

		#region Public

		public bool IsInRange(Transform targetTransform, float withinDistance) => Helper.IsWithinDistance(transform, targetTransform, withinDistance);

		public bool IsInRange(Vector3 targetPoint, float withinDistance) => Helper.IsWithinDistance(transform.position, targetPoint, withinDistance);

		public bool CanMoveTo(Vector3 destination)
		{
			var path = new NavMeshPath();
			var hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
			if(!hasPath) return false;
			if(path.status != NavMeshPathStatus.PathComplete) return false;
			return!(Helper.GetPathLength(path) > GlobalValues.MaxNavPathLength);
		}

		public IAction Move(Vector3 destination, float speedFraction = 1f, float withinDistance = 0f)
		{
			_actionScheduler.StartAction(this);
			MoveWithoutAction(destination, speedFraction, withinDistance);
			return this;
		}

		public void MoveWithoutAction(Vector3 destination, float speedFraction = 1f, float withinDistance = 0f)
		{
			if(!_navMeshAgent.enabled) return;
			_navMeshAgent.destination = destination;
			_distanceBeforeReachingDestination = withinDistance;
			_navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
			_navMeshAgent.isStopped = false;
		}

		public void Dash(Vector3 destination, float duration)
		{
			var initialAcceleration = _navMeshAgent.acceleration;
			var currentPosition = transform.position;
			var speedRequired = Vector3.Distance(currentPosition, destination) / duration;
			Helper.DoAfterSeconds(() => _navMeshAgent.acceleration = initialAcceleration, duration, this);
			_navMeshAgent.acceleration *= 2;
			_navMeshAgent.destination = destination;
			_navMeshAgent.speed = speedRequired;
			_navMeshAgent.isStopped = false;
		}

		public void Blink(Vector3 point)
		{
			DisableMoverFor(.4f, () => _navMeshAgent.Warp(point));
			StartCoroutine(RotateOverTime(0.2f, point));
		}

		public void CancelAction()
		{
			if(!_navMeshAgent.enabled) return;
			_navMeshAgent.isStopped = true;
		}

		public void CompleteAction()
		{
			_navMeshAgent.isStopped = true;
			_actionScheduler.CompleteAction();
			OnActionComplete?.Invoke();
		}

		public void ExecuteQueuedAction(IActionData data)
		{
			var moveData = (MoverActionData)data;
			MoveWithoutAction(moveData.Destination, moveData.Speed, moveData.StopDistance);
		}

		public void QueueMoveAction(Vector3 destination, float speedFraction = 1f, float withinDistance = 0f) => _actionScheduler.EnqueueAction(new MoverActionData(this, destination, speedFraction, withinDistance));

		#endregion

		#region Private

		private IEnumerator RotateOverTime(float time, Vector3 targetPosition)
		{
			var currentRotation = transform.rotation;
			var lookRotation = Quaternion.LookRotation(targetPosition - transform.position);
			var progress = 0f;
			while(progress < 1)
			{
				transform.rotation = Quaternion.Slerp(currentRotation, lookRotation, progress);
				progress += Time.deltaTime / time;
				yield return null;
			}
		}

		private void DisableMoverFor(float duration, Action extraActionOnEnd = null)
		{
			DisableMover();
			Helper.DoAfterSeconds(() =>
			{
				extraActionOnEnd?.Invoke();
				EnableMover();
			}, duration, this);
		}

		private void DisableMover()
		{
			_navMeshAgent.enabled = false;
			_selfUpdateRoutine.StopCoroutine(this);
		}

		private void EnableMover()
		{
			_navMeshAgent.enabled = true;
			_selfUpdateRoutine = _selfUpdateRoutine.StartCoroutine(this, UpdateMover());
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

		#endregion

		#region State

		public void RestoreState(object state)
		{
			var position = (SerializableVector3)state;
			_navMeshAgent.enabled = false;
			transform.position = position.ToVector();
			_navMeshAgent.enabled = true;
		}

		public object CaptureState() => new SerializableVector3(transform.position);

		#endregion
	}
}