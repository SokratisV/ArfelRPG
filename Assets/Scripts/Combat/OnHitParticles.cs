namespace RPG.Combat
{
    using UnityEngine;

    public class OnHitParticles : MonoBehaviour
    {
        [SerializeField] private GameObject onHitParticles;
        [SerializeField] private float lifeAfterImpact;

        public void SpawnParticles()
        {
            if(onHitParticles != null)
            {
                Destroy(Instantiate(onHitParticles, transform.position, Random.rotation).gameObject, lifeAfterImpact);
            }
        }
    }
}