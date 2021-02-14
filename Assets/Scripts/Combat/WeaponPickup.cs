using System.Collections;
using RPG.Attributes;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] private WeaponConfig weapon = null;
        [SerializeField] private float respawnTime = 5, healthToRestore = 0, pickupRange = 1f;

        private Collider _collider;

        private void Awake()
        {
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
            if(Vector3.Distance(transform.position, subject.transform.position) > GetInteractionRange()) return false;
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

        private void ToggleOutline(bool toggle)
        {
            if(TryGetComponent(out Outline outline))
                outline.enabled = toggle;
        }

        private void OnMouseEnter()
        {
            ToggleOutline(true);
        }

        private void OnMouseExit()
        {
            ToggleOutline(false);
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }

        public float GetInteractionRange()
        {
            return pickupRange;
        }
    }
}