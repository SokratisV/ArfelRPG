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
        NavMeshAgent navMeshAgent;
        Health health;
        [SerializeField] float maxSpeed = 6f;
        [SerializeField] float maxNavPathLength = 40f;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }
        void Update()
        {
            navMeshAgent.enabled = !health.IsDead();
            UpdateAnimator();
        }
        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }
        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            StartCoroutine(_CompleteMove(destination));
            navMeshAgent.isStopped = false;
        }
        private IEnumerator _CompleteMove(Vector3 destination)
        {
            float range = GetComponent<Fighter>().GetWeaponConfig().GetRange();
            // Not precise, but cheaper(?)
            // while ((transform.position - destination).sqrMagnitude > range * range)
            // {
            //     yield return null;
            // } 
            while (Vector3.Distance(transform.position, destination) > 0.1f) { yield return null; }
            Complete();
        }
        public bool CanMoveTo(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength(path) > maxNavPathLength) return false;
            return true;
        }
        private float GetPathLength(NavMeshPath path)
        {
            float total = 0f;
            if (path.corners.Length < 2) return total;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }

            return total;
        }
        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }
        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }
        public void QueueMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().EnqueueAction(new MoverActionData(this, destination, speedFraction));
        }
        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }
        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = position.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
        }
        public void Complete()
        {
            GetComponent<ActionScheduler>().CompleteAction();
        }
        public void ExecuteAction(ActionData data)
        {
            Vector3 destination = ((MoverActionData)data).destination;
            float speed = ((MoverActionData)data).speed;

            MoveTo(destination, speed);
        }
    }
}
