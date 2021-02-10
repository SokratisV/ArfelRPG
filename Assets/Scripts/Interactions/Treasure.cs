using RPG.Control;
using RPG.Saving;
using UnityEngine;

namespace RPG.Interactions
{
    public class Treasure : MonoBehaviour, IRaycastable, ISaveable
    {
        [SerializeField] private Transform[] dropLocations;
        [SerializeField] private RandomWeaponDrop lootTable;
        [SerializeField] private float interactionRange = 1f;

        private Transform _pickupManager;
        private bool _isOpened = false;

        private void Start()
        {
            _pickupManager = GameObject.FindGameObjectWithTag("PickupManager").transform;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if(!_isOpened)
            {
                if(!callingController.GetComponent<Collector>().CanCollect(gameObject)) return false;
                CheckPressedButtons(callingController);
                return true;
            }

            return false;
        }

        private void CheckPressedButtons(PlayerController callingController)
        {
            if(Input.GetKey(KeyCode.LeftControl))
            {
                if(Input.GetMouseButtonDown(0))
                {
                    callingController.GetComponent<Collector>().QueueCollectAction(gameObject);
                }
            }
            else
            {
                if(Input.GetMouseButtonDown(0))
                {
                    callingController.GetComponent<Collector>().Collect(this);
                }
            }
        }

        public void OpenTreasure()
        {
            var animator = GetComponentInChildren<Animator>();
            GetComponentInChildren<AudioSource>().Play();
            if(animator != null)
            {
                animator.enabled = true;
            }

            if(_isOpened) return;
            // Drop loot in animation event
        }

        public void DropLoot()
        {
            lootTable.GenerateLoot(dropLocations, _pickupManager);
            _isOpened = true;
            GetComponent<Collider>().enabled = false;
        }

        public void RestoreState(object state)
        {
            _isOpened = (bool)state;
            if(_isOpened)
            {
                GetComponentInChildren<Animator>().enabled = true;
            }
        }

        public object CaptureState()
        {
            return _isOpened;
        }

        public float GetInteractionRange()
        {
            return interactionRange;
        }

        private void ToggleOutline(bool toggle)
        {
            if (TryGetComponent(out Outline outline))
                outline.enabled = toggle;
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