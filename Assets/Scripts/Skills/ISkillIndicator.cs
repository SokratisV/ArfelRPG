using UnityEngine;

namespace RPG.Skills
{
	public interface ISkillIndicator
	{
		void ShowIndicator(float radius);
		void UpdateIndicator(Vector3 position);
		void HideIndicator();
	}
}