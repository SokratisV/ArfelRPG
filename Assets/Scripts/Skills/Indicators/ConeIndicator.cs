using RPG.Core;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Skills
{
	public class ConeIndicator : SkillIndicatorBase, ISkillIndicator
	{
		[SerializeField] private Image coneImage;

		private Transform _thisTransform;
		private Transform _user;

		private void Awake()
		{
			Type = Behaviors.IndicatorType.Cone;
			_thisTransform = transform;
		}

		public void ShowIndicator(Skill skill, GameObject user)
		{
			_user = user.transform;
			var specialFloat1 = skill.SpecialFloats[0];
			var specialFloat2 = skill.SpecialFloats[1];
			coneImage.fillAmount = specialFloat1 / 360;
			coneImage.transform.localRotation = Quaternion.Euler(0, 0, -specialFloat1 / 2);
			var indicatorRange = specialFloat2 > 0 ? specialFloat2 : 15;
			coneImage.transform.localScale = Vector3.one * (indicatorRange * 2);
			gameObject.SetActive(true);
		}

		public void UpdateIndicator(Vector3 position)
		{
			_thisTransform.position = _user.position;
			Helper.RotateFromScreenSpaceDirection(_thisTransform);
		}

		public void HideIndicator() => gameObject.SetActive(false);
	}
}