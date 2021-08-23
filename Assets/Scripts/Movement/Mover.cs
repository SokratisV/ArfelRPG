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
		public bool IsMoving => !MeshAgent.isStopped;
		public float CurrentSpeed { get; set; }
		public NavMeshAgent MeshAgent { get; private set; }

		[SerializeField] private float maxSpeed = 6f, timeBeforeIdle = 10f;

		private bool _lockMovement;
		private float _distanceBeforeReachingDestination, _idleTimer = 5f, _timeMoved, _distanceTravelled;
		private Health _health;
		private Animator _animator;
		private Coroutine _selfUpdateRoutine;
		private ActionScheduler _actionScheduler;
		private static readonly int ForwardSpeed = Animator.StringToHash("forwardSpeed");
		private static readonly int SuddenStop = Animator.StringToHash("suddenStop");
		private static readonly int IdleAnimations = Animator.StringToHash("idleAnimations");
		private static readonly int DodgeHash = Animator.StringToHash("dodge");

		#region Unity

		private void Awake()
		{
			MeshAgent = GetComponent<NavMeshAgent>();
			_animator = GetComponent<Animator>();
			_actionScheduler = GetComponent<ActionScheduler>();
			_health = GetComponent<Health>();
			CurrentSpeed = maxSpeed;
		}

		private void OnEnable()
		{
			_health.OnDeath += DisableMover;
			_selfUpdateRoutine = _selfUpdateRoutine.StartCoroutine(this, UpdateMover());
		}

		private void OnDisable()
		{
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
			if (!hasPath) return false;
			if (path.status != NavMeshPathStatus.PathComplete) return false;
			return !(Helper.GetPathLength(path) > GlobalValues.MaxNavPathLength);
		}

		public IAction Move(Vector3 destination, float speedFraction = 1f, float withinDistance = 0f)
		{
			if (MoveWithoutAction(destination, speedFraction, withinDistance))
			{
				_actionScheduler.StartAction(this);
				return this;
			}

			return null;
		}

		public bool MoveWithoutAction(Vector3 destination, float speedFraction = 1f, float withinDistance = 0f)
		{
			if (!MeshAgent.enabled || _lockMovement) return false;
			MeshAgent.destination = destination;
			_distanceBeforeReachingDestination = withinDistance;
			MeshAgent.speed = CurrentSpeed * Mathf.Clamp01(speedFraction);
			MeshAgent.isStopped = false;
			_idleTimer = 0;
			return true;
		}

		public void Dash(Vector3 destination, float duration)
		{
			_lockMovement = true;
			var initialAcceleration = MeshAgent.acceleration;
			var currentPosition = transform.position;
			var speedRequired = Vector3.Distance(currentPosition, destination) / duration;
			Helper.DoAfterSeconds(() =>
			{
				MeshAgent.acceleration = initialAcceleration;
				_lockMovement = false;
			}, duration, this);
			MeshAgent.acceleration *= 2;
			MeshAgent.destination = destination;
			MeshAgent.speed = speedRequired;
			MeshAgent.isStopped = false;
		}

		public void Dodge(Vector3 destination)
		{
			_animator.SetTrigger(DodgeHash);
			Dash(destination, GlobalValues.DodgeDuration);
		}

		public void Blink(Vector3 point, float delay = .4f)
		{
			RotateOverTime(0.2f, point);
			LockMovementFor(delay + .2f);
			Helper.DoAfterSeconds(() => { MeshAgent.Warp(point); }, delay, this);
		}

		public void LockMovementFor(float duration)
		{
			_lockMovement = true;
			Helper.DoAfterSeconds(() => { _lockMovement = false; }, duration, this);
		}

		public void CancelAction()
		{
			if (!MeshAgent.enabled) return;
			MeshAgent.isStopped = true;
			_idleTimer = 0;
			_timeMoved = 0;
			_animator.ResetTrigger(SuddenStop);
		}

		public void CompleteAction()
		{
			MeshAgent.isStopped = true;
			_actionScheduler.CompleteAction();
			_idleTimer = 0;
			_timeMoved = 0;
			_animator.ResetTrigger(SuddenStop);
			OnActionComplete?.Invoke();
		}

		public void ExecuteQueuedAction(IActionData data)
		{
			var moveData = (MoverActionData) data;
			MoveWithoutAction(moveData.Destination, moveData.SpeedFraction, moveData.StopDistance);
		}

		public void QueueAction(IActionData data) => _actionScheduler.EnqueueAction(data);

		public void RotateOverTime(float time, Vector3 targetPosition) => StartCoroutine(_RotateOverTime(time, targetPosition));

		public void RevertToOriginalSpeed() => CurrentSpeed = maxSpeed;

		public void EnableMover()
		{
			MeshAgent.enabled = true;
			_selfUpdateRoutine = _selfUpdateRoutine.StartCoroutine(this, UpdateMover());
		}

		#endregion

		#region Private

		private IEnumerator _RotateOverTime(float time, Vector3 targetPosition)
		{
			var currentRotation = transform.rotation;
			var lookRotation = Quaternion.LookRotation(targetPosition - transform.position);
			var progress = 0f;
			while (progress < 1)
			{
				transform.rotation = Quaternion.Slerp(currentRotation, lookRotation, progress);
				progress += Time.deltaTime / time;
				yield return null;
			}
		}

		private void DisableMover()
		{
			MeshAgent.velocity = Vector3.zero;
			MeshAgent.enabled = false;
			_selfUpdateRoutine.StopCoroutine(this);
		}

		private IEnumerator UpdateMover()
		{
			while (true)
			{
				_idleTimer += Time.deltaTime;
				CheckIfDestinationIsReached();
				UpdateAnimator();
				yield return null;
			}
		}

		private void CheckIfDestinationIsReached()
		{
			if (!MeshAgent.isStopped)
			{
				_timeMoved += Time.deltaTime;
				if (_timeMoved > 2) _animator.SetTrigger(SuddenStop);
				if (Helper.IsWithinDistance(transform.position, MeshAgent.destination, _distanceBeforeReachingDestination))
				{
					CompleteAction();
				}
			}
		}

		private void UpdateAnimator()
		{
			var velocity = MeshAgent.velocity;
			var localVelocity = transform.InverseTransformDirection(velocity);
			var speed = localVelocity.z;
			_animator.SetFloat(ForwardSpeed, speed);
			if (_idleTimer > timeBeforeIdle)
			{
				_animator.SetTrigger(IdleAnimations);
				_idleTimer = 0;
			}
		}

		#endregion

		#region State

		public void RestoreState(object state)
		{
			var position = (SerializableVector3) state;
			MeshAgent.enabled = false;
			transform.position = position.ToVector();
			MeshAgent.enabled = true;
		}

		public object CaptureState() => new SerializableVector3(transform.position);

		#endregion
	}
}