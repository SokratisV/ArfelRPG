using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 1f, maxLifeTime = 5f, lifeAfterImpact = 2f;
        [SerializeField] bool isHoming = false;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] GameObject[] destroyOnHit = null;
        Health target = null;
        GameObject instigator = null;
        float weaponDamage = 0f, arrowDamage = 1f;

        private void Start()
        {
            transform.LookAt(GetAimLocation());
        }
        void Update()
        {
            if (target == null) return;
            if (isHoming && !target.IsDead())
            {
                transform.LookAt(GetAimLocation());
            }
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            this.weaponDamage = damage;
            this.target = target;
            this.instigator = instigator;

            Destroy(gameObject, maxLifeTime);
        }

        private Vector3 GetAimLocation()
        {
            if (target.GetComponent<CapsuleCollider>() == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * (target.GetComponent<CapsuleCollider>().height / 2);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() == target)
            {
                if (target.IsDead()) return;
                target.TakeDamage(instigator, CalculateDamage());
                speed = 0;
                if (hitEffect != null)
                {
                    Instantiate(hitEffect, GetAimLocation(), transform.rotation);
                }
                foreach (var toDestroy in destroyOnHit)
                {
                    Destroy(toDestroy);
                }

                Destroy(gameObject, lifeAfterImpact);
            }
        }

        private float CalculateDamage()
        {
            return weaponDamage * arrowDamage;
        }
    }
}