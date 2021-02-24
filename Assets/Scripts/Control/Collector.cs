using System;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
	public class Collector : MonoBehaviour, IAction
	{
		public event Action OnActionComplete;
		private ICollectable _collectible;
		private Mover _mover;
		private ActionScheduler _actionScheduler;

		#region Unity

		private void Awake()
		{
			_mover = GetComponent<Mover>();
			_actionScheduler = GetComponent<ActionScheduler>();
		}

		private void Update()
		{
			if(_collectible == null) return;
			if(_mover.IsInRange(_collectible.GetTransform(), _collectible.InteractionDistance()))
			{
				CollectBehavior();
			}
			else
			{
				if (_mover.IsMoving) return;
				_mover.MoveWithoutAction(_collectible.GetTransform().position, withinDistance: _collectible.InteractionDistance());
			}
		}

		#endregion
		
		#region Public

		public void StartCollectAction(ICollectable collectable)
		{
			_collectible = collectable;
			_actionScheduler.StartAction(this);
		}

		public void CancelAction()
		{
			_collectible = null;
			_mover.CancelAction();
		}

		public void QueueCollectAction(GameObject obj) => _actionScheduler.EnqueueAction(new PickableActionData(this, obj.transform));

		public void CompleteAction()
		{
			OnActionComplete?.Invoke();
			_actionScheduler.CompleteAction();
		}

		public void ExecuteAction(IActionData data) => _collectible = ((PickableActionData)data).Treasure.GetComponent<ICollectable>();

		public bool CanCollect(ICollectable collectable) => collectable != null && _mover.CanMoveTo(collectable.GetTransform().position);

		#endregion

		#region Private

		private void CollectBehavior()
		{
			_mover.CancelAction();
			_collectible.Collect();
			_collectible = null;
			CompleteAction();
		}

		#endregion
	}
}