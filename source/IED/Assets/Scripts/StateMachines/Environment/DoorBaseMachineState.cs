using System;
using Animations;
using Environment.DoorObject;
using StateMachines;
using UnityEngine;

namespace StateMachines.Environment
{
    public class DoorBaseMachineState : BaseStateMachineState
    {
        protected readonly DoorStateMachine _door;
        protected readonly SimpleAnimator _animator;
        private readonly StateMachine _stateMachine;

        public DoorBaseMachineState(StateMachine stateMachine, string animationName, SimpleAnimator animator, DoorStateMachine door)
        {
            _stateMachine = stateMachine ?? throw new ArgumentNullException(nameof(stateMachine));
            _door = door ?? throw new ArgumentNullException(nameof(door));
            _animator = animator ?? throw new ArgumentNullException(nameof(animator));
            
            this.animationName = Animator.StringToHash(animationName);
        }

        public override bool IsCanBeInterrupted() => true;

        public override void Enter()
        {
            base.Enter();
            _animator.SetBool(animationName, true);
        }

        public override void Exit()
        {
            base.Exit();
            _animator.SetBool(animationName, false);
        }

        public void ChangeState(DoorBaseMachineState state)
        {
            _stateMachine.ChangeState(state);
        }
    }
}