using UnityEngine;

namespace RPG.Core
{
	public class PersistentObjectSpawner : MonoBehaviour
	{
		[SerializeField] private GameObject persistentObjectPrefab;
		private static bool _hasSpawned;

		private void Awake()
		{
			if(_hasSpawned) return;
			SpawnPersistentObjects();
			_hasSpawned = true;
		}

		private void SpawnPersistentObjects()
		{
			var persistentObject = Instantiate(persistentObjectPrefab);
			DontDestroyOnLoad(persistentObject);
		}
	}
}