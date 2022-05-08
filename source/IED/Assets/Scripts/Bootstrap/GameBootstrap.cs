using GameStates;
using GameStates.States;
using Loots;
using SceneLoading;
using Services;
using UnityEngine;

namespace Bootstrap
{
    public class GameBootstrap : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private LoadingCurtain curtainPrefab;
        [SerializeField] private LootContainer lootContainerPrefab;

        private AllServices _allServices;
        private Game _game;

        private void Awake()
        {
            _allServices = new AllServices();
            _game = new Game(
                coroutineRunner: this,
                curtain: Instantiate(curtainPrefab),
                services: ref _allServices,
                lootContainer: Instantiate(lootContainerPrefab, transform));
            _game.StateMachine.Enter<BootstrapState>();
            DontDestroyOnLoad(this);
        }

        private void OnDestroy()
        {
            _allServices.Cleanup();
        }
    }
}