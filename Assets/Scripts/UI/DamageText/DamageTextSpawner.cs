using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] float additionalHeight = .5f;
        [SerializeField] DamageText damageTextPrefab = null;

        public void Spawn(float damageAmount)
        {
            DamageText instance = Instantiate(damageTextPrefab, transform);
            Vector3 vector3 = instance.transform.position + Vector3.up * (GetComponentInParent<CapsuleCollider>().height + additionalHeight);
            instance.transform.position = vector3;
            instance.SetValue(damageAmount);
        }
    }
}
