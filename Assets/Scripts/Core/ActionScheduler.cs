using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public interface ActionData
    {
        IAction GetAction();
    }
    public struct MoverActionData : ActionData
    {
        IAction action;
        public Vector3 destination;
        public float speed;

        public MoverActionData(IAction action, Vector3 destination, float speed)
        {
            this.action = action;
            this.destination = destination;
            this.speed = speed;
        }
        public IAction GetAction()
        {
            return action;
        }
    }
    public struct FighterActionData : ActionData
    {
        IAction action;
        public GameObject target;

        public FighterActionData(IAction action, GameObject target)
        {
            this.action = action;
            this.target = target;
        }
        public IAction GetAction()
        {
            return action;
        }
    }
    public struct PickableActionData : ActionData
    {
        IAction action;
        public Transform treasure;

        public PickableActionData(IAction action, Transform treasure)
        {
            this.action = action;
            this.treasure = treasure;
        }
        public IAction GetAction()
        {
            return action;
        }
    }
    public class ActionScheduler : MonoBehaviour
    {
        IAction currentAction;
        Queue<ActionData> actionsQueue = new Queue<ActionData>();

        public void StartAction(IAction action)
        {
            ClearActionsQueue();
            if (currentAction == action) return;
            if (currentAction != null)
            {
                currentAction.Cancel();
            }
            print("started");
            currentAction = action;
        }
        private void ClearActionsQueue()
        {
            if (actionsQueue.Count > 0)
            {
                actionsQueue.Clear();
            }
        }
        public void CancelCurrentAction()
        {
            StartAction(null);
        }
        public void QueueAction(ActionData moveData)
        {
            print("Enqueued");
            if (actionsQueue.Count == 0)
            {
                print("Start immediately");
                StartNextAction(moveData.GetAction(), moveData);
            }
            actionsQueue.Enqueue(moveData);
        }

        private void DequeueAction(out IAction action, out ActionData data)
        {
            action = null;
            data = null;
            if (actionsQueue.Count == 0) { return; }

            data = actionsQueue.Dequeue();
            action = data.GetAction();
        }

        public void CompleteAction()
        {
            if (actionsQueue.Count == 0) return;

            IAction action;
            ActionData data;
            DequeueAction(out action, out data);
            StartNextAction(action, data);
        }

        private void StartNextAction(IAction action, ActionData data)
        {
            currentAction = action;
            action.ExecuteAction(data);
        }
    }
}