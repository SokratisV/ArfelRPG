using RPG.Core;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Skills
{
	public class LineIndicator : SkillIndicatorBase, ISkillIndicator
	{
		[SerializeField] private Image lineImage;

		private Transform _user;
		private Transform _thisTransform;
		protected override Color32 SkillIndicatorDisabled { get; } = new Color32(200, 10, 10, 150);

		protected override void Init()
		{
			Type = Behaviors.IndicatorType.Line;
			_thisTransform = transform;
		}

		protected override void ChangeColor(Color32 color) => lineImage.color = color;

		public void ShowIndicator(Skill skill, GameObject user)
		{
			_user = user.transform;
			lineImage.transform.localScale = new Vector3(1, skill.CastingRange, 1);
			lineImage.transform.localPosition = new Vector3(0, .01f, 0.5f * skill.CastingRange);
			gameObject.SetActive(true);
		}

		public void UpdateIndicator(Vector3 _)
		{
			_thisTransform.position = _user.position;
			if (Helper.RaycastIndicator(out var hit))
			{
				Helper.RotateToLocation(_thisTransform, hit.point);
			}
		}

		public void HideIndicator() => gameObject.SetActive(false);
	}
}