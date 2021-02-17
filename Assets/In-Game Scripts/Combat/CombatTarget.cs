using RPG.Control;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public CursorType GetCursorType() => CursorType.Combat;

        public bool HandleRaycast(PlayerController callingController)
        {
            if(!callingController.GetComponent<Fighter>().CanAttack(gameObject)) return false;
            CheckPressedButtons(callingController);
            return true;
        }

        private void CheckPressedButtons(PlayerController callingController)
        {
            if(Input.GetKey(KeyCode.LeftControl))
            {
                if(Input.GetMouseButtonDown(0))
                {
                    callingController.GetComponent<Fighter>().QueueAttackAction(gameObject);
                }
            }
            else
            {
                if(Input.GetMouseButtonDown(0))
                {
                    callingController.GetComponent<Fighter>().StartAttackAction(gameObject);
                }
            }
        }

        public float GetInteractionRange() => 0f;

        private void ToggleOutline(bool toggle)
        {
            if(TryGetComponent(out Outline outline))
                outline.enabled = toggle;
        }

        private void OnMouseEnter() => ToggleOutline(true);

        private void OnMouseExit() => ToggleOutline(false);
    }
}