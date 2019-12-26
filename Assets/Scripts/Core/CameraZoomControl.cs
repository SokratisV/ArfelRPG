using Cinemachine;
using UnityEngine;

namespace RPG.Core
{
    public class CameraZoomControl : MonoBehaviour
    {
        [SerializeField] float zoomSpeed = 3f;

        float zoom = 18f;
        CinemachineFramingTransposer cm_Cam;

        private void Awake()
        {
            cm_Cam = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
            zoom = cm_Cam.m_CameraDistance;
        }
        private void LateUpdate()
        {
            float mouseScroll = Input.mouseScrollDelta.y;
            // if (!Mathf.Approximately(mouseScroll, 0))
            // {
            //     zoom += mouseScroll;
            //     cm_Cam.m_CameraDistance = Mathf.Clamp(zoom, 5f, 20f);
            // }
            if (mouseScroll < 0 || mouseScroll > 0)
            {
                zoom -= mouseScroll * zoomSpeed;
                cm_Cam.m_CameraDistance = Mathf.Clamp(zoom, 5f, 20f);
            }
        }
    }
}
