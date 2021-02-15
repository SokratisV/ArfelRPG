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
		private GameObject _instigator = null;
		private float _weaponDamage = 0f, _arrowDamage = 1f;

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

		public void SetTarget(Health target, GameObject instigator, float damage)
		{
			_weaponDamage = damage;
			_target = target;
			_instigator = instigator;

			Destroy(gameObject, maxLifeTime);
		}

		private Vector3 GetAimLocation()
		{
			var capsuleCollider = _target.GetComponent<CapsuleCollider>();
			if(capsuleCollider == null)
			{
				return _target.transform.position;
			}

			return _target.transform.position + Vector3.up * (capsuleCollider.height / 2);
		}

		private void OnTriggerEnter(Collider other)
		{
			if(other.GetComponent<Health>() == _target)
			{
				if(_target.IsDead) return;
				_target.TakeDamage(_instigator, CalculateDamage());
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

		private float CalculateDamage() => _weaponDamage * _arrowDamage;
	}
}