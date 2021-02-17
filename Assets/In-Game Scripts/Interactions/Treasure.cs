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

		private void Start() => _pickupManager = GameObject.FindGameObjectWithTag("PickupManager").transform;

		public CursorType GetCursorType() => CursorType.Pickup;

		public bool HandleRaycast(PlayerController callingController)
		{
			if(!_isOpened)
			{
				var collector = callingController.GetComponent<Collector>();
				if(!collector.CanCollect(gameObject)) return false;
				CheckPressedButtons(collector);
				return true;
			}

			return false;
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

		public void OpenTreasure()
		{
			var animator = GetComponentInChildren<Animator>();
			GetComponentInChildren<AudioSource>().Play();
			if(animator != null)
			{
				animator.enabled = true;
			}

			// Drops loot in animation event
		}

		//Animation event
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

		public object CaptureState() => _isOpened;

		public float GetInteractionRange() => interactionRange;

		private void ToggleOutline(bool toggle)
		{
			if(TryGetComponent(out Outline outline))
				outline.enabled = toggle;
		}

		private void OnMouseEnter() => ToggleOutline(true);

		private void OnMouseExit() => ToggleOutline(false);
	}
}