using RPG.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
	public class TargetedProjectile : MonoBehaviour
	{
		[SerializeField] private float speed = 1f, maxLifeTime = 5f, lifeAfterImpact = 2f;
		[SerializeField] private bool isHoming = false;
		[SerializeField] private GameObject hitEffect = null;
		[SerializeField] private GameObject[] destroyOnHit = null;
		[SerializeField] private UnityEvent onHit;
		private Health _target = null;
		private CapsuleCollider _targetCollider;
		private GameObject _instigator = null;
		private float _damage = 0f;

		private void Start() => transform.LookAt(GetAimLocation());

		private void Update()
		{
			if(_target == null) return;
			if(isHoming && !_target.IsDead)
			{
				transform.LookAt(GetAimLocation());
			}

			transform.Translate(Vector3.forward * (speed * Time.deltaTime));
		}

		public void SetTarget(Health target, GameObject instigator, float damage, float newSpeed = 0)
		{
			_damage = damage;
			_target = target;
			_targetCollider = _target.GetComponent<CapsuleCollider>();
			_instigator = instigator;
			if(newSpeed > 0) speed = newSpeed;
			Destroy(gameObject, maxLifeTime);
		}

		private Vector3 GetAimLocation()
		{
			if(_targetCollider == null)
			{
				return _target.transform.position;
			}

			return _target.transform.position + Vector3.up * (_targetCollider.height / 2);
		}

		private void OnTriggerEnter(Collider other)
		{
			if(other.GetComponent<Health>() == _target)
			{
				if(_target.IsDead) return;
				_target.TakeDamage(_instigator, _damage);
				speed = 0;
				onHit.Invoke();
				if(hitEffect != null)
				{
					Instantiate(hitEffect, GetAimLocation(), transform.rotation);
				}

				foreach(var toDestroy in destroyOnHit)
				{
					Destroy(toDestroy);
				}

				Destroy(gameObject, lifeAfterImpact);
			}
		}
	}
}