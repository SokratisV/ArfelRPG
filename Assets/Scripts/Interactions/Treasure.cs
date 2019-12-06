using RPG.Control;
using RPG.Saving;
using UnityEngine;

namespace RPG.Interactions
{
    public class Treasure : MonoBehaviour, IRaycastable, ISaveable
    {
        [SerializeField] Transform[] dropLocations;
        [SerializeField] RandomWeaponDrop lootTable;
        [SerializeField] float interactionRange = 1f;

        Transform pickupManager;
        bool isOpened = false;

        private void Start()
        {
            pickupManager = GameObject.FindGameObjectWithTag("PickupManager").transform;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
        public bool HandleRaycast(PlayerController callingController)
        {
            if (!isOpened)
            {
                if (!callingController.GetComponent<Collector>().CanCollect(gameObject)) return false;
                if (Input.GetMouseButtonDown(0))
                {
                    callingController.GetComponent<Collector>().Collect(this);
                }
                return true;
            }
            return false;
        }

        public void OpenTreasure()
        {
            // lootTable.GenerateLoot(dropLocations);
            Animator animator = GetComponentInChildren<Animator>();
            if (animator != null)
            {
                animator.enabled = true;
            }
            if (isOpened) return;
            lootTable.GenerateLoot(dropLocations, pickupManager);
            isOpened = true;
            GetComponent<Collider>().enabled = false;
        }

        public void RestoreState(object state)
        {
            isOpened = (bool)state;
            if (isOpened == true)
            {
                GetComponentInChildren<Animator>().enabled = true;
            }
        }
        public object CaptureState()
        {
            return isOpened;
        }
        public float GetInteractionRange()
        {
            return interactionRange;
        }
    }
}
