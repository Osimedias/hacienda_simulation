using Godot;
using Godot.Collections;
/*
    file: StateMachine.cs.
    author: Saúl Rodríguez Martínez (Trinketos)
    date: 1:23 PM 27/04/25

    This code is part of Hacienda Simulation(Shity name xdxd).
    So the owner of this code is me Trinketos.

    A GenericStateMachine use states for making complex crap whit this.
*/

namespace Trinketos.HaciendaSimulator
{
    [GlobalClass]
    public partial class StateMachine : Node
    {
        
        [Export]
        public State initialState;

        private Dictionary<string,State> _states;
        private State _currentState;

        public override void _Ready()
        {
            base._Ready();
            _states = new Dictionary<string, State>();
            foreach (Node state in GetChildren())
            {
                if(state is State s)
                {
                    _states[state.Name] = s;
                    s.fsm = this;
                    s.StateReady();
                    s.Exit();
                }
            }
            _currentState = initialState;
            _currentState.Enter();
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            _currentState.Update(delta);
        }

        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);
            _currentState.Update(delta);
        }
        public override void _UnhandledInput(InputEvent @event)
        {
            base._UnhandledInput(@event);
            _currentState.HandleInput(@event);
        }

        public void TransitionTo(string key)
        {
            if(!_states.ContainsKey(key) || _currentState == _states[key])
            {
                return;
            }
            _currentState.Exit();
            _currentState = _states[key];
            _currentState.Enter();
        }

    }
}