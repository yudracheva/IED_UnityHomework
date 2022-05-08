using System;
using System.Collections;
using System.Collections.Generic;
using ConstantsValue;
using GameStates;
using GameStates.States;
using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Menus
{
    public class MainMenuWindow : BaseWindow
    {
        [SerializeField] private Button leaderboardButton;
        [SerializeField] private Button playButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button quitButton;

        private IGameStateMachine _gameStateMachine;

        public void Construct(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine ?? throw  new ArgumentNullException(nameof(gameStateMachine));
        }

        protected override void Subscribe()
        {
            base.Subscribe();
            playButton.onClick.AddListener(StartGame);
            quitButton.onClick.AddListener(Exit);
            leaderboardButton.onClick.AddListener(Close);
            settingsButton.onClick.AddListener(Close);
        }

        protected override void Cleanup()
        {
            base.Cleanup();
            playButton.onClick.RemoveListener(StartGame);
            leaderboardButton.onClick.RemoveListener(Close);
            settingsButton.onClick.RemoveListener(Close);
            quitButton.onClick.RemoveListener(Exit);
        }
        
        private static void Exit()
        {
            Application.Quit();
        }

        private void StartGame()
        {
            _gameStateMachine.Enter<LoadGameLevelState, string>(Constants.GameScene);
        }
    }
}