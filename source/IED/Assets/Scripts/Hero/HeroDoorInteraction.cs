using System;
using UnityEngine;

namespace Hero
{
    public class HeroDoorInteraction : MonoBehaviour
    {
        public event Action<bool, string> ShowUIMessage;

        private DoorInteraction _doorInteraction;
        
        public void HideMessage()
        {
            ShowUIMessage?.Invoke(false, null);
        }

        public void ShowMessage(string text)
        {
            ShowUIMessage?.Invoke(true, text);
        }
    }
}
