using System;
using UserSettings;

namespace Services.UserSetting
{
    public interface IUserSettingService : IService
    {
        event Action<GeneralUserSettings> Changed;
        
        GeneralUserSettings GetUserSettings();

        void SetUserSettings(GeneralUserSettings settings);

        void UpdateGameSettings(GeneralUserSettings settings = null);
    }
}