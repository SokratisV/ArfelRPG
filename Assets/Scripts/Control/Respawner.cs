using System.Collections;
using RPG.Core;
using RPG.Core.SystemEvents;
using UnityEngine;
using UnityEngine.AI;

namespace Control
{
	public class Respawner : MonoBehaviour
	{
		[SerializeField] private FloatEvent faderEvent;
		[SerializeField] private Transform respawnLocation;
		[SerializeField] [Min(0)] private float respawnDelay = 3f;
		[SerializeField] [Min(0)] private float fadeTime = 3f;

		public Transform RespawnLocation
		{
			get => respawnLocation;
			set => respawnLocation = value;
		}

		//Convenience method
		public static void SetRespawnLocation(Transform newLocation)
		{
			PlayerFinder.Player.GetComponent<Respawner>().RespawnLocation = newLocation;
		}

		[ContextMenu("Test respawn")]
		private void Respawn() => StartCoroutine(RespawnRoutine());

		private IEnumerator RespawnRoutine()
		{
			if (respawnLocation == null)
			{
				Debug.LogError("Couldn't find the Respawn Location.");
				yield break;
			}

			yield return new WaitForSeconds(respawnDelay);
			faderEvent.Raise(fadeTime);
			yield return new WaitForSeconds(fadeTime);
			GetComponent<NavMeshAgent>().Warp(respawnLocation.position);
			faderEvent.Raise(fadeTime);
			yield return new WaitForSeconds(fadeTime);
		}
	}
}