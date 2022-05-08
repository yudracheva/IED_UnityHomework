using System;
using Audio;
using ConstantsValue;
using GameStates.States.Interfaces;
using SceneLoading;
using Services.UI.Factory;
using Services.UI.Windows;
using Services.UserSetting;
using UnityEngine;

namespace GameStates.States
{
    public class MainMenuState : IState
    {
        private readonly IUIFactory _uiFactory;
        private readonly IWindowsService _windowsService;
        private readonly ISceneLoader _sceneLoader;
        private readonly IUserSettingService _userSettingService;
        
        public MainMenuState(IUIFactory uiFactory, IWindowsService windowsService, ISceneLoader sceneLoader, IUserSettingService userSettingService)
        {
            _uiFactory = uiFactory ?? throw new ArgumentNullException(nameof(uiFactory));
            _windowsService = windowsService ?? throw new ArgumentNullException(nameof(windowsService));
            _sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
            _userSettingService = userSettingService ?? throw new ArgumentNullException(nameof(userSettingService));
        }

        public void Enter()
        {
            _sceneLoader.Load(Constants.MainMenuScene, OnLoaded);
        }

        private void OnLoaded()
        {
            _uiFactory.CreateUIRoot();
            InitAudio();
            _windowsService.Open(WindowId.MainMenu);
        }

        public void Exit()
        {
        }
        
        private void InitAudio()
        {
            _userSettingService.UpdateGameSettings();
        }
    }
}