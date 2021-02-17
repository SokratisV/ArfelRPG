using UnityEngine;

namespace RPG.Core
{
    public class DestroyAfterEffect : MonoBehaviour
    {
        [SerializeField] private GameObject targetToDestroy = null;

        private void Update()
        {
            if(!GetComponent<ParticleSystem>().IsAlive())
            {
                Destroy(targetToDestroy != null? targetToDestroy:gameObject);
            }
        }
    }
}