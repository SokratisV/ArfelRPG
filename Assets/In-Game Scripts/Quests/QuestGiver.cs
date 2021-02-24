using UnityEngine;

namespace RPG.Quests
{
	public class QuestGiver : MonoBehaviour
	{
		[SerializeField] private Quest quest;

		//UnityEvent
		public void GiveQuest()
		{
			var questList = GameObject.FindWithTag("Player").GetComponent<QuestList>();
			questList.AddQuest(quest);
		}
	}
}