using Godot;

/*
    file: State.cs.
    author: Saúl Rodríguez Martínez (Trinketos)
    date: 1:23 PM 27/04/25

    This code is part of Hacienda Simulation(Shity name xdxd).
    So the owner of this code is me Trinketos.

    Base class  for all states for the StateMachine.cs.
    extend this in another class to make a new state. is flaxible.
*/

namespace Trinketos.HaciendaSimulator
{
    [GlobalClass]
    public partial class State : Node
    {
        public StateMachine fsm;

        // Note use a timer for manager TransitionTo in StateMachine

        public virtual void Enter() { }
        public virtual void Exit() { }

        public virtual void StateReady() { }
        public virtual void Update(double delta) { }
        public virtual void PhysicsUpdate(double delta) { }
        public virtual void HandleInput(InputEvent @event) { }
    }
}
