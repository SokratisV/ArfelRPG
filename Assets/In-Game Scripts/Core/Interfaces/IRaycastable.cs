using UnityEngine;

namespace RPG.Core
{
    public interface IRaycastable
    {
        CursorType GetCursorType();
        bool HandleRaycast(GameObject caller);
        void ShowInteractivity();
        float InteractionDistance();
    }
}