using UnityEngine;

public class OnHitParticles : MonoBehaviour
{
    [SerializeField] GameObject onHitParticles;
    [SerializeField] float lifeAfterImpact;

    public void SpawnParticles()
    {
        print(transform.name);
        if (onHitParticles != null)
        {
            Destroy(Instantiate(onHitParticles, transform.position, Random.rotation).gameObject, lifeAfterImpact);
        }
    }
}
