using RPG.Core;
using UnityEngine;
using RPG.Inventories;

namespace RPG.Control
{
	[RequireComponent(typeof(Collider))]
	[RequireComponent(typeof(Pickup))]
	public class ClickablePickup : MonoBehaviour, IRaycastable, ICollectable
	{
		private Pickup _pickup;
		private OutlineableComponent _outlineableComponent;

		private void Awake()
		{
			_pickup = GetComponent<Pickup>();
			_outlineableComponent = new OutlineableComponent(gameObject);
		}

		public CursorType GetCursorType() => _pickup.CanBePickedUp()? CursorType.Pickup:CursorType.InventoryFull;

		public bool HandleRaycast(PlayerController callingController)
		{
			var collector = callingController.GetComponent<Collector>();
			if(!collector.CanCollect(this)) return false;
			CheckPressedButtons(collector);
			return true;
		}

		private void CheckPressedButtons(Collector collector)
		{
			if(Input.GetKey(KeyCode.LeftControl))
			{
				if(Input.GetMouseButtonDown(0)) collector.QueueCollectAction(gameObject);
			}
			else
			{
				if(Input.GetMouseButtonDown(0)) collector.StartCollectAction(this);
			}
		}

		public void ShowInteractivity() => _outlineableComponent.ShowOutline(this);

		public float InteractionDistance() => GlobalValues.InteractableRange;

		public Transform GetTransform() => transform.parent;

		public void Collect() => _pickup.PickupItem();
	}
}