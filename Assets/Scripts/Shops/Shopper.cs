using System;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Shops
{
	public class Shopper : MonoBehaviour, IAction
	{
		public event Action ActiveShopChange;
		public event Action OnActionComplete;

		private Shop _activeShop = null;
		private Mover _mover = null;
		private ActionScheduler _actionScheduler;
		private IInteractable _interactTarget;

		#region Unity

		private void Awake()
		{
			_actionScheduler = GetComponent<ActionScheduler>();
			_mover = GetComponent<Mover>();
		}

		private void Update()
		{
			if (_interactTarget == null) return;
			if (_mover.IsInRange(_interactTarget.GetTransform(), _interactTarget.InteractionDistance()))
			{
				InteractBehavior();
			}
			else
			{
				if (_mover.IsMoving) return;
				_mover.MoveWithoutAction(_interactTarget.GetTransform().position, withinDistance: _interactTarget.InteractionDistance());
			}
		}

		#endregion

		#region Public

		public static Shopper GetPlayerShopper()
		{
			var player = PlayerFinder.Player;
			return player.GetComponent<Shopper>();
		}

		public void SetActiveShop(Shop shop)
		{
			_activeShop = shop;
			ActiveShopChange?.Invoke();
		}

		public Shop GetActiveShop() => _activeShop;

		public void CancelAction()
		{
		}

		public void CompleteAction()
		{
		}

		public void ExecuteQueuedAction(IActionData data)
		{
		}

		public void QueueInteractAction(GameObject obj)
		{
			_actionScheduler.EnqueueAction(new InteractableActionData(this, obj.transform));
		}

		public void StartInteractAction(IInteractable interactable)
		{
			_interactTarget = interactable;
			_actionScheduler.StartAction(this);
		}

		#endregion

		#region Private

		private void InteractBehavior()
		{
			_mover.CancelAction();
			_interactTarget.Interact();
			_interactTarget = null;
			CompleteAction();
		}

		#endregion
	}
}