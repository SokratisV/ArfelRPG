using RPG.Control;

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

		public void AggroEverything()
		{
			if(aggroDistance < 0) aggroDistance = 5000;
			var hits = Physics.SphereCastAll(transform.position, aggroDistance, Vector3.up, 0);
			foreach(var hit in hits)
			{
				if(hit.transform.TryGetComponent(out AIController ai))
					ai.Aggrevate();
			}
		}
	}
}