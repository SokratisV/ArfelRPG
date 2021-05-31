using RPG.Core;
using UnityEngine;
using RPG.Inventories;

namespace RPG.Control
{
	[RequireComponent(typeof(Collider))]
	[RequireComponent(typeof(Pickup))]
	[SelectionBase]
	public class ClickablePickup : MonoBehaviour, IRaycastable, ICollectable
	{
		private Pickup _pickup;
		private OutlineableComponent _outlineableComponent;

		private void Awake() => _pickup = GetComponent<Pickup>();

		private void Start() => _outlineableComponent = new OutlineableComponent(gameObject, _pickup.GetItem().Rarity.Color);

		public CursorType GetCursorType() => _pickup.CanBePickedUp()? CursorType.Pickup:CursorType.InventoryFull;

		public bool HandleRaycast(GameObject player)
		{
			var collector = player.GetComponent<Collector>();
			if(!collector.CanCollect(this)) return false;
			CheckPressedButtons(collector);
			return true;
		}

		private void CheckPressedButtons(Collector collector)
		{
			if(Input.GetKey(KeyCode.LeftControl))
			{
				if(Input.GetMouseButtonDown(0)) collector.QueueAction(new InteractableActionData(collector, transform));
			}
			else
			{
				if(Input.GetMouseButtonDown(0)) collector.StartCollectAction(this);
			}
		}

		public void ShowInteractivity() => _outlineableComponent.ShowOutline(this);

		public float InteractionDistance() => GlobalValues.InteractableRange;

		public Transform GetTransform() => transform.parent == null? transform:transform.parent;

		public void Collect() => _pickup.PickupItem();
	}
}