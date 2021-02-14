using System;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Interactions
{
	public class Collector : MonoBehaviour, IAction
	{
		public event Action OnComplete;
		private bool _isInRange;
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
			if(_isInRange && _collectible != null)
			{
				CollectBehavior();
			}
		}

		private void CollectBehavior()
		{
			transform.LookAt(_collectible.transform);
			_collectible.OpenTreasure();
			_collectible = null;
			Complete();
		}

		public void Collect(Treasure collectible)
		{
			_isInRange = false;
			_collectible = collectible;

			void Action()
			{
				StartCollectAction();
				_mover.OnComplete -= Action;
			}

			_mover.StartMoveAction(collectible.transform.position, withinDistance: collectible.GetInteractionRange()).OnComplete += Action;
		}
		
		private void StartCollectAction()
		{
			_actionScheduler.StartAction(this);
			_isInRange = true;
		}

		public void Cancel() => _collectible = null;

		public bool CanCollect(GameObject collectible)
		{
			if(collectible == null) return false;
			// TODO check for range like in fighter
			return true;
		}

		private bool IsInRange(Transform targetTransform) => Helper.IsWithinDistance(transform, targetTransform, targetTransform.GetComponent<Treasure>().GetInteractionRange());

		public void QueueCollectAction(GameObject obj) => _actionScheduler.EnqueueAction(new PickableActionData(this, obj.transform));

		public void Complete()
		{
			OnComplete?.Invoke();
			_actionScheduler.CompleteAction();
		}

		public void ExecuteAction(IActionData data) => _collectible = ((PickableActionData)data).Treasure.GetComponent<Treasure>();
	}
}