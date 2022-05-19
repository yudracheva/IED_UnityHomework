using System;
using System.Collections.Generic;
using GameStates.States;
using GameStates.States.Interfaces;
using Loots;
using SceneLoading;
using Services;
using Services.Factories.GameFactories;
using Services.Factories.Loot;
using Services.Loot;
using Services.Progress;
using Services.Shop;
using Services.StaticData;
using Services.UI.Factory;
using Services.UI.Windows;
using Services.UserSetting;
using Services.Waves;
using UserSettings;

namespace GameStates
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public GameStateMachine(
            ISceneLoader sceneLoader, 
            ref AllServices services, 
            ICoroutineRunner coroutineRunner,
            LootContainer lootContainer)
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, ref services, coroutineRunner, lootContainer),
                [typeof(LoadProgressState)] = new LoadProgressState(this, sceneLoader, services.Single<IPersistentProgressService>()),
                [typeof(GameLoopState)] = new GameLoopState(this, services.Single<IWaveServices>()),
                [typeof(LoadGameLevelState)] = new LoadGameLevelState(
                    services.Single<IPersistentProgressService>(),
                    sceneLoader,
                    this,
                    services.Single<IGameFactory>(),
                    services.Single<IUIFactory>(),
                    services.Single<IStaticDataService>(),
                    services.Single<IWaveServices>(),
                    services.Single<ILootService>(),
                    services.Single<ILootSpawner>(),
                    services.Single<IShopService>(),
                    services.Single<IUserSettingService>()),
                [typeof(MainMenuState)] = new MainMenuState(services.Single<IUIFactory>(),
                    services.Single<IWindowsService>(), sceneLoader, services.Single<IUserSettingService>()),
                [typeof(LoadGameLevel2State)] = new LoadGameLevel2State(
                    services.Single<IPersistentProgressService>(),
                    sceneLoader,
                    this,
                    services.Single<IGameFactory>(),
                    services.Single<IUIFactory>(),
                    services.Single<IStaticDataService>(),
                    services.Single<IWaveServices>(),
                    services.Single<ILootService>(),
                    services.Single<ILootSpawner>(),
                    services.Single<IShopService>(),
                    services.Single<IUserSettingService>()),
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            var state = ChangeState<TState>();
            state.Enter(payload);
        }

        public void Enter<TState, TPayload, TCallback>(TPayload payload, TCallback loadedCallback,
            TCallback curtainHideCallback) where TState : class, IPayloadedCallbackState<TPayload, TCallback>
        {
            var state = ChangeState<TState>();
            state.Enter(payload, loadedCallback, curtainHideCallback);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();

            var state = GetState<TState>();
            _activeState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState =>
            _states[typeof(TState)] as TState;
    }
}