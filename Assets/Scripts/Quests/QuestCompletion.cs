using UnityEngine;

namespace RPG.Quests
{
	public class QuestCompletion : MonoBehaviour
	{
		[SerializeField] private QuestData quest;
		[SerializeField] private QuestEvent questEvent;

		//Unity Event
		public void CompleteObjective()
		{
			questEvent.Raise(quest);
		}
	}
}