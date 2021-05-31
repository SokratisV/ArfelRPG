using RPG.Core;
using RPG.Inventories;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace RPG.Control
{
	public class Treasure : ItemDropper, IRaycastable, ISaveable, ICollectable
	{
		[SerializeField] private Transform unlockTransform;
		[SerializeField] private DropLibrary dropLibrary;
		[Min(1)] [SerializeField] private int treasureLevel;
		[SerializeField] private float scatterDistance = 1;
		
		private OutlineableComponent _outlineableComponent;
		private bool _isOpened = false;
		private const int Attempts = 30;
		
		#region Unity

		private void Awake() => _outlineableComponent = new OutlineableComponent(gameObject, GlobalValues.PickupColor);

		#endregion

		#region Public

		//Animation event
		public void DropLoot()
		{
			var item = dropLibrary.GetRandomDrops(treasureLevel);
			foreach(var dropped in item)
			{
				DropItem(dropped.Item, dropped.Number);
			}
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

		public CursorType GetCursorType() => CursorType.Pickup;

		public bool HandleRaycast(GameObject player)
		{
			if(_isOpened) return false;
			var collector = player.GetComponent<Collector>();
			if(!collector.CanCollect(this)) return false;
			CheckPressedButtons(collector);
			return true;
		}

		public void ShowInteractivity() => _outlineableComponent.ShowOutline(this);

		#endregion

		#region Private

		protected override Vector3 GetDropLocation()
		{
			for(var i = 0;i < Attempts;i++)
			{
				var randomPoint = transform.position + Random.onUnitSphere * scatterDistance;
				if(NavMesh.SamplePosition(randomPoint, out var hit, 0.1f, NavMesh.AllAreas))
				{
					return hit.position;
				}
			}

			return transform.position;
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

		#endregion
	}
}