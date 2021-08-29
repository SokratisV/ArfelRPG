using RPG.Core;
using UnityEngine;

namespace RPG.Skills
{
	public class LineIndicator : SkillIndicatorBase, ISkillIndicator
	{
		[SerializeField] private Transform lineTransform;
		private Transform _user;
		private Transform _thisTransform;

		private void Awake()
		{
			Type = Behaviors.IndicatorType.Line;
			_thisTransform = transform;
		}

		public void ShowIndicator(Skill skill, GameObject user)
		{
			_user = user.transform;
			lineTransform.localScale = new Vector3(1, skill.CastingRange, 1);
			lineTransform.localPosition = new Vector3(0, .01f, 0.5f * skill.CastingRange);
			gameObject.SetActive(true);
		}

		public void UpdateIndicator(Vector3 position)
		{
			_thisTransform.position = _user.position;
			Helper.RotateFromScreenSpaceDirection(_thisTransform);
		}

		public void HideIndicator() => gameObject.SetActive(false);
	}
}