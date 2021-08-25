using UnityEngine;

namespace RPG.Skills
{
	public class TargetQuadIndicator : SkillIndicatorBase, ISkillIndicator
	{
		[SerializeField] private Transform quadTransform;

		private void Awake() => Type = Behaviors.IndicatorType.TargetCircle;

		public void ShowIndicator(Skill skill, GameObject _)
		{
			var indicatorRadius = skill.Radius * 2;
			quadTransform.localScale = new Vector3(indicatorRadius, indicatorRadius, indicatorRadius);
			gameObject.SetActive(true);
		}

		public void UpdateIndicator(Vector3 position) => transform.position = position;

		public void HideIndicator() => gameObject.SetActive(false);
	}
}