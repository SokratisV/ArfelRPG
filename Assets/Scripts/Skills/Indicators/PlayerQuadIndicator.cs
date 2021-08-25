using UnityEngine;

namespace RPG.Skills
{
	public class PlayerQuadIndicator : SkillIndicatorBase, ISkillIndicator
	{
		[SerializeField] private Transform quadTransform;
		private Transform _userTransform;
		
		private void Awake() => Type = Behaviors.IndicatorType.PlayerCircle;

		public void ShowIndicator(Skill skill, GameObject user)
		{
			_userTransform = user.transform;
			var indicatorRadius = skill.CastingRange * 2;
			quadTransform.localScale = new Vector3(indicatorRadius, indicatorRadius, indicatorRadius);
			gameObject.SetActive(true);
		}

		public void UpdateIndicator(Vector3 position) => transform.position = _userTransform.position;

		public void HideIndicator() => gameObject.SetActive(false);
	}
}