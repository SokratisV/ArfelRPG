using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
	public class CinematicControlRemover : MonoBehaviour
	{
		private GameObject _player;

		private void OnEnable()
		{
			var playableDirector = GetComponent<PlayableDirector>();
			playableDirector.played += DisableControl;
			playableDirector.stopped += EnableControl;
		}

		private void OnDisable()
		{
			var playableDirector = GetComponent<PlayableDirector>();
			playableDirector.played -= DisableControl;
			playableDirector.stopped -= EnableControl;
		}

		private void Awake() => _player = PlayerFinder.Player;

		private void DisableControl(PlayableDirector pd)
		{
			_player.GetComponent<ActionScheduler>().CancelCurrentAction();
			_player.GetComponent<PlayerController>().enabled = false;
		}

		private void EnableControl(PlayableDirector pd)
		{
			_player.GetComponent<PlayerController>().enabled = true;
		}
	}
}