namespace RPG.Combat
{
    using UnityEngine;

    public class OnHitParticles : MonoBehaviour
    {
        [SerializeField] GameObject onHitParticles;
        [SerializeField] float lifeAfterImpact;

        public void SpawnParticles()
        {
            if (onHitParticles != null)
            {
                Destroy(Instantiate(onHitParticles, transform.position, Random.rotation).gameObject, lifeAfterImpact);
            }
        }
    }
}

