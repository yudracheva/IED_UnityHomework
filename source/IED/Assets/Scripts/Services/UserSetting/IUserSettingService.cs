using UserSettings;

namespace Services.UserSetting
{
    public interface IUserSettingService : IService
    {
        GeneralUserSettings GetUserSettings();

        void SetUserSettings(GeneralUserSettings settings);

        void UpdateGameSettings(GeneralUserSettings settings = null);
    }
}