using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Skills
{
	public class ConeIndicator : SkillIndicatorBase, ISkillIndicator
	{
		[SerializeField] private Image coneImage;
		private Transform _user;

		private void Awake() => Type = Behaviors.IndicatorType.Cone;

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
			transform.position = _user.position;
			var lookPos = position - transform.position;
			lookPos.y = 0;
			var rotation = Quaternion.LookRotation(lookPos);
			transform.rotation = rotation;
		}

		public void HideIndicator() => gameObject.SetActive(false);
	}
}