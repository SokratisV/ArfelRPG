using System.Linq;
using Sirenix.OdinInspector;
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

#if UNITY_EDITOR
		[Button(ButtonSizes.Large)]
		private void UpdateReferences()
		{
			rightHand = GetComponentsInChildren<Transform>().FirstOrDefault(c => c.gameObject.name == "Hand_R" || c.gameObject.name == "RightHand");
			projectileLocation = rightHand;
			leftHand = GetComponentsInChildren<Transform>().FirstOrDefault(c => c.gameObject.name == "Hand_L"|| c.gameObject.name == "LeftHand");
			rightFoot = GetComponentsInChildren<Transform>().FirstOrDefault(c => c.gameObject.name == "Ankle_R"|| c.gameObject.name == "RightFoot");
			leftFoot = GetComponentsInChildren<Transform>().FirstOrDefault(c => c.gameObject.name == "Ankle_L"|| c.gameObject.name == "LeftFoot");
		}
#endif
	}
}