using Animations;
using Environment.DoorObject;

namespace StateMachines.Environment
{
    public class DoorOpeningState : DoorBaseMachineState
    {
        public DoorOpeningState(
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