using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using RPG.Attributes;
using System.Collections;
using RPG.Combat;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        private NavMeshAgent _navMeshAgent;
        private Health _health;
        private Animator _animator;
        private ActionScheduler _actionScheduler;
        private NavMeshAgent _agent;
        [SerializeField] private float maxSpeed = 6f;
        [SerializeField] private float maxNavPathLength = 40f;
        private bool _completeCoroutineStarted = false;
        private Coroutine _completeCoroutine = null;
        private static readonly int ForwardSpeed = Animator.StringToHash("forwardSpeed");

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _health = GetComponent<Health>();
            _animator = GetComponent<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            _navMeshAgent.enabled = !_health.IsDead();
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            var velocity = _navMeshAgent.velocity;
            var localVelocity = transform.InverseTransformDirection(velocity);
            var speed = localVelocity.z;
            _animator.SetFloat(ForwardSpeed, speed);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            _navMeshAgent.destination = destination;
            _navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            //TODO: Remove from common (Player/AI) code
            if(gameObject.CompareTag("Player"))
            {
                if(_completeCoroutineStarted)
                {
                    StopCoroutine(_completeCoroutine);
                }

                _completeCoroutine = StartCoroutine(_CompleteMove(destination));
            }

            _navMeshAgent.isStopped = false;
        }

        private IEnumerator _CompleteMove(Vector3 destination)
        {
            _completeCoroutineStarted = true;
            var range = GetComponent<Fighter>().GetWeaponConfig().GetRange();
            while(Vector3.Distance(transform.position, destination) > 0.1f)
            {
                yield return null;
            }

            Complete();
            _completeCoroutineStarted = false;
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

        public void Cancel()
        {
            _navMeshAgent.isStopped = true;
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            _actionScheduler.StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void QueueMoveAction(Vector3 destination, float speedFraction)
        {
            _actionScheduler.EnqueueAction(new MoverActionData(this, destination, speedFraction));
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            var position = (SerializableVector3)state;
            _agent.enabled = false;
            transform.position = position.ToVector();
            _agent.enabled = true;
        }

        public void Complete()
        {
            _actionScheduler.CompleteAction();
        }

        public void ExecuteAction(IActionData data)
        {
            var destination = ((MoverActionData)data).Destination;
            var speed = ((MoverActionData)data).Speed;

            MoveTo(destination, speed);
        }
    }
}