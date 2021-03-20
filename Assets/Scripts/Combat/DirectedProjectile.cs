using RPG.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
	public class DirectedProjectile : MonoBehaviour
	{
		[SerializeField] private float speed = 1f, maxLifeTime = 5f, lifeAfterImpact = 2f;
		[SerializeField] private GameObject hitEffect = null;
		[SerializeField] private GameObject[] destroyOnHit = null;
		[SerializeField] private UnityEvent onHit;
		private GameObject _instigator = null;
		private float _damage = 0f;
		
		private void Update() => transform.Translate(Vector3.forward * (speed * Time.deltaTime));

		public void Setup(GameObject instigator, float damage, float newSpeed = 0)
		{
			_damage = damage;
			_instigator = instigator;
			if(newSpeed > 0) speed = newSpeed;
			Destroy(gameObject, maxLifeTime);
		}
		
		private void OnTriggerEnter(Collider other)
		{
			if(other.TryGetComponent(out Health health))
			{
				if (health.gameObject == _instigator) return;
				health.TakeDamage(_instigator, _damage);
				speed = 0;
				onHit.Invoke();
				if(hitEffect != null)
				{
					Instantiate(hitEffect, other.ClosestPoint(transform.position), transform.rotation);
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