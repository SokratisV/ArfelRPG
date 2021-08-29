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

		protected override void Init()
		{
			Type = Behaviors.IndicatorType.Line;
			_thisTransform = transform;
			lineImage.material = InstancedMaterial;
		}

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