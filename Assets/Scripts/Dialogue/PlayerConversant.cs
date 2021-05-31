using System;
using System.Collections.Generic;
using System.Linq;
using RPG.Core.Interfaces;
using RPG.Core;
using RPG.Movement;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RPG.Dialogue
{
	public class PlayerConversant : MonoBehaviour, IAction
	{
		public event Action OnActionComplete;
		public event Action<bool?> OnUpdated;
		private Dialogue _currentDialogue;
		private DialogueNode _currentNode = null;
		private IInteractable _interactTarget;
		private Mover _mover;
		private ActionScheduler _actionScheduler;
		private AIConversant _currentConversant = null;

		public bool IsChoosing {get;private set;}
		public bool HasNext => FilterOnCondition(_currentDialogue.GetAllChildren(_currentNode)).Any();
		public bool IsActive => _currentDialogue != null;
		public string Name => _currentConversant.Name;

		#region Unity

		private void Awake()
		{
			_mover = GetComponent<Mover>();
			_actionScheduler = GetComponent<ActionScheduler>();
		}

		private void Update()
		{
			if(_interactTarget == null) return;
			if(_mover.IsInRange(_interactTarget.GetTransform(), _interactTarget.InteractionDistance()))
			{
				InteractBehavior();
			}
			else
			{
				if(_mover.IsMoving) return;
				_mover.MoveWithoutAction(_interactTarget.GetTransform().position, withinDistance: _interactTarget.InteractionDistance());
			}
		}

		private void InteractBehavior()
		{
			_mover.CancelAction();
			_interactTarget.Interact();
			_interactTarget = null;
			CompleteAction();
		}

		#endregion

		#region Public

		public void StartDialogue(AIConversant newConversant, Dialogue newDialogue)
		{
			_currentConversant = newConversant;
			_currentDialogue = newDialogue;
			_currentNode = _currentDialogue.GetRootNode();
			TriggerEnterAction();
			OnUpdated?.Invoke(true);
		}

		public void Quit()
		{
			TriggerExitAction();
			_currentConversant = null;
			_currentDialogue = null;
			_currentNode = null;
			IsChoosing = false;
			OnUpdated?.Invoke(false);
		}

		public string GetText() => _currentNode == null? "":_currentNode.Text;

		public IEnumerable<DialogueNode> GetChoices() => FilterOnCondition(_currentDialogue.GetPlayerChildren(_currentNode));

		public void SelectChoice(DialogueNode chosenNode)
		{
			_currentNode = chosenNode;
			TriggerEnterAction();
			IsChoosing = false;
			Next();
		}

		public void Next()
		{
			if(FilterOnCondition(_currentDialogue.GetPlayerChildren(_currentNode)).Any())
			{
				IsChoosing = true;
				TriggerExitAction();
				OnUpdated?.Invoke(null);
				return;
			}

			var children = FilterOnCondition(_currentDialogue.GetAIChildren(_currentNode)).ToArray();
			TriggerExitAction();
			_currentNode = children[Random.Range(0, children.Length)];
			TriggerEnterAction();
			OnUpdated?.Invoke(null);
		}
		
		public void CancelAction()
		{
			_interactTarget = null;
			_mover.CancelAction();
		}

		public void CompleteAction()
		{
			OnActionComplete?.Invoke();
			_interactTarget = null;
			_actionScheduler.CompleteAction();
		}

		public void ExecuteQueuedAction(IActionData data) => _interactTarget = ((InteractableActionData)data).Target.GetComponent<IInteractable>();

		public void StartInteractAction(IInteractable interactable)
		{
			_interactTarget = interactable;
			_actionScheduler.StartAction(this);
		}

		public void QueueAction(IActionData data) => _actionScheduler.EnqueueAction(data);

		#endregion

		#region Private

		private IEnumerable<DialogueNode> FilterOnCondition(IEnumerable<DialogueNode> inputNode)
		{
			foreach(var node in inputNode)
			{
				if(node.CheckCondition(GetEvaluators())) yield return node;
			}
		}

		private IEnumerable<IPredicateEvaluator> GetEvaluators() => GetComponents<IPredicateEvaluator>();

		private void TriggerEnterAction()
		{
			if(_currentNode != null && _currentNode.OnEnterAction != DialogueAction.None)
			{
				TriggerAction(_currentNode.OnEnterAction);
			}
		}

		private void TriggerExitAction()
		{
			if(_currentNode != null && _currentNode.OnExitAction != DialogueAction.None)
			{
				TriggerAction(_currentNode.OnExitAction);
			}
		}

		private void TriggerAction(DialogueAction action)
		{
			if(action == DialogueAction.None) return;
			foreach(var trigger in _currentConversant.GetComponents<DialogueTrigger>())
			{
				trigger.Trigger(action);
			}
		}

		#endregion
	}
}