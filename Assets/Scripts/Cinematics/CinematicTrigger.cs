using UnityEngine.Playables;
using UnityEngine;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        private bool hasBeenPlayed = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                print("Trigger by: " + other.name);
                GetComponent<PlayableDirector>().Play();
                hasBeenPlayed = true;
            }

        }
    }
}
