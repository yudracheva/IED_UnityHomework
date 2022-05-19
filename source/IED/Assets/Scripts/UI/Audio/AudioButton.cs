using System;
using Services.UserSetting;
using UnityEngine;
using UserSettings;

namespace UI.Audio
{
    public class AudioButton : MonoBehaviour
    {
        private AudioSource _audioSource;
        private IUserSettingService _userSettingService;
        private bool _actionAdded;
        
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

        private void UpdateVolume(float value)
        {
            _audioSource.volume = value;
        }

        public void Construct(IUserSettingService userSettingService)
        {
            _userSettingService = userSettingService;
            _userSettingService.Changed += UserSettingServiceOnChanged;
            _actionAdded = true;
            
            UpdateVolume(userSettingService.GetUserSettings().ActionsVolume);
        }

        public void OnDestroy()
        {
            if (_actionAdded)
                _userSettingService.Changed -= UserSettingServiceOnChanged;                
        }

        private void UserSettingServiceOnChanged(GeneralUserSettings obj)
        {
            UpdateVolume(obj.ActionsVolume);
        }
    }   
}
