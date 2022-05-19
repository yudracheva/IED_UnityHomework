using Animations;
using Environment.DoorObject;

namespace StateMachines.Environment
{
    public class DoorTryingToOpenState : DoorBaseMachineState
    {
        public DoorTryingToOpenState(
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
        
        public override void TriggerAnimation()
        {
            base.TriggerAnimation();   
            ChangeState(_door.IdleState);
        }
    }
}