using UnityEngine;
using UnityEngine.UI;

namespace RPG.Skills
{
	public class TargetQuadIndicator : SkillIndicatorBase, ISkillIndicator
	{
		[SerializeField] private Image quadImage;

		protected override void Init() => Type = Behaviors.IndicatorType.TargetCircle;
		protected override void ChangeColor(Color32 color) => quadImage.color = color;

		protected override void ChangeColor(byte customAlpha = default)
		{
			if (customAlpha > 0)
			{
				Color32 newColor = quadImage.color;
				newColor.a = customAlpha;
				quadImage.color = newColor;
			}
		}

		public override void ShowIndicator(Skill skill, GameObject _)
		{
			base.ShowIndicator(skill, _);
			var indicatorRadius = skill.Radius * 2;
			quadImage.transform.localScale = new Vector3(indicatorRadius, indicatorRadius, indicatorRadius);
			gameObject.SetActive(true);
		}

		public void UpdateIndicator(Vector3 position) => transform.position = position;

		public void HideIndicator() => gameObject.SetActive(false);
	}
}