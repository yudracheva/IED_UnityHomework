using Services.UserSetting;
using UnityEngine;
using UserSettings;

namespace Audio
{
    public class AudioBackground : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
    
        private IUserSettingService _userSettingService;
        private bool _actionAdded;
        
        public void Construct(IUserSettingService userSettingService, AudioClip audioClip)
        {
            _userSettingService = userSettingService;
            _userSettingService.Changed += UserSettingServiceOnChanged;
            _actionAdded = true;
            
            audioSource.clip = audioClip;
            audioSource.Play();
            
            UpdateSettings(userSettingService.GetUserSettings());
        }

        public void OnDestroy()
        {
            if (_actionAdded)
                _userSettingService.Changed -= UserSettingServiceOnChanged;
        }
        
        private void UserSettingServiceOnChanged(GeneralUserSettings obj)
        {
            UpdateSettings(obj);
        }

        private void UpdateSettings(GeneralUserSettings generalUserSettings)
        {
            audioSource.volume = generalUserSettings.MusicVolume;
        }
    }
}
