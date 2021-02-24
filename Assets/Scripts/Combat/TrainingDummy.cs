namespace RPG.Combat
{
	using UnityEngine;

	public class TrainingDummy : MonoBehaviour
	{
		[SerializeField] private GameObject trainingDummyPrefab;
		[SerializeField] [Range(-1, 400)] private float aggroDistance;

		public void Respawn()
		{
			Instantiate(trainingDummyPrefab, transform.position, transform.rotation);
			Destroy(gameObject, 0.1f);
		}
	}
}