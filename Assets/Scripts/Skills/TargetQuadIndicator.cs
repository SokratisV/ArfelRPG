using UnityEngine;

namespace RPG.Skills
{
	public class TargetQuadIndicator : SkillIndicatorBase, ISkillIndicator
	{
		public void ShowIndicator(Skill skill, GameObject _)
		{
			gameObject.SetActive(true);
			transform.localScale = new Vector3(skill.Radius * 2, 1, skill.Radius * 2);
		}

		public void UpdateIndicator(Vector3 position) => transform.position = position;

		public void HideIndicator() => gameObject.SetActive(false);
	}
}