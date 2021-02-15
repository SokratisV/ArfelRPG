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
			shoutDistance = 0f,
			aggroShoutInterval = .4f;

		[SerializeField] private PatrolPath patrolPath;
		[Range(0, 1)] [SerializeField] private float patrolSpeedFraction = 0.2f;

		public static event Action<bool, CombatMusicAreas> OnPlayerAggro;

		private GameObject _player;
		private Health _health;
		private Fighter _fighter;
		private Mover _mover;
		private ActionScheduler _actionScheduler;
		private LazyValue<Vector3> _guardPosition;

		private float _timeSinceLastSawPlayer = Mathf.Infinity,
			_timeSinceArrivedAtWaypoint = Mathf.Infinity,
			_timeSinceAggrevated = Mathf.Infinity,
			_timeSinceNotifiedOthers = 0;

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
			_actionScheduler = GetComponent<ActionScheduler>();
		}

		private Vector3 GetGuardPosition() => transform.position;

		private void OnEnable() => _health.OnTakeDamage += AttackAttacker;

		private void AttackAttacker(GameObject obj)
		{
			if(_health.IsDead)
			{
				_health.OnTakeDamage -= AttackAttacker;
				return;
			}

			Aggrevate();
		}

		private void Start() => _guardPosition.ForceInit();

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
			if(_health.IsDead)
			{
				if(_hasInformedPlayerOfAggro)
				{
					OnPlayerAggro?.Invoke(false, combatMusic);
					_hasInformedPlayerOfAggro = false;
				}

				GetComponent<Collider>().enabled = false;
				return true;
			}

			return false;
		}

		private bool IsSuspicious()
		{
			if(_timeSinceLastSawPlayer < suspicionTime) return true;

			if(_hasInformedPlayerOfAggro)
			{
				OnPlayerAggro?.Invoke(false, combatMusic);
				_hasInformedPlayerOfAggro = false;
			}

			return false;
		}

		public void Aggrevate() => _timeSinceAggrevated = 0;

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
				_mover.Move(nextPosition, patrolSpeedFraction);
			}
		}

		private Vector3 GetCurrentWaypoint() => patrolPath.GetWaypoint(_currentWayPointIndex);

		private void CycleWaypoint() => _currentWayPointIndex = patrolPath.GetNextIndex(_currentWayPointIndex);

		private bool AtWaypoint() => Helper.IsWithinDistance(transform.position, GetCurrentWaypoint(), wayPointTolerance);

		private void SuspicionBehaviour() => GetComponent<ActionScheduler>().CancelCurrentAction();

		private void AttackBehaviour()
		{
			if(!_hasInformedPlayerOfAggro)
			{
				_hasInformedPlayerOfAggro = true;
				OnPlayerAggro?.Invoke(true, combatMusic);
			}

			_fighter.Attack(_player);
			_timeSinceLastSawPlayer = 0;
			AggrevateNearbyEnemies();
		}

		private void AggrevateNearbyEnemies()
		{
			if(_timeSinceNotifiedOthers <= aggroShoutInterval) return;
			_timeSinceNotifiedOthers = 0f;

			var hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
			foreach(var hit in hits)
			{
				if(hit.transform.TryGetComponent(out AIController ai))
				{
					if (ai == this) continue;
					if (ai.IsAggrevated()) continue;
					ai.Aggrevate();
				}
			}
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(transform.position, chaseDistance);
		}
	}
}