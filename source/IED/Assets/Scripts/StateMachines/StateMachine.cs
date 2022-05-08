namespace StateMachines
{
    public class StateMachine
    {
        public BaseStateMachineState State { get; private set; }

        private void ExitState()
        {
            State.Exit();
        }

        public void Initialize(BaseStateMachineState state)
        {
            State = state;
            State.Enter();
        }

        public void ChangeState(BaseStateMachineState newState)
        {
            ExitState();
            Initialize(newState);
        }
    }
}