using System;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Interactions
{
	public class Collector : MonoBehaviour, IAction
	{
		public event Action OnActionComplete;
		private Treasure _collectible;
		private Mover _mover;
		private ActionScheduler _actionScheduler;

		private void Awake()
		{
			_mover = GetComponent<Mover>();
			_actionScheduler = GetComponent<ActionScheduler>();
		}

		private void Update()
		{
			if (_collectible == null) return;
			if(IsInRange(_collectible.transform))
			{
				CollectBehavior();
			}
			else
			{
				_mover.MoveWithoutAction(_collectible.transform.position);
			}
		}

		private void CollectBehavior()
		{
			transform.LookAt(_collectible.transform);
			if(IsInRange(_collectible.transform))
			{
				_mover.CancelAction();
				Collect();
			}
		}

		private void Collect()
		{
			_collectible.OpenTreasure();
			_collectible = null;
			CompleteAction();
		}

		public void StartCollectAction(Treasure collectible)
		{
			_collectible = collectible;
			_actionScheduler.StartAction(this);
		}

		public void CancelAction()
		{
			_collectible = null;
			_mover.CancelAction();
		}

		public bool CanCollect(GameObject collectible)
		{
			if(collectible == null) return false;
			return!_mover.CanMoveTo(collectible.transform.position);
		}

		private bool IsInRange(Transform targetTransform) => Helper.IsWithinDistance(transform, targetTransform, _collectible.GetInteractionRange());

		public void QueueCollectAction(GameObject obj) => _actionScheduler.EnqueueAction(new PickableActionData(this, obj.transform));

		public void CompleteAction()
		{
			OnActionComplete?.Invoke();
			_actionScheduler.CompleteAction();
		}

		public void ExecuteAction(IActionData data) =>_collectible = ((PickableActionData)data).Treasure.GetComponent<Treasure>();
	}
}