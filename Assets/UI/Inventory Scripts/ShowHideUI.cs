using UnityEngine;

namespace RPG.UI
{
    public class ShowHideUI : MonoBehaviour
    {
        [SerializeField] private KeyCode toggleKey = KeyCode.Escape;
        [SerializeField] private GameObject uiContainer = null;

        private void Start() => uiContainer.SetActive(false);

        private void Update()
        {
            if (Input.GetKeyDown(toggleKey))
            {
                uiContainer.SetActive(!uiContainer.activeSelf);
            }
        }
    }
}