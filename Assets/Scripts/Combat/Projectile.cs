using RPG.Core;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] bool isHoming = false;
    Health target = null;
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

    public void SetTarget(Health target, float damage)
    {
        this.weaponDamage = damage;
        this.target = target;
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
            target.TakeDamage(CalculateDamage());
            Destroy(gameObject);
        }
    }

    private float CalculateDamage()
    {
        return weaponDamage * arrowDamage;
    }
}
