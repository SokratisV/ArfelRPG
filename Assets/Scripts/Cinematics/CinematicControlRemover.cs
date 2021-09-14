using RPG.Core.SystemEvents;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
	public class CinematicControlRemover : MonoBehaviour
	{
		[SerializeField] private BooleanEvent controlRemoveRequest;

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

		private void DisableControl(PlayableDirector pd) => controlRemoveRequest.Raise(false);
		private void EnableControl(PlayableDirector pd) => controlRemoveRequest.Raise(true);
	}
}