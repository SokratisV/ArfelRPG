using UnityEngine;
using RPG.Inventories;

namespace RPG.Control
{
    [RequireComponent(typeof(SphereCollider))]
    [RequireComponent(typeof(Pickup))]
    public class ClickablePickup : MonoBehaviour, IRaycastable
    {
        private Pickup _pickup;

        private void Awake() => _pickup = GetComponent<Pickup>();

        public CursorType GetCursorType() => _pickup.CanBePickedUp()? CursorType.Pickup:CursorType.FullPickup;

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _pickup.PickupItem();
            }
            return true;
        }

        public void ShowInteractivity()
        {
            
        }

        public float GetInteractionRange()
        {
            return 0;
        }
    }
}