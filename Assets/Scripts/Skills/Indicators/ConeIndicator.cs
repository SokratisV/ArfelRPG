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

		protected override void Init()
		{
			Type = Behaviors.IndicatorType.Cone;
			_thisTransform = transform;
		}

		protected override void ChangeColor(Color32 color) => coneImage.color = color;

		protected override void ChangeColor(byte customAlpha = default)
		{
			if (customAlpha > 0)
			{
				Color32 newColor = coneImage.color;
				newColor.a = customAlpha;
				coneImage.color = newColor;
			}
		}

		public override void ShowIndicator(Skill skill, GameObject user)
		{
			base.ShowIndicator(skill, user);
			_user = user.transform;
			var specialFloat1 = skill.SpecialFloats[0];
			var specialFloat2 = skill.SpecialFloats[1];
			coneImage.fillAmount = specialFloat1 / 360;
			coneImage.transform.localRotation = Quaternion.Euler(0, 0, -specialFloat1 / 2);
			var indicatorRange = specialFloat2 > 0 ? specialFloat2 : 15;
			coneImage.transform.localScale = Vector3.one * (indicatorRange * 2);
			gameObject.SetActive(true);
		}

		public void UpdateIndicator(Vector3 target)
		{
			_thisTransform.position = _user.position;
			Helper.RotateToLocation(_thisTransform, target);
		}

		public void HideIndicator() => gameObject.SetActive(false);
	}
}