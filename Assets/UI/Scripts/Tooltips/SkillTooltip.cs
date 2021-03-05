using RPG.Skills;
using TMPro;
using UnityEngine;

namespace RPG.UI.Skills
{
	public class SkillTooltip : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI titleText = null;
		[SerializeField] private TextMeshProUGUI bodyText = null;

		public void Setup(Skill skill)
		{
			titleText.text = skill.DisplayName;
			bodyText.text = skill.Description;
		}
	}
}