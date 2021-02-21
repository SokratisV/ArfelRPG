using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
	public interface IActionData
	{
		IAction GetAction();
	}

	public struct MoverActionData : IActionData
	{
		private IAction _action;
		public Vector3 Destination;
		public float Speed;
		public float StopDistance;

		public MoverActionData(IAction action, Vector3 destination, float speed, float stopWithinDistance)
		{
			_action = action;
			Destination = destination;
			Speed = speed;
			StopDistance = stopWithinDistance;
		}

		public IAction GetAction() => _action;
	}

	public struct FighterActionData : IActionData
	{
		private IAction _action;
		public GameObject Target;

		public FighterActionData(IAction action, GameObject target)
		{
			_action = action;
			Target = target;
		}

		public IAction GetAction() => _action;
	}

	public struct PickableActionData : IActionData
	{
		private IAction _action;
		public Transform Treasure;

		public PickableActionData(IAction action, Transform treasure)
		{
			_action = action;
			Treasure = treasure;
		}

		public IAction GetAction() => _action;
	}
	
	public struct InteractableActionData : IActionData
	{
		private IAction _action;
		public Transform Target;

		public InteractableActionData(IAction action, Transform target)
		{
			_action = action;
			Target = target;
		}

		public IAction GetAction() => _action;
	}

	public class ActionScheduler : MonoBehaviour
	{
		private IAction _currentAction;
		private Queue<IActionData> _actionsQueue = new Queue<IActionData>();

		public void StartAction(IAction action)
		{
			ClearActionsQueue();
			if(_currentAction == action) return;
			_currentAction?.CancelAction();
			_currentAction = action;
		}

		private void ClearActionsQueue() => _actionsQueue.Clear();

		public void CancelCurrentAction() => StartAction(null);

		public void EnqueueAction(IActionData actionData)
		{
			if(_actionsQueue.Count == 0 && _currentAction == null)
			{
				if(_currentAction != null) _actionsQueue.Enqueue(actionData);
				else StartNextAction(actionData.GetAction(), actionData);
			}
			else
			{
				_actionsQueue.Enqueue(actionData);
			}
		}

		private void DequeueAction(out IAction action, out IActionData data)
		{
			action = null;
			data = null;
			if(_actionsQueue.Count == 0) return;

			data = _actionsQueue.Dequeue();
			action = data.GetAction();
		}

		public void CompleteAction()
		{
			_currentAction = null;
			if(_actionsQueue.Count == 0) return;
			DequeueAction(out var action, out var data);
			StartNextAction(action, data);
		}

		private void StartNextAction(IAction action, IActionData data)
		{
			_currentAction = action;
			action.ExecuteAction(data);
		}
	}
}