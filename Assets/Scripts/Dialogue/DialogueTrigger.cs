using RPG.Quests;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Dialogue
{
	public class DialogueTrigger : MonoBehaviour
	{
		[SerializeField] private DialogueAction respondToAction;
		[SerializeField] private UnityEvent actionEvent;
		[SerializeField] private QuestEvent questEvent;
		[SerializeField] private QuestData quest;
		
		public void Trigger(DialogueAction action)
		{
			if(action == respondToAction) actionEvent.Invoke();
			questEvent.Raise(quest);
		}
	}
}