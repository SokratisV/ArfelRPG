using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Interactions
{
    public class Collector : MonoBehaviour, IAction
    {
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
            if(_collectible == null)
            {
                return;
            }

            if(!IsInRange(_collectible.transform))
            {
                _mover.MoveTo(_collectible.transform.position, 1f);
            }
            else
            {
                _mover.Cancel();
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
            _actionScheduler.StartAction(this);
            _collectible = collectible;
        }

        public void Cancel()
        {
            _collectible = null;
        }

        public bool CanCollect(GameObject collectible)
        {
            if(collectible == null) return false;
            // TODO check for range like in fighter
            return true;
        }

        private bool IsInRange(Transform targetTransform)
        {
            return Vector3.Distance(transform.position, targetTransform.position) < targetTransform.GetComponent<Treasure>().GetInteractionRange();
        }

        public void QueueCollectAction(GameObject obj)
        {
            _actionScheduler.EnqueueAction(new PickableActionData(this, obj.transform));
        }

        public void Complete()
        {
            _actionScheduler.CompleteAction();
        }

        public void ExecuteAction(IActionData data)
        {
            _collectible = ((PickableActionData)data).Treasure.GetComponent<Treasure>();
        }
    }
}