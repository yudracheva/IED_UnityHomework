using GameStates;
using GameStates.States;
using Services.Score;
using TMPro;
using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Menus
{
    public class DeathMenuWindow : BaseWindow
    {
        [SerializeField] private TMP_InputField nicknameInputField;
        [SerializeField] private TextMeshProUGUI leaderboardTipText;
        [SerializeField] private Button saveScoreButton;
        [SerializeField] private Button menuButton;
        
        private IGameStateMachine _gameStateMachine;
        private IScoreService _scoreService;

        public void Construct(IGameStateMachine gameStateMachine, IScoreService scoreService)
        {
            _gameStateMachine = gameStateMachine;
            _scoreService = scoreService;
        }

        public override async void Open()
        {
            if (await _scoreService.IsPLayerInTop())
                ChangeSaveScoreElementsActive(true);

            Time.timeScale = 0;
            base.Open();
        }
        
        public override void Close()
        {
            Time.timeScale = 1;
            base.Close();
        }

        protected override void Subscribe()
        {
            base.Subscribe();
            menuButton.onClick.AddListener(LoadMenu);
            saveScoreButton.onClick.AddListener(SaveScore);
        }

        protected override void Cleanup()
        {
            base.Cleanup();
            menuButton.onClick.RemoveListener(LoadMenu);
            saveScoreButton.onClick.RemoveListener(SaveScore);
        }

        private void SaveScore()
        {
            if (nicknameInputField.text.Length > 0)
                _scoreService.SavePlayerInLeaderboard(nicknameInputField.text);

            ChangeSaveScoreElementsActive(false);
        }

        private void ChangeSaveScoreElementsActive(bool isActive)
        {
            nicknameInputField.gameObject.SetActive(isActive);
            saveScoreButton.gameObject.SetActive(isActive);
            leaderboardTipText.gameObject.SetActive(isActive);
        }

        private void LoadMenu()
        {
            _gameStateMachine.Enter<MainMenuState>();
        }
    }
}