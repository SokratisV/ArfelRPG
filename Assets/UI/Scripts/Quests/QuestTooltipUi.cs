using RPG.Quests;
using TMPro;
using UnityEngine;

namespace RPG.UI.Quests
{
	public class QuestTooltipUi : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI title;
		[SerializeField] private Transform objectiveParent;
		[SerializeField] private GameObject objectivePrefab, objectiveIncompletePrefab;
		[SerializeField] private TextMeshProUGUI rewardText;

		public void Setup(QuestStatus status)
		{
			var quest = status.Quest;
			title.SetText(quest.Title);
			foreach(Transform child in objectiveParent)
			{
				Destroy(child.gameObject);
			}

			foreach(var objective in quest.GetObjectives())
			{
				var prefab = status.IsObjectiveComplete(objective.reference)? objectivePrefab:objectiveIncompletePrefab;
				var obj = Instantiate(prefab, objectiveParent);
				var text = obj.GetComponentInChildren<TextMeshProUGUI>();
				text.SetText(objective.description);
			}

			rewardText.SetText(GetRewardText(quest));
		}

		private string GetRewardText(Quest quest)
		{
			var text = string.Empty;
			foreach(var reward in quest.GetRewards())
			{
				if(!string.IsNullOrEmpty(text)) text += ", ";
				text += $"{reward.item.DisplayName} (x{reward.number})";
			}

			if(string.IsNullOrEmpty(text)) text = "No Reward";
			text += ".";
			return text;
		}
	}
}