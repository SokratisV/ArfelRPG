using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Transform target;

    private void LateUpdate()
    {
        transform.position = target.position;
    }
}
