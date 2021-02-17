using System.Collections;
using RPG.Attributes;
using RPG.Control;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
	public class WeaponPickup : MonoBehaviour, IRaycastable
	{
		[SerializeField] private WeaponConfig weapon = null;
		[SerializeField] private float respawnTime = 5, healthToRestore = 0, pickupRange = 1f;

		private Collider _collider;
		private OutlineableComponent _outlineableComponent;

		private void Awake()
		{
			if(TryGetComponent(out Outline outline))
				_outlineableComponent = new OutlineableComponent(outline);
			else
			{
				outline = gameObject.AddComponent<Outline>();
				outline.enabled = false;
				_outlineableComponent = new OutlineableComponent(outline);
			}
			_collider = GetComponent<Collider>();
		}

		//TODO: Remove and fix to pickup on first click
		private void OnTriggerEnter(Collider other)
		{
			if(other.CompareTag("Player"))
			{
				Pickup(other.gameObject);
			}
		}

		private bool Pickup(GameObject subject)
		{
			if(!Helper.IsWithinDistance(transform, subject.transform, GetInteractionRange())) return false;
			if(weapon != null)
			{
				subject.GetComponent<Fighter>().EquipWeapon(weapon);
			}

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

		public bool HandleRaycast(PlayerController callingController)
		{
			if(Input.GetMouseButtonDown(0))
			{
				if(!Pickup(callingController.gameObject))
				{
					return false;
				}
			}

			return true;
		}

		public void ShowInteractivity() => _outlineableComponent.ShowOutline(this);

		public CursorType GetCursorType() => CursorType.Pickup;

		public float GetInteractionRange() => pickupRange;
	}
}