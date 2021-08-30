using RPG.Skills.Behaviors;
using UnityEngine;

namespace RPG.Skills
{
	public interface ISkillIndicator
	{
		IndicatorType IndicatorType();
		void ShowIndicator(Skill skill, GameObject user);
		void UpdateIndicator(Vector3 position);
		void HideIndicator();
		void ToggleColorState(bool toggle);
		void ChangeIndicatorAlpha(byte customAlpha);
	}
}