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

		private void Awake()
		{
			_mover = GetComponent<Mover>();
			_actionScheduler = GetComponent<ActionScheduler>();
		}

		private void Update()
		{
			if(_collectible == null) return;
			if(IsInRange(_collectible.GetTransform()))
			{
				CollectBehavior();
			}
			else
			{
				_mover.MoveWithoutAction(_collectible.GetTransform().position);
			}
		}

		private void CollectBehavior()
		{
			transform.LookAt(_collectible.GetTransform());
			if(IsInRange(_collectible.GetTransform()))
			{
				_mover.CancelAction();
				Collect();
			}
		}

		private void Collect()
		{
			_collectible.Collect();
			_collectible = null;
			CompleteAction();
		}

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

		public bool CanCollect(ICollectable collectable) => collectable != null && _mover.CanMoveTo(collectable.GetTransform().position);

		private bool IsInRange(Transform targetTransform) => Helper.IsWithinDistance(transform, targetTransform, _collectible.InteractionDistance());

		public void QueueCollectAction(GameObject obj) => _actionScheduler.EnqueueAction(new PickableActionData(this, obj.transform));

		public void CompleteAction()
		{
			OnActionComplete?.Invoke();
			_actionScheduler.CompleteAction();
		}

		public void ExecuteAction(IActionData data) => _collectible = ((PickableActionData)data).Treasure.GetComponent<ICollectable>();
	}
}