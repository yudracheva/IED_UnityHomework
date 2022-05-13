using System;
using GameStates;
using GameStates.States;
using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Menus
{
    public class PauseMenuWindow : BaseWindow
    {
        [SerializeField] private Button menuButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private GameObject confirmWindow;
        [SerializeField] private GameObject generalWindow;
        [SerializeField] private Button yesButton;
        [SerializeField] private Button noButton;
        
        private IGameStateMachine _gameStateMachine;

        public void Construct(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine ?? throw  new ArgumentNullException(nameof(gameStateMachine));
        }

        protected override void Subscribe()
        {
            base.Subscribe();
            menuButton.onClick.AddListener(LoadMainMenu);
            closeButton.onClick.AddListener(Close);
            quitButton.onClick.AddListener(Quit);
        }

        public override void Open()
        {
            Time.timeScale = 0;
            base.Open();
        }
        
        public override void Close()
        {
            Time.timeScale = 1;
            base.Close();
        }
        
        protected override void Cleanup()
        {
            base.Cleanup();
            menuButton.onClick.RemoveListener(LoadMainMenu);
            closeButton.onClick.RemoveListener(Close);
            quitButton.onClick.RemoveListener(Quit);
        }
        
        private void Quit()
        {
            generalWindow.SetActive(false);
            confirmWindow.SetActive(true);
            yesButton.onClick.AddListener(ConfirmedQuit);
            noButton.onClick.AddListener(RejectedQuit);
        }

        private void RejectedQuit()
        {
            generalWindow.SetActive(true);
            confirmWindow.SetActive(false);
        }

        private void ConfirmedQuit()
        {
            Application.Quit();
        }

        private void LoadMainMenu()
        {
            _gameStateMachine.Enter<MainMenuState>();
        }
    }
}