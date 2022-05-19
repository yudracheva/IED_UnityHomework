using System;
using System.IO;
using Audio;
using Newtonsoft.Json;
using UI.Audio;
using UnityEngine;
using GeneralUserSettings = UserSettings.GeneralUserSettings;
using Object = UnityEngine.Object;

namespace Services.UserSetting
{
    public class UserSettingService : IUserSettingService
    {
        private GeneralUserSettings _currentUserSettings;
        private readonly string _settingPath = Path.Combine(Application.persistentDataPath, "UserSettings.json");

        public UserSettingService()
        {
            if (!TryLoadSettings())
            {
                _currentUserSettings = new GeneralUserSettings() 
                {
                    ActionsVolume = 1,
                    MusicVolume = 1
                };
            }
        }

        public GeneralUserSettings GetUserSettings()
        {
            return (GeneralUserSettings) _currentUserSettings.Clone();
        }

        public void SetUserSettings(GeneralUserSettings settings)
        {
            _currentUserSettings = settings;
            SaveSettings();
        }

        public void UpdateGameSettings(GeneralUserSettings customSettings = null)
        {
            var settings = customSettings ?? GetUserSettings();
            var audioButton = Object.FindObjectOfType<AudioButton>();
            if (audioButton != null)
            {   
                audioButton.UpdateVolume(settings.ActionsVolume);
            }
            
            var audioPrefab = Object.FindObjectOfType<AudioBackground>();
            if (audioPrefab != null)
            {
                audioPrefab.UpdateSettings(settings);
            }
        }
        
        private bool TryLoadSettings()
        {
            if (!File.Exists(_settingPath))
                return false;

            var content = File.ReadAllText(_settingPath);
            try
            {
                _currentUserSettings = JsonConvert.DeserializeObject<GeneralUserSettings>(content);
            }
            catch (Exception e)
            {
                Debug.Log($"Failed loading of user settings. {e.Message}");
                return false;
            }

            return true;
        }

        private void SaveSettings()
        {
            var jsonSettings = JsonConvert.SerializeObject(_currentUserSettings);
            File.WriteAllText(_settingPath, jsonSettings);
            UpdateGameSettings();
        }
    }
}