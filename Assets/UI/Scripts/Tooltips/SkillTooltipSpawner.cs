using RPG.UI.Tooltips;
using UnityEngine;

namespace RPG.UI.Skills
{
	public class SkillTooltipSpawner : TooltipSpawner
	{
		public override void UpdateTooltip(GameObject tooltip)
		{
			var itemTooltip = tooltip.GetComponent<SkillTooltip>();
			if(!itemTooltip) return;
			var item = GetComponent<ISkillHolder>().GetSkill();
			itemTooltip.Setup(item);
		}

		public override bool CanCreateTooltip()
		{
			var skill = GetComponent<ISkillHolder>().GetSkill();
			return skill;
		}
	}
}