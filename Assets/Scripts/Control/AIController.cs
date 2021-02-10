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
        [SerializeField] private float chaseDistance = 5f,
            suspicionTime = 3f,
            wayPointTolerance = 1f,
            wayPointDwellTime = 3f,
            aggroCooldownTime = 5f,
            shoutDistance = 0f;

        [SerializeField] private PatrolPath patrolPath = default;
        [Range(0, 1)] [SerializeField] private float patrolSpeedFraction = 0.2f;

        public static event Action<bool, CombatMusicAreas> OnPlayerAggro = delegate {};

        private GameObject _player;
        private Health _health;
        private Fighter _fighter;
        private Mover _mover;
        private LazyValue<Vector3> _guardPosition;

        private float _timeSinceLastSawPlayer = Mathf.Infinity,
            _timeSinceArrivedAtWaypoint = Mathf.Infinity,
            _timeSinceAggrevated = Mathf.Infinity;

        private int _currentWayPointIndex = 0;
        private bool _hasInformedPlayerOfAggro = false;
        [SerializeField] private CombatMusicAreas combatMusic;

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            _mover = GetComponent<Mover>();
            _guardPosition = new LazyValue<Vector3>(GetGuardPosition);
        }

        private Vector3 GetGuardPosition()
        {
            return transform.position;
        }

        private void Start()
        {
            _guardPosition.ForceInit();
        }

        private void Update()
        {
            if(IsDead()) return;

            if(IsAggrevated() && _fighter.CanAttack(_player))
            {
                AttackBehaviour();
            }
            else if(IsSuspicious())
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
            if(_health.IsDead())
            {
                if(_hasInformedPlayerOfAggro)
                {
                    OnPlayerAggro(false, combatMusic);
                    _hasInformedPlayerOfAggro = false;
                }

                GetComponent<Collider>().enabled = false;
                return true;
            }

            return false;
        }

        private bool IsSuspicious()
        {
            if(_timeSinceLastSawPlayer < suspicionTime)
            {
                return true;
            }

            if(_hasInformedPlayerOfAggro)
            {
                OnPlayerAggro(false, combatMusic);
                _hasInformedPlayerOfAggro = false;
            }

            return false;
        }

        public void Aggrevate()
        {
            _timeSinceAggrevated = 0;
        }

        private void UpdateTimers()
        {
            _timeSinceLastSawPlayer += Time.deltaTime;
            _timeSinceArrivedAtWaypoint += Time.deltaTime;
            _timeSinceAggrevated += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            var nextPosition = _guardPosition.Value;

            if(patrolPath != null)
            {
                if(AtWaypoint())
                {
                    _timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }

                nextPosition = GetCurrentWaypoint();
            }

            if(_timeSinceArrivedAtWaypoint > wayPointDwellTime)
            {
                _mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(_currentWayPointIndex);
        }

        private void CycleWaypoint()
        {
            _currentWayPointIndex = patrolPath.GetNextIndex(_currentWayPointIndex);
        }

        private bool AtWaypoint()
        {
            var distanceToWayPoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWayPoint < wayPointTolerance;
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            if(_hasInformedPlayerOfAggro == false)
            {
                _hasInformedPlayerOfAggro = true;
                OnPlayerAggro(true, combatMusic);
            }

            _timeSinceLastSawPlayer = 0;
            _fighter.Attack(_player);

            AggrevateNearbyEnemies();
        }

        private void AggrevateNearbyEnemies()
        {
            var hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
            foreach(var hit in hits)
            {
                if(hit.transform.TryGetComponent(out AIController ai))
                    ai.Aggrevate();
            }
        }

        private bool IsAggrevated()
        {
            var distanceToPlayer = Vector3.Distance(_player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance || _timeSinceAggrevated < aggroCooldownTime;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}