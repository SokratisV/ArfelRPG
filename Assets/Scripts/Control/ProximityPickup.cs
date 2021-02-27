using System.Collections;
using RPG.Attributes;
using RPG.Control;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
	public class ProximityPickup : MonoBehaviour, IRaycastable
	{
		[SerializeField] private float respawnTime = 5, healthToRestore = 0;

		private Collider _collider;
		private OutlineableComponent _outlineableComponent;

		private void Awake()
		{
			_outlineableComponent = new OutlineableComponent(gameObject, GlobalValues.PickupColor);
			_collider = GetComponent<Collider>();
		}

		private void OnTriggerEnter(Collider other)
		{
			if(other.CompareTag("Player"))
			{
				Pickup(other.gameObject);
			}
		}

		private bool Pickup(GameObject subject)
		{
			if(!Helper.IsWithinDistance(transform, subject.transform, InteractionDistance())) return false;
			if(healthToRestore > 0)
			{
				subject.GetComponent<Health>().Heal(healthToRestore);
			}

			if(respawnTime > 0)
			{
				StartCoroutine(HideForSeconds(respawnTime));
			}
			else
			{
				Destroy(gameObject);
			}

			return true;
		}

		private IEnumerator HideForSeconds(float seconds)
		{
			TogglePickup(false);
			yield return new WaitForSeconds(seconds);
			TogglePickup(true);
		}

		private void TogglePickup(bool shouldShow)
		{
			_collider.enabled = shouldShow;
			foreach(Transform children in transform)
			{
				children.gameObject.SetActive(shouldShow);
			}
		}

		public bool HandleRaycast(GameObject player)
		{
			if(Input.GetMouseButtonDown(0))
			{
				if(!Pickup(player))
				{
					return false;
				}
			}

			return true;
		}

		public void ShowInteractivity() => _outlineableComponent.ShowOutline(this);

		public CursorType GetCursorType() => CursorType.Pickup;

		public float InteractionDistance() => 0;
	}
}