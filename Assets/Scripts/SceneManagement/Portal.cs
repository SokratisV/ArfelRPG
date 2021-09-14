using System.Collections;
using System.Linq;
using RoboRyanTron.SceneReference;
using RPG.Core;
using RPG.Core.SystemEvents;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
	public class Portal : MonoBehaviour
	{
		// Connects portals across scenes with the same destination enum
		private enum DestinationIdentifier
		{
			A,
			B,
			C,
			D,
			E
		}

		[SerializeField] private SceneReference sceneToLoad;
		[SerializeField] private Transform spawnPoint;
		[SerializeField] private DestinationIdentifier destination;
		[SerializeField] private float fadeInTime = 2f, fadeOutTime = 2f, fadeWaitTime = 2f;
		[SerializeField] private BooleanEvent controlRemoveRequest;
		[SerializeField] private PlayerTeleportRequestEvent teleportRequest;

		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.CompareTag("Player"))
			{
				StartCoroutine(Transition());
			}
		}

		private IEnumerator Transition()
		{
			if (sceneToLoad == null)
			{
				Debug.LogError("Scene to load was empty.");
				yield return null;
			}

			DontDestroyOnLoad(gameObject);
			var fader = FindObjectOfType<Fader>();
			var wrapper = FindObjectOfType<SavingWrapper>();
			
			wrapper.Save();
			
			controlRemoveRequest.Raise(false);
			yield return fader.FadeOutRoutine(fadeOutTime);
			yield return SceneManager.LoadSceneAsync(sceneToLoad.SceneName);

			PlayerFinder.ResetPlayer();
			controlRemoveRequest.Raise(false);

			wrapper.Load();

			var otherPortal = GetOtherPortal();
			teleportRequest.Raise(new TeleportData(otherPortal.spawnPoint.position, otherPortal.spawnPoint.rotation));

			wrapper.Save();

			yield return new WaitForSeconds(fadeWaitTime);
			fader.FadeInRoutine(fadeInTime);

			controlRemoveRequest.Raise(true);
			Destroy(gameObject);
		}

		private Portal GetOtherPortal() => FindObjectsOfType<Portal>().Where(portal => portal != this).FirstOrDefault(portal => portal.destination == destination);
	}
}