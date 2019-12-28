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
                CheckPressedButtons(callingController);
                return true;
            }
            return false;
        }
        private void CheckPressedButtons(PlayerController callingController)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    callingController.GetComponent<Collector>().QueueCollectAction(gameObject);
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    callingController.GetComponent<Collector>().Collect(this);
                }
            }
        }
        public void OpenTreasure()
        {
            Animator animator = GetComponentInChildren<Animator>();
            GetComponentInChildren<AudioSource>().Play();
            if (animator != null)
            {
                animator.enabled = true;
            }
            if (isOpened) return;
            // Drop loot in animation event
        }
        public void DropLoot()
        {
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
        private void ToggleOutline(bool toggle)
        {
            Outline outline;
            if (outline = GetComponentInChildren<Outline>())
            {
                outline.enabled = toggle;
            }
        }
        private void OnMouseEnter()
        {
            ToggleOutline(true);
        }
        private void OnMouseExit()
        {
            ToggleOutline(false);
        }
    }
}
