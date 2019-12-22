namespace Combat
{
    using UnityEngine;

    public class TrainingDummy : MonoBehaviour
    {
        [SerializeField] GameObject trainingDummyPrefab;

        public void Respawn()
        {
            Instantiate(trainingDummyPrefab);
            Destroy(gameObject);
        }
    }
}
