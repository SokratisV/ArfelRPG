using System;
using System.Collections.Generic;
using System.Linq;
using RPG.Core;
using RPG.Movement;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RPG.Dialogue
{
	public class PlayerConversant : MonoBehaviour, IAction
	{
		public event Action OnActionComplete;
		public event Action OnUpdated;
		private Dialogue _currentDialogue;
		private DialogueNode _currentNode = null;
		private IInteractable _interactTarget;
		private Mover _mover;
		private ActionScheduler _actionScheduler;

		public bool IsChoosing {get;private set;}
		public bool HasNext => _currentNode.Children.Count > 0;
		public bool IsActive => _currentDialogue != null;

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
				if (_mover.IsMoving) return;
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

		public void StartDialogue(Dialogue newDialogue)
		{
			_currentDialogue = newDialogue;
			_currentNode = _currentDialogue.GetRootNode();
			OnUpdated?.Invoke();
		}

		public void Quit()
		{
			_currentDialogue = null;
			_currentNode = null;
			IsChoosing = false;
			OnUpdated?.Invoke();
		}

		public string GetText() => _currentNode == null? "":_currentNode.Text;

		public IEnumerable<DialogueNode> GetChoices() => _currentDialogue.GetPlayerChildren(_currentNode);

		public void SelectChoice(DialogueNode chosenNode)
		{
			_currentNode = chosenNode;
			IsChoosing = false;
			Next();
		}

		public void Next()
		{
			if(_currentDialogue.GetPlayerChildren(_currentNode).Any())
			{
				IsChoosing = true;
				OnUpdated?.Invoke();
				return;
			}

			var children = _currentDialogue.GetAIChildren(_currentNode).ToArray();
			_currentNode = children[Random.Range(0, children.Length)];
			OnUpdated?.Invoke();
		}

		public bool CanInteract(IInteractable interactable) => interactable != null && _mover.CanMoveTo(interactable.GetTransform().position);

		#endregion
		
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

		public void ExecuteAction(IActionData data) => _interactTarget = ((InteractableActionData)data).Target.GetComponent<IInteractable>();

		public void StartInteractAction(IInteractable interactable)
		{
			_interactTarget = interactable;
			_actionScheduler.StartAction(this);
		}

		public void QueueInteractAction(GameObject obj) => _actionScheduler.EnqueueAction(new InteractableActionData(this, obj.transform));
	}
}