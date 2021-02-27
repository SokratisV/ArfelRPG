using RPG.Core;
using UnityEngine;

namespace RPG.Quests
{
	public class QuestGiver : MonoBehaviour
	{
		[SerializeField] private Quest quest;

		//UnityEvent
		public void GiveQuest()
		{
			var questList = PlayerFinder.Player.GetComponent<QuestList>();
			questList.AddQuest(quest);
		}
	}
}