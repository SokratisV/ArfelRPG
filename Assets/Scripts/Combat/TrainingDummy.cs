namespace RPG.Combat
{
    using UnityEngine;

    public class TrainingDummy : MonoBehaviour
    {
        [SerializeField] GameObject trainingDummyPrefab;

        public void Respawn()
        {
            Instantiate(trainingDummyPrefab, transform.position, transform.rotation);
            Destroy(gameObject, 0.1f);
        }
    }
}
