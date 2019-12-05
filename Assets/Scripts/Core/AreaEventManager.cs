using System;
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

        public void ExitPreviousArea()
        {
            if (areasCurrentlyIn.Peek() == Areas.None)
            {
                Areas area = areasCurrentlyIn.Pop();
                currentArea = area;
                OnEnterArea(area);
            }
            else
            {
                areasCurrentlyIn.Pop();
            }

        }

        private void Start()
        {
            areasCurrentlyIn = new Stack<Areas>();
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