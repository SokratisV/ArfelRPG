using GameDevTV.Utils;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;
using UnityEngine;
using System;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f, suspicionTime = 3f, wayPointTolerance = 1f, wayPointDwellTime = 3f, aggroCooldownTime = 5f, shoutDistance = 0f;
        [SerializeField] PatrolPath patrolPath = default;
        [Range(0, 1)] [SerializeField] float patrolSpeedFraction = 0.2f;

        public static event Action<bool> onPlayerAggro = delegate { };

        GameObject player;
        Health health;
        Fighter fighter;
        Mover mover;
        LazyValue<Vector3> guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity, timeSinceArrivedAtWaypoint = Mathf.Infinity, timeSinceAggrevated = Mathf.Infinity;
        int currentWayPointIndex = 0;
        bool hasInformedPlayerOfAggro = false;

        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            guardPosition = new LazyValue<Vector3>(GetGuardPosition);
        }

        private Vector3 GetGuardPosition()
        {
            return transform.position;
        }

        private void Start()
        {
            guardPosition.ForceInit();
        }

        private void Update()
        {
            if (IsDead()) return;

            if (IsAggrevated() && fighter.CanAttack(player))
            {
                AttackBehaviour();
            }
            else if (IsSuspicious())
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }

            UpdateTimers();
        }

        private bool IsDead()
        {
            if (health.IsDead())
            {
                if (hasInformedPlayerOfAggro == true)
                {
                    onPlayerAggro(false);
                    hasInformedPlayerOfAggro = false;
                }
                GetComponent<Collider>().enabled = false;
                return true;
            }
            return false;
        }

        private bool IsSuspicious()
        {
            if (timeSinceLastSawPlayer < suspicionTime) { return true; }
            else
            {
                if (hasInformedPlayerOfAggro == true)
                {
                    onPlayerAggro(false);
                    hasInformedPlayerOfAggro = false;
                }
                return false;
            }
        }

        public void Aggrevate()
        {
            timeSinceAggrevated = 0;
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
            timeSinceAggrevated += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition.value;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            if (timeSinceArrivedAtWaypoint > wayPointDwellTime)
            {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWayPointIndex);
        }

        private void CycleWaypoint()
        {
            currentWayPointIndex = patrolPath.GetNextIndex(currentWayPointIndex);
        }

        private bool AtWaypoint()
        {
            float distanceToWayPoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWayPoint < wayPointTolerance;
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            if (hasInformedPlayerOfAggro == false)
            {
                hasInformedPlayerOfAggro = true;
                onPlayerAggro(true);
            }
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);

            AggrevateNearbyEnemies();
        }

        private void AggrevateNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
            AIController ai;
            foreach (RaycastHit hit in hits)
            {
                if (ai = hit.transform.GetComponent<AIController>())
                {
                    ai.Aggrevate();
                }
            }
        }

        private bool IsAggrevated()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return (distanceToPlayer < chaseDistance) || (timeSinceAggrevated < aggroCooldownTime);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }

}