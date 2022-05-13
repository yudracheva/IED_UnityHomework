using UnityEngine;
using UserSettings;

namespace Audio
{
    public class AudioBackground : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
    
        public void UpdateSettings(GeneralUserSettings generalUserSettings)
        {
            audioSource.volume = generalUserSettings.MusicVolume;
        }
    }
}
