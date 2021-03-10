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
		private Rigidbody _rigidbody;
		private NavMeshAgent _navMeshAgent;
		private Coroutine _selfUpdateRoutine;
		private ActionScheduler _actionScheduler;
		private static readonly int ForwardSpeed = Animator.StringToHash("forwardSpeed");
		private static readonly int Roll = Animator.StringToHash("roll");
		private static readonly int BlinkHash = Animator.StringToHash("blink");

		#region Unity

		private void Awake()
		{
			_navMeshAgent = GetComponent<NavMeshAgent>();
			_animator = GetComponent<Animator>();
			_actionScheduler = GetComponent<ActionScheduler>();
			_health = GetComponent<Health>();
			_rigidbody = GetComponent<Rigidbody>();
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

		public bool IsInRange(Transform targetTransform, float withinDistance) =>
			Helper.IsWithinDistance(transform, targetTransform, withinDistance);

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
			DisableMoverFor(duration, () => _rigidbody.isKinematic = true);
			transform.rotation = Quaternion.LookRotation(destination - transform.position);
			_rigidbody.isKinematic = false;
			var currentPosition = transform.position;
			var speedRequired = Vector3.Distance(currentPosition, destination) / duration;
			var direction = (destination - currentPosition).normalized;
			_rigidbody.velocity = direction * speedRequired;
			_animator.SetTrigger(Roll);
		}

		public void Blink(Vector3 point)
		{
			_animator.SetTrigger(BlinkHash);
			DisableMoverFor(.4f, () => _navMeshAgent.Warp(point));
			transform.rotation = Quaternion.LookRotation(point - transform.position);
		}

		public void DisableMoverFor(float duration, Action extraActionOnEnd = null) => StartCoroutine(DisableForSeconds(duration, extraActionOnEnd));

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

		private IEnumerator DisableForSeconds(float seconds, Action extraActionOnEnd = null)
		{
			DisableMover();
			yield return new WaitForSeconds(seconds);
			extraActionOnEnd?.Invoke();
			yield return null;
			EnableMover();
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