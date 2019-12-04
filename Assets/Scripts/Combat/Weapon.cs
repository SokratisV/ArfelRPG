using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] UnityEvent onHit;
        // Animation event
        public void OnHit()
        {
            onHit.Invoke();
        }
    }
}
