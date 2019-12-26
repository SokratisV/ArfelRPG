using UnityEngine;

namespace RPG.Core
{
    public class OnAreaTrigger : MonoBehaviour
    {
        [SerializeField] Areas area;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            GetComponentInParent<AreaEventManager>().EnterNewArea(area);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            GetComponentInParent<AreaEventManager>().ExitArea(area);
        }
    }
}