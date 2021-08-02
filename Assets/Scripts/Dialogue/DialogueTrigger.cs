using UnityEngine;
using UnityEngine.Events;

namespace RPG.Dialogue
{
	public class DialogueTrigger : MonoBehaviour
	{
		[SerializeField] private DialogueAction respondToAction;
		[SerializeField] private UnityEvent actionEvent;

		public void Trigger(DialogueAction action)
		{
			if(action == respondToAction) actionEvent.Invoke();
		}
	}
}