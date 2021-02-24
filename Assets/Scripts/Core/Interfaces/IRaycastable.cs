using UnityEngine;

namespace RPG.Core
{
    public interface IRaycastable
    {
        CursorType GetCursorType();
        bool HandleRaycast(GameObject player);
        void ShowInteractivity();
        float InteractionDistance();
    }
}