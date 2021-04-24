using UnityEngine;

namespace RPG.Combat
{
	public class BodyParts : MonoBehaviour
	{
		[SerializeField] private Transform rightHand, leftHand;
		[SerializeField] private Transform rightFoot, leftFoot;
		[SerializeField] private Transform projectileLocation;

		private Transform _originalProjectileLocation;
		private void Awake() => _originalProjectileLocation = projectileLocation;

		public Transform ProjectileLocation
		{
			get => projectileLocation == null ? transform : projectileLocation;
			set => projectileLocation = value;
		}

		public void RevertProjectileLocation() => ProjectileLocation = _originalProjectileLocation;

		public Transform RightFoot => rightFoot;
		public Transform LeftFoot => leftFoot;
		public Transform LeftHand => leftHand == null ? transform : leftHand;
		public Transform RightHand => rightHand == null ? transform : rightHand;
	}
}