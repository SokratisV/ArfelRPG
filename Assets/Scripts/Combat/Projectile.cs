﻿using System.Collections;
using RPG.Attributes;
using RPG.Core;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
	public class Projectile : MonoBehaviour
	{
		[SerializeField] private bool stopOnContact = true;
		[SerializeField] private float speed = 1f, maxLifeTime = 5f, lifeAfterImpact = 2f;
		[SerializeField] private bool isHoming = false;
		[SerializeField] private GameObject hitEffect = null;
		[SerializeField] private GameObject[] destroyOnHit = null;
		[SerializeField] private UnityEvent onHit;
		private Health _target = null;
		private Vector3? _direction;
		private Collider _targetCollider = null;
		private GameObject _instigator = null;
		private float _damage = 0f;
		private Coroutine _selfDestructTimer;

		public float Speed => speed;

		private void Start()
		{
			var aimLocation = GetAimLocation();
			if (aimLocation.HasValue) transform.LookAt(aimLocation.Value);
			var aimDirection = GetAimDirection();
			if (aimDirection.HasValue) transform.rotation = Quaternion.LookRotation(aimDirection.Value);
		}

		private void Update()
		{
			if (_target != null && isHoming && !_target.IsDead)
			{
				transform.LookAt(GetAimLocation().Value);
			}

			transform.Translate(Vector3.forward * (speed * Time.deltaTime));
		}

		private void Setup(GameObject instigator, float damage, Health target = null, Vector3? direction = null, float newSpeed = 0, float lifeTime = -1)
		{
			_damage = damage;
			_target = target;
			if (_target != null) _targetCollider = _target.GetComponent<Collider>();
			_instigator = instigator;
			_direction = direction;
			if (newSpeed > 0) speed = newSpeed;
			_selfDestructTimer = _selfDestructTimer.StartCoroutine(this, DestroyAfterTime(lifeTime > 0 ? lifeTime : maxLifeTime));
		}

		public void Setup(Vector3 direction, GameObject instigator, float damage, float newSpeed = 0, float lifeTime = -1) => Setup(instigator, damage, null, direction, newSpeed, lifeTime);
		public void Setup(Health target, GameObject instigator, float damage, float newSpeed = 0, float lifeTime = -1) => Setup(instigator, damage, target, newSpeed: newSpeed, lifeTime: lifeTime);
		public void Setup(GameObject instigator, float damage, float newSpeed = 0, float lifeTime = -1) => Setup(instigator, damage, null, null, newSpeed, lifeTime);

		private Vector3? GetAimLocation()
		{
			if (_target == null) return null;
			if (_targetCollider == null) return _target.transform.position;
			return _target.transform.position + Vector3.up * (_targetCollider.bounds.size.y / 2);
		}

		private Vector3? GetAimDirection() => _direction;

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out Health health))
			{
				if (_target != null) TargetedProjectile(health);
				else DirectedProjectile(health);
			}
		}

		private IEnumerator DestroyAfterTime(float time)
		{
			var timer = 0f;
			while (timer < time)
			{
				timer += Time.deltaTime;
				yield return null;
			}

			SpawnHitEffects();
			SelfDestruct();
		}

		private void SpawnHitEffects()
		{
			if (hitEffect != null)
			{
				Destroy(Instantiate(hitEffect, transform.position, transform.rotation), 3f);
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
			SpawnHitEffects();
			if (stopOnContact)
			{
				speed = 0;
				onHit.Invoke();
				SelfDestruct();
			}
		}

		private void SelfDestruct()
		{
			foreach (var toDestroy in destroyOnHit)
			{
				Destroy(toDestroy);
			}

			Destroy(gameObject, lifeAfterImpact);
			_selfDestructTimer.StopCoroutine(this);
		}
	}
}