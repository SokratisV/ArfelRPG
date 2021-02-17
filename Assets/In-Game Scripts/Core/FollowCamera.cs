using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform target = default;

        private void LateUpdate()
        {
            transform.position = target.position;
        }
    }
}