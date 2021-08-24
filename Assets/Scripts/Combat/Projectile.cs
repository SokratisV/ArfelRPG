using RPG.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
	public class Projectile : MonoBehaviour
	{
		[SerializeField] private float speed = 1f, maxLifeTime = 5f, lifeAfterImpact = 2f;
		[SerializeField] private bool isHoming = false;
		[SerializeField] private GameObject hitEffect = null;
		[SerializeField] private GameObject[] destroyOnHit = null;
		[SerializeField] private UnityEvent onHit;
		private Health _target = null;
		private Vector3? _targetPoint;
		private Collider _targetCollider = null;
		private GameObject _instigator = null;
		private float _damage = 0f;

		private void Start()
		{
			var aimLocation = GetAimLocation();
			if (aimLocation != null) transform.LookAt(aimLocation.Value);
		}

		private void Update()
		{
			if (_target != null && isHoming && !_target.IsDead)
			{
				transform.LookAt(GetAimLocation().Value);
			}
			
			transform.Translate(Vector3.forward * (speed * Time.deltaTime));
		}

		private void Setup(GameObject instigator, float damage, Health target = null, Vector3? targetPoint = null, float newSpeed = 0, float lifeTime = -1)
		{
			_damage = damage;
			_target = target;
			if (_target != null) _targetCollider = _target.GetComponent<Collider>();
			_instigator = instigator;
			_targetPoint = targetPoint;
			if (newSpeed > 0) speed = newSpeed;
			Destroy(gameObject, lifeTime > 0 ? lifeTime : maxLifeTime);
		}

		public void Setup(Vector3 targetPoint, GameObject instigator, float damage, float newSpeed = 0, float lifeTime = -1) => Setup(instigator, damage, null, targetPoint, newSpeed, lifeTime);
		public void Setup(Health target, GameObject instigator, float damage, float newSpeed = 0, float lifeTime = -1) => Setup(instigator, damage, target, newSpeed: newSpeed, lifeTime:lifeTime);
		public void Setup(GameObject instigator, float damage, float newSpeed = 0, float lifeTime = -1) => Setup(instigator, damage, null, null, newSpeed, lifeTime);

		private Vector3? GetAimLocation()
		{
			if (_target != null)
			{
				if (_targetCollider == null)
				{
					return _target.transform.position;
				}
				return _target.transform.position + Vector3.up * (_targetCollider.bounds.size.y / 2);
			}

			return _targetPoint;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out Health health))
			{
				if (_target != null) TargetedProjectile(health);
				else DirectedProjectile(health);
			}
		}

		private void DirectedProjectile(Health health)
		{
			if (health.IsDead) return;
			if (health.gameObject == _instigator) return;
			TriggerProjectile(health);
		}

		private void TargetedProjectile(Health health)
		{
			if (_target != health) return;
			if (_target.IsDead) return;
			if (_target.gameObject == _instigator) return;
			TriggerProjectile(health);
		}

		private void TriggerProjectile(Health health)
		{
			health.TakeDamage(_instigator, _damage);
			speed = 0;
			onHit.Invoke();
			health.TakeDamage(_instigator, _damage);
			if (hitEffect != null)
			{
				Instantiate(hitEffect, transform.position, transform.rotation);
			}

			foreach (var toDestroy in destroyOnHit)
			{
				Destroy(toDestroy);
			}

			Destroy(gameObject, lifeAfterImpact);
		}
	}
}