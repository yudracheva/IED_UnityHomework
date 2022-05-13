using UnityEngine;

namespace UI.Audio
{
    public class AudioButton : MonoBehaviour
    {
        private AudioSource _audioSource;

        protected void Awake()
        {
            _audioSource = GetComponentInChildren<AudioSource>();
        }

        public void OnClick()
        {
            _audioSource.enabled = true;
            _audioSource.Stop();
            _audioSource.Play();
        }

        public void UpdateVolume(float value)
        {
            _audioSource.volume = value;
        }
    }   
}
