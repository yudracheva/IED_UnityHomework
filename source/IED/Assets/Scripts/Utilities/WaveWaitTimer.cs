using System;
using System.Collections;
using UnityEngine;

namespace Utilities
{
    public class WaveWaitTimer : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public event Action TimeOut;

        public void StartWait(float time)
        {
            StartCoroutine(Wait(time));
        }

        private IEnumerator Wait(float time)
        {
            yield return new WaitForSeconds(time);
            TimeOut?.Invoke();
        }
    }
}