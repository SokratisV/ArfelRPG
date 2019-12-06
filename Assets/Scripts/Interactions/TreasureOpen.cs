using RPG.Control;
using RPG.Saving;
using UnityEngine;

namespace RPG.Interactions
{
    public class TreasureOpen : MonoBehaviour, IRaycastable, ISaveable
    {
        [SerializeField] Transform[] dropLocations;
        [SerializeField] RandomWeaponDrop lootTable;
        [SerializeField] float interactionRange = 1f;
        bool isOpened = false;

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!isOpened)
                {
                    if (!OpenTreasure(callingController.transform))
                    {
                        return false;
                    }
                }
                else
                {
                    //Play animation
                }
            }
            return true;
        }

        public bool OpenTreasure(Transform callingController)
        {
            if (Vector3.Distance(transform.position, callingController.transform.position) > InteractableRange()) return false;
            isOpened = true;
            lootTable.GenerateLoot(dropLocations);
            Animator animator = GetComponentInChildren<Animator>();
            if (animator != null)
            {
                animator.enabled = true;
            }
            return true;
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

        public float InteractableRange()
        {
            return interactionRange;
        }
    }
}
