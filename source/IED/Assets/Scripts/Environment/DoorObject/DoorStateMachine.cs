using System;
using Animations;
using ConstantsValue;
using Hero;
using Services.Progress;
using StateMachines.Environment;
using UnityEngine;

namespace Environment.DoorObject
{
    public class DoorStateMachine : BaseEntityStateMachine
    {
        public DoorIdleState IdleState { get; private set; }
        public DoorOpeningState OpeningState { get; private set; }
        public DoorTryingToOpenState TryingToOpenState { get; private set; }

        private SimpleAnimator _animator;
        private IPersistentProgressService _persistentProgressService;
        private bool _isOpen;
        
        public void Construct(IPersistentProgressService persistentProgressService)
        {
            _persistentProgressService = persistentProgressService ?? throw new ArgumentNullException(nameof(persistentProgressService));
            _animator = GetComponent<SimpleAnimator>();
            Initialize();
        }
        
        protected override void CreateStates()
        {
            IdleState = new DoorIdleState(
                stateMachine: stateMachine, 
                animationName: AnimationStateConstants.IsIdle,
                animator: _animator,
                door: this);
            
            OpeningState = new DoorOpeningState(
                stateMachine: stateMachine, 
                animationName: AnimationStateConstants.IsOpening, 
                animator: _animator, 
                door: this);
            
            TryingToOpenState = new DoorTryingToOpenState(
                stateMachine: stateMachine, 
                animationName: AnimationStateConstants.IsTryingToOpen,
                animator: _animator,
                door: this);
        }

        protected override void SetDefaultState() =>
            stateMachine.Initialize(IdleState);
        
        public void SetOpeningState()
        {
            if (!stateMachine.State.IsCanBeInterrupted()) 
                return;
            
            if (_persistentProgressService.Player.Inventory.HasKey())
            {
                stateMachine.ChangeState(OpeningState);
                _isOpen = true;
            }
            else
            {
                stateMachine.ChangeState(TryingToOpenState);
            }
        }

        public bool DoorIsOpen() => _isOpen;
    }
}