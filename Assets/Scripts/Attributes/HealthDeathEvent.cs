using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
	[RequireComponent(typeof(Health))]
	public class HealthDeathEvent : MonoBehaviour
	{
		[SerializeField] private UnityEvent healthDeath;
		private Health _health;

		private void Awake() => _health = GetComponent<Health>();
		private void OnEnable() => _health.OnDeath += InvokeDeathAction;
		private void OnDisable() => _health.OnDeath -= InvokeDeathAction;
		private void InvokeDeathAction() => healthDeath?.Invoke();
	}
}