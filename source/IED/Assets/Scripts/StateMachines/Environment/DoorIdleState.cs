using Animations;
using UnityEngine;
using Environment.DoorObject;

namespace StateMachines.Environment
{
    public class DoorIdleState : DoorBaseMachineState
    {
        public DoorIdleState(
            StateMachine stateMachine, 
            string animationName, 
            SimpleAnimator animator,
            DoorStateMachine door) 
            : base(stateMachine, animationName, animator, door)
        {
        }

        public override bool IsCanBeInterrupted()
        {
            return true;
        }
    }
}