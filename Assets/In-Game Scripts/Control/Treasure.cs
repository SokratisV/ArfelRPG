using RPG.Core;
using RPG.Saving;
using UnityEngine;

namespace RPG.Control
{
	public class Treasure : MonoBehaviour, IRaycastable, ISaveable, ICollectable
	{
		[SerializeField] private Transform unlockTransform;

		private OutlineableComponent _outlineableComponent;
		private bool _isOpened = false;

		private void Awake() => _outlineableComponent = new OutlineableComponent(gameObject);

		public CursorType GetCursorType() => CursorType.Pickup;

		public bool HandleRaycast(GameObject caller)
		{
			if(_isOpened) return false;
			var collector = caller.GetComponent<Collector>();
			if(!collector.CanCollect(this)) return false;
			CheckPressedButtons(collector);
			return true;

		}

		public void ShowInteractivity() => _outlineableComponent.ShowOutline(this);

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

		private void OpenTreasure()
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

		public Transform GetTransform() => unlockTransform;

		public void Collect() => OpenTreasure();

		public float InteractionDistance() => GlobalValues.InteractableRange;
	}
}