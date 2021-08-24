using UnityEngine;
using UnityEngine.UI;

namespace RPG.Skills
{
	public class ConeIndicator : SkillIndicatorBase, ISkillIndicator
	{
		[SerializeField] private Image coneImage;
		private Transform _user;

		public void ShowIndicator(Skill skill, GameObject user)
		{
			_user = user.transform;
			coneImage.fillAmount = skill.SpecialFloat1 / 360;
			coneImage.rectTransform.rotation = Quaternion.Euler(90, skill.SpecialFloat1 / 2, 0);
			var indicatorRange = skill.SpecialFloat2 > 0 ? skill.SpecialFloat2 : 15;
			coneImage.transform.localScale = Vector3.one * indicatorRange;
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