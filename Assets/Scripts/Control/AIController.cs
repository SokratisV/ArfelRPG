using RPG.Utils;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;
using RPG.Attributes;
using RPG.Core.SystemEvents;
using UnityEngine.AI;

namespace RPG.Control
{
	public class AIController : MonoBehaviour
	{
		[SerializeField] private float chaseDistance = 5f,
			suspicionTime = 3f,
			wayPointTolerance = 1f,
			wayPointDwellTime = 3f,
			aggroCooldownTime = 5f,
			shoutDistance = 0f,
			aggroShoutInterval = .4f;

		[SerializeField] private PatrolPath patrolPath;
		[Range(0, 1)] [SerializeField] private float patrolSpeedFraction = 0.2f;
		[SerializeField] private BooleanEvent onPlayerAggro;

		private GameObject _player;
		private Health _health;
		private Fighter _fighter;
		private Mover _mover;
		private LazyValue<Vector3> _guardPosition;

		private float _timeSinceLastSawPlayer = Mathf.Infinity,
			_timeSinceArrivedAtWaypoint = Mathf.Infinity,
			_timeSinceAggrevated = Mathf.Infinity,
			_timeSinceNotifiedOthers = 0;

		private int _currentWayPointIndex = 0;
		private bool _hasInformedPlayerOfAggro = false;

		#region Unity

		private void Awake()
		{
			_player = PlayerFinder.Player;
			_fighter = GetComponent<Fighter>();
			_health = GetComponent<Health>();
			_mover = GetComponent<Mover>();
			_guardPosition = new LazyValue<Vector3>(GetGuardPosition);
			_guardPosition.ForceInit();
		}

		private void OnEnable()
		{
			_health.OnHealthChange += AttackAttacker;
			_health.OnDeath += MarkDead;
		}

		private void Update()
		{
			if (_health.IsDead) return;

			if (IsAggrevated() && _fighter.CanExecute(_player))
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

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(transform.position, chaseDistance);
		}

		#endregion

		#region Public

		public void Aggrevate() => _timeSinceAggrevated = 0;

		public void ResetEnemy()
		{
			GetComponent<NavMeshAgent>().Warp(_guardPosition.Value);
			_timeSinceLastSawPlayer = Mathf.Infinity;
			_timeSinceArrivedAtWaypoint = Mathf.Infinity;
			_timeSinceAggrevated = Mathf.Infinity;
			_timeSinceNotifiedOthers = 0;
			_currentWayPointIndex = 0;
			_hasInformedPlayerOfAggro = false;
		}

		#endregion

		#region Private

		private Vector3 GetGuardPosition() => transform.position;

		private void AttackAttacker(GameObject obj, float _)
		{
			if (_health.IsDead)
			{
				_health.OnHealthChange -= AttackAttacker;
				return;
			}

			Aggrevate();
		}

		private void MarkDead()
		{
			if (_hasInformedPlayerOfAggro)
			{
				onPlayerAggro.Raise(false);
				_hasInformedPlayerOfAggro = false;
				GetComponent<Collider>().enabled = false;
				_health.OnDeath -= MarkDead;
			}
		}

		private bool IsSuspicious()
		{
			if (_timeSinceLastSawPlayer < suspicionTime) return true;

			if (_hasInformedPlayerOfAggro)
			{
				onPlayerAggro.Raise(false);
				_hasInformedPlayerOfAggro = false;
			}

			return false;
		}

		private bool IsAggrevated() => Helper.IsWithinDistance(_player.transform, transform, chaseDistance) || _timeSinceAggrevated < aggroCooldownTime;

		private void UpdateTimers()
		{
			_timeSinceLastSawPlayer += Time.deltaTime;
			_timeSinceArrivedAtWaypoint += Time.deltaTime;
			_timeSinceNotifiedOthers += Time.deltaTime;
			_timeSinceAggrevated += Time.deltaTime;
		}

		private void PatrolBehaviour()
		{
			var nextPosition = _guardPosition.Value;

			if (patrolPath != null)
			{
				if (AtWaypoint())
				{
					_timeSinceArrivedAtWaypoint = 0;
					CycleWaypoint();
				}

				nextPosition = GetCurrentWaypoint();
			}

			if (_timeSinceArrivedAtWaypoint > wayPointDwellTime)
			{
				_mover.Move(nextPosition, patrolSpeedFraction);
			}
		}

		private Vector3 GetCurrentWaypoint() => patrolPath.GetWaypoint(_currentWayPointIndex);
		private void CycleWaypoint() => _currentWayPointIndex = patrolPath.GetNextIndex(_currentWayPointIndex);

		private bool AtWaypoint() => Helper.IsWithinDistance(transform.position, GetCurrentWaypoint(), wayPointTolerance);

		private void SuspicionBehaviour() => GetComponent<ActionScheduler>().CancelCurrentAction();

		private void AttackBehaviour()
		{
			if (!_hasInformedPlayerOfAggro)
			{
				_hasInformedPlayerOfAggro = true;
				onPlayerAggro.Raise(true);
			}

			_fighter.Execute(_player);
			_timeSinceLastSawPlayer = 0;
			AggrevateNearbyEnemies();
		}

		private void AggrevateNearbyEnemies()
		{
			if (_timeSinceNotifiedOthers <= aggroShoutInterval) return;
			_timeSinceNotifiedOthers = 0f;

			var hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
			foreach (var hit in hits)
			{
				if (hit.transform.TryGetComponent(out AIController ai))
				{
					if (ai == this) continue;
					if (ai.IsAggrevated()) continue;
					ai.Aggrevate();
				}
			}
		}

		#endregion
	}
}