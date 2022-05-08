using System;
using System.Globalization;
using Services.UserSetting;
using TMPro;
using UI.Base;
using UnityEngine;
using UnityEngine.UI;
using UserSettings;

namespace UI.Windows.Settings
{
    public class UserSettingsWindow: BaseWindow
    {
        [SerializeField] private Button menuButton;
        [SerializeField] private Button saveButton;
        [SerializeField] private Scrollbar musicScrollBar;
        [SerializeField] private Scrollbar effectsScrollBar;
        
        private IUserSettingService _userSettingService;
        private GeneralUserSettings _generalUserSettings;
        
        private TextMeshProUGUI _musicText;
        private TextMeshProUGUI _effectsText;
        
        private bool _isChanged;
        
        public void Construct(IUserSettingService userSettingService)
        {
            _userSettingService = userSettingService ?? throw new ArgumentNullException(nameof(userSettingService));

            _effectsText = effectsScrollBar.GetComponentInChildren<TextMeshProUGUI>();
            _musicText = musicScrollBar.GetComponentInChildren<TextMeshProUGUI>();
            LoadUserSettings();
        }

        protected override void Subscribe()
        {
            base.Subscribe();
            menuButton.onClick.AddListener(Close);
            musicScrollBar.onValueChanged.AddListener(OnMusicValueChanged);
            effectsScrollBar.onValueChanged.AddListener(OnEffectValueChanged);
            saveButton.onClick.AddListener(SetSettings);
        }

        protected override void Cleanup()
        {
            base.Cleanup();
            menuButton.onClick.RemoveListener(Close);
            musicScrollBar.onValueChanged.RemoveListener(OnMusicValueChanged);
            effectsScrollBar.onValueChanged.RemoveListener(OnEffectValueChanged);
            saveButton.onClick.RemoveListener(SetSettings);
        }

        public override void Close()
        {
            _userSettingService.UpdateGameSettings();
            Destroy(gameObject);
        }
        
        private void LoadUserSettings()
        {
            _generalUserSettings = _userSettingService.GetUserSettings();
            UpdateView();
        }

        private void SetSettings()
        {
            _userSettingService.SetUserSettings(_generalUserSettings);
            _isChanged = false;
            saveButton.GetComponent<Button>().interactable = false;
        }

        private void UpdateView()
        {
            musicScrollBar.value = _generalUserSettings.MusicVolume;
            _musicText.text = GetScrollBarValueText(musicScrollBar.value); 

            effectsScrollBar.value = _generalUserSettings.ActionsVolume;
            _effectsText.text = GetScrollBarValueText(effectsScrollBar.value);
        }

        private void OnMusicValueChanged(float value)
        {
            if (Math.Abs(_generalUserSettings.MusicVolume - value) < 0.001)
                return;
            
            _generalUserSettings.MusicVolume = value;
            SetTextMesh(_musicText, value);
        }
        
        private void OnEffectValueChanged(float value)
        {
            if (Math.Abs(_generalUserSettings.ActionsVolume - value) < 0.001)
                return;
            
            _generalUserSettings.ActionsVolume = value;
            SetTextMesh(_effectsText, value);
        }
        
        private void SetTextMesh(TextMeshProUGUI textMesh, float newValue)
        {
            textMesh.text = GetScrollBarValueText(newValue);
            if (!_isChanged)
                saveButton.GetComponent<Button>().interactable = true;
            
            _isChanged = true;
            _userSettingService.UpdateGameSettings(_generalUserSettings);
        }

        private static string GetScrollBarValueText(float currentValue)
        {
            return Math.Round(currentValue * 100, 0).ToString(CultureInfo.InvariantCulture);
        }
    }
}