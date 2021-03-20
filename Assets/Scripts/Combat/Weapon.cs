using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
	public class Weapon : MonoBehaviour
	{
		[SerializeField] private Transform projectileLocation;
		[SerializeField] private UnityEvent onHit;

		public Transform ProjectileLocation => projectileLocation;

		// Animation event
		public void OnHit() => onHit.Invoke();

		public void Destroy() => Destroy(gameObject);
	}
}