using RPG.UI.Tooltips;
using UnityEngine;

namespace RPG.UI.Skills
{
	public class SkillTooltipSpawner : TooltipSpawner
	{
		private ISkillHolder _skillHolder;

		private void Awake() => _skillHolder = GetComponent<ISkillHolder>();

		public override void UpdateTooltip(GameObject tooltip)
		{
			var itemTooltip = tooltip.GetComponent<SkillTooltip>();
			if (!itemTooltip) return;
			itemTooltip.Setup(_skillHolder.GetSkill());
		}

		public override bool CanCreateTooltip() => _skillHolder?.GetSkill();
	}
}