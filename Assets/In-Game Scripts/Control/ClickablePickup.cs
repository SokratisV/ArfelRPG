using RPG.Core;
using UnityEngine;
using RPG.Inventories;

namespace RPG.Control
{
    [RequireComponent(typeof(SphereCollider))]
    [RequireComponent(typeof(Pickup))]
    public class ClickablePickup : MonoBehaviour, IRaycastable
    {
        private Pickup _pickup;
        private OutlineableComponent _outlineableComponent;
        
        private void Awake()
        {
            _pickup = GetComponent<Pickup>();
            _outlineableComponent = new OutlineableComponent(gameObject);
        }

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
            _outlineableComponent.ShowOutline(this);
        }

        public float GetInteractionRange()
        {
            return 0;
        }
    }
}