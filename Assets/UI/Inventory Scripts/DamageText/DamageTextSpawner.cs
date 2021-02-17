using UnityEngine;

namespace RPG.UI.DamageText
{
	public class DamageTextSpawner : MonoBehaviour
	{
		[SerializeField] private float additionalHeight = .5f;
		[SerializeField] private DamageText damageTextPrefab = null;
		private CapsuleCollider _collider;

		private void Awake() => _collider = GetComponentInParent<CapsuleCollider>();

		public void Spawn(float damageAmount)
		{
			var text = Instantiate(damageTextPrefab, transform);
			var transform1 = text.transform;
			var vector3 = transform1.position + Vector3.up * (_collider.height + additionalHeight);
			transform1.position = vector3;
			text.SetValue(damageAmount);
		}
	}
}