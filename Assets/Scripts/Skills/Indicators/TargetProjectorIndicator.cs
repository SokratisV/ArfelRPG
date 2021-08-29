using UnityEngine;

namespace RPG.Skills
{
	public class TargetProjectorIndicator : SkillIndicatorBase, ISkillIndicator
	{
		private Projector _projector;

		protected override void Init()
		{
			_projector = GetComponentInChildren<Projector>();
			_projector.enabled = false;
			Type = Behaviors.IndicatorType.TargetCircle;
		}

		public void ShowIndicator(Skill skill, GameObject _)
		{
			var sizeAdjustment = skill.Radius * 3 / 100;
			_projector.orthographicSize = skill.Radius + sizeAdjustment;
			_projector.enabled = true;
		}

		public void UpdateIndicator(Vector3 position) => transform.position = position;

		public void HideIndicator() => _projector.enabled = false;
	}
}