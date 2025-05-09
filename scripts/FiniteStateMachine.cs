using Godot;
using Godot.Collections;

namespace Trinketos.HaciendaSimulator
{
    [GlobalClass]
    public partial class FiniteStateMachine : Node
    {
        [Export]
        public Array<FiniteState> States;
        private FiniteState CurrentState;
        private FiniteState NextState;
        private double stateTime;


        public override void _Ready()
        {
            base._Ready();
            CurrentState = States[0];
            stateTime = 0;
            CurrentState.Activate(this);
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            if (CurrentState != null)
            {
                stateTime += delta;
                if (CurrentState.Duration > 0 && stateTime >= CurrentState.Duration)
                {
                    if (CurrentState.CanBeTransition(this))
                    {
                        ChangeState(NextState);
                    }
                }
            }
        }
        public void ChangeState(FiniteState newState)
        {
            if (newState == null || (CurrentState != null && !CurrentState.CanInterrupt))
            {
                return;
            }
            CurrentState = newState;
            stateTime = 0;
            CurrentState.Activate(this);
        }

    }
}
