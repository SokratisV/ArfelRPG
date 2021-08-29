using UnityEngine;
using UnityEngine.UI;

namespace RPG.Skills
{
	public class PlayerQuadIndicator : SkillIndicatorBase, ISkillIndicator
	{
		[SerializeField] private Image quadImage;
		
		private Transform _userTransform;
		
		protected override void Init()
		{
			Type = Behaviors.IndicatorType.PlayerCircle;
			quadImage.material = InstancedMaterial;
		}

		public void ShowIndicator(Skill skill, GameObject user)
		{
			_userTransform = user.transform;
			var indicatorRadius = skill.CastingRange * 2;
			quadImage.transform.localScale = new Vector3(indicatorRadius, indicatorRadius, indicatorRadius);
			gameObject.SetActive(true);
		}

		public void UpdateIndicator(Vector3 position) => transform.position = _userTransform.position;

		public void HideIndicator() => gameObject.SetActive(false);
	}
}