using RPG.UI.Tooltips;
using UnityEngine;

namespace RPG.UI.Quests
{
	public class QuestTooltipSpawner : TooltipSpawner
	{
		private QuestItemUi _questItemUi;

		private void Awake() => _questItemUi = GetComponent<QuestItemUi>();

		public override void UpdateTooltip(GameObject tooltip) => tooltip.GetComponent<QuestTooltipUi>().Setup(_questItemUi.Status);

		public override bool CanCreateTooltip() => true;
	}
}