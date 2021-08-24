using UnityEngine;

namespace RPG.Skills
{
	public class LineIndicator : SkillIndicatorBase, ISkillIndicator
	{
		[SerializeField] private Transform lineTransform;
		private Transform _user;

		public void ShowIndicator(Skill skill, GameObject user)
		{
			_user = user.transform;
			lineTransform.localScale = new Vector3(1, skill.CastingRange, 1);
			lineTransform.localPosition = new Vector3(0, .01f, 0.5f * skill.CastingRange);
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