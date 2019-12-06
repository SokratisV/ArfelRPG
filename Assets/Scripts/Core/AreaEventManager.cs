using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Core
{
    public class AreaEventManager : MonoBehaviour
    {
        [SerializeField] EnterArea onEnterArea;
        [SerializeField] Areas currentArea = Areas.None;
        Stack<Areas> areasCurrentlyIn;

        public void EnterNewArea(Areas area)
        {
            areasCurrentlyIn.Push(currentArea);
            currentArea = area;
            OnEnterArea(area);
        }

        public void ExitArea(Areas area)
        {
            if (areasCurrentlyIn.Peek() == area)
            {
                areasCurrentlyIn.Pop();
                return;
            }
            Areas previousArea = areasCurrentlyIn.Pop();
            currentArea = previousArea;
            OnEnterArea(previousArea);
        }

        private void Awake()
        {
            areasCurrentlyIn = new Stack<Areas>();
        }

        private void Start()
        {
            OnEnterArea(currentArea);
        }

        [System.Serializable]
        public class EnterArea : UnityEvent<Areas>
        {

        }

        private void OnEnterArea(Areas area)
        {
            onEnterArea.Invoke(area);
        }
    }
}