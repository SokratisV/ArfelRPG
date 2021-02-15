using System.Collections;
using RPG.Control;
using UnityEngine;
using UnityEngine.AI;
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

		[SerializeField] private int sceneToLoad = -1;
		[SerializeField] private Transform spawnPoint;
		[SerializeField] private DestinationIdentifier destination;
		[SerializeField] private float fadeInTime = 2f, fadeOutTime = 2f, fadeWaitTime = 2f;

		private void OnTriggerEnter(Collider other)
		{
			if(other.gameObject.CompareTag("Player"))
			{
				StartCoroutine(Transition());
			}
		}

		private IEnumerator Transition()
		{
			if(sceneToLoad < 0)
			{
				Debug.LogError("Scene to load is not set.");
				yield break;
			}

			DontDestroyOnLoad(gameObject);
			var fader = FindObjectOfType<Fader>();

			var wrapper = FindObjectOfType<SavingWrapper>();
			wrapper.Save();
			var player = GameObject.FindWithTag("Player");
			var playerController = player.GetComponent<PlayerController>();
			playerController.enabled = false;

			yield return fader.FadeOut(fadeOutTime);
			yield return SceneManager.LoadSceneAsync(sceneToLoad);

			var newPlayerController = player.GetComponent<PlayerController>();
			newPlayerController.enabled = false;

			wrapper.Load();

			var otherPortal = GetOtherPortal();
			UpdatePlayer(otherPortal, player.transform);

			wrapper.Save();

			yield return new WaitForSeconds(fadeWaitTime);
			fader.FadeIn(fadeInTime);

			newPlayerController.enabled = true;
			Destroy(gameObject);
		}

		private Portal GetOtherPortal()
		{
			foreach(var portal in FindObjectsOfType<Portal>())
			{
				if(portal == this) continue;
				if(portal.destination != destination) continue;
				return portal;
			}

			return null;
		}

		private void UpdatePlayer(Portal otherPortal, Transform player)
		{
			player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
			player.rotation = otherPortal.spawnPoint.rotation;
		}
	}
}