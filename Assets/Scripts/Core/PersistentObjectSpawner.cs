using UnityEngine;

namespace RPG.Core
{
	public class PersistentObjectSpawner : MonoBehaviour
	{
		[SerializeField] private GameObject persistentObjectPrefab;
		private static bool HasSpawned;

		private void Awake()
		{
			if(HasSpawned) return;
			SpawnPersistentObjects();
			HasSpawned = true;
		}

		private void SpawnPersistentObjects()
		{
			var persistentObject = Instantiate(persistentObjectPrefab);
			DontDestroyOnLoad(persistentObject);
		}
	}
}