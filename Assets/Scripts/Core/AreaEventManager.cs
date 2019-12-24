using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class AreaEventManager : MonoBehaviour
    {
        public static event Action<Areas> onEnterArea = delegate { };
        [SerializeField] Areas currentArea = Areas.None;
        Stack<Areas> areasCurrentlyIn;

        private void Awake()
        {
            areasCurrentlyIn = new Stack<Areas>();
        }

        private void Start()
        {
            OnEnterArea(currentArea);
        }

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

        private void OnEnterArea(Areas area)
        {
            onEnterArea(area);
        }
    }
}