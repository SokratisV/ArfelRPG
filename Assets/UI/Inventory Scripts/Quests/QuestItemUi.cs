using RPG.Quests;
using TMPro;
using UnityEngine;

namespace RPG.UI.Quests
{
	public class QuestItemUi : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI title, progress;
		public QuestStatus Status {get;private set;} = null;

		public void Setup(QuestStatus status)
		{
			Status = status;
			title.SetText(status.Quest.Title);
			progress.SetText($"{status.CompletedCount}/{status.Quest.ObjectiveCount}");
		}
	}
}