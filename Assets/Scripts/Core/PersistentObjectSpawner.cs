using UnityEngine;

namespace RPG.Core
{
	public class PersistentObjectSpawner : MonoBehaviour
	{
		[SerializeField] private GameObject persistentObjectPrefab;
		[SerializeField] private GameObject profilingTool;
		private static bool _hasSpawned;

		private void Awake()
		{
			if(_hasSpawned) return;
			SpawnPersistentObjects();
			_hasSpawned = true;
		}

		private void SpawnPersistentObjects()
		{
			DontDestroyOnLoad(Instantiate(persistentObjectPrefab));
			if (Debug.isDebugBuild)
			{
				DontDestroyOnLoad(Instantiate(profilingTool));
			}
		}
	}
}