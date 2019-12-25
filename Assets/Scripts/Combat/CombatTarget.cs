using RPG.Control;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (!callingController.GetComponent<Fighter>().CanAttack(gameObject)) return false;

            if (Input.GetMouseButton(0))
            {
                callingController.GetComponent<Fighter>().Attack(gameObject);
            }
            return true;
        }

        public float GetInteractionRange()
        {
            return 0f;
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