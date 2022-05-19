using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using GeneralUserSettings = UserSettings.GeneralUserSettings;

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

        public event Action<GeneralUserSettings> Changed;

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
            Changed?.Invoke(settings);
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