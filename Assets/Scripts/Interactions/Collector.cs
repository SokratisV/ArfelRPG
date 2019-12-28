using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Interactions
{
    public class Collector : MonoBehaviour, IAction
    {
        Treasure collectible;

        private void Update()
        {
            if (collectible == null) { return; }
            if (!IsInRange(collectible.transform))
            {
                GetComponent<Mover>().MoveTo(collectible.transform.position, 1f);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                CollectBehavior();
            }
        }
        private void CollectBehavior()
        {
            transform.LookAt(collectible.transform);
            collectible.OpenTreasure();
            collectible = null;
            Complete();
        }
        public void Collect(Treasure collectible)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            this.collectible = collectible;
        }
        public void Cancel()
        {
            collectible = null;
        }
        public bool CanCollect(GameObject collectible)
        {
            if (collectible == null) return false;
            // TODO check for range like in fighter
            return true;
        }
        private bool IsInRange(Transform targetTransform)
        {
            return (Vector3.Distance(transform.position, targetTransform.position) < targetTransform.GetComponent<Treasure>().GetInteractionRange());
        }
        public void QueueCollectAction(GameObject gameObject)
        {
            GetComponent<ActionScheduler>().EnqueueAction(new PickableActionData(this, gameObject.transform));
        }
        public void Complete()
        {
            GetComponent<ActionScheduler>().CompleteAction();
        }
        public void ExecuteAction(ActionData data)
        {
            collectible = ((PickableActionData)data).treasure.GetComponent<Treasure>();
        }
    }
}