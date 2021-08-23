using UnityEngine;

namespace RPG.Skills
{
	public class PlayerQuadIndicator : SkillIndicatorBase, ISkillIndicator
	{
		private Transform _userTransform;
		public void ShowIndicator(Skill skill, GameObject user)
		{
			_userTransform = user.transform;
			transform.localScale = new Vector3(skill.CastingRange * 2, 1, skill.CastingRange * 2);
			transform.position = _userTransform.position;
			gameObject.SetActive(true);
		}

		public void UpdateIndicator(Vector3 position) => transform.position = _userTransform.position;

		public void HideIndicator() => gameObject.SetActive(false);
	}
}