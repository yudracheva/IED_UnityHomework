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

        private IGameStateMachine gameStateMachine;

        public void Construct(IGameStateMachine gameStateMachine)
        {
            this.gameStateMachine = gameStateMachine;
        }

        protected override void Subscribe()
        {
            base.Subscribe();
            menuButton.onClick.AddListener(LoadMainMenu);
            closeButton.onClick.AddListener(Close);
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
        }
        
        private void LoadMainMenu()
        {
            gameStateMachine.Enter<MainMenuState>();
        }
    }
}