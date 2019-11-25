using UnityEngine.Playables;
using UnityEngine;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        private bool hasBeenPlayed = false;

        private void OnTriggerEnter(Collider other)
        {
            if (hasBeenPlayed) return;
            if (other.gameObject.CompareTag("Player"))
            {
                GetComponent<PlayableDirector>().Play();
                hasBeenPlayed = true;
            }
        }
    }
}
