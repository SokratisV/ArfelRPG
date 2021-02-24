using UnityEngine.Playables;
using UnityEngine;

namespace RPG.Cinematics
{
	public class CinematicTrigger : MonoBehaviour
	{
		private bool _hasBeenPlayed;

		private void OnTriggerEnter(Collider other)
		{
			if(_hasBeenPlayed) return;
			if(other.gameObject.CompareTag("Player"))
			{
				GetComponent<PlayableDirector>().Play();
				_hasBeenPlayed = true;
			}
		}
	}
}