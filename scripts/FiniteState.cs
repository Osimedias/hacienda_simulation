using Godot;
using Godot.Collections;

namespace Trinketos.HaciendaSimulator
{
    [GlobalClass]
    public partial class FiniteState : Resource
    {
        [Export]
        public string Name;
        [Export]
        Action action;
        [Export]
        public double Duration;
        [Export]
        public bool CanInterrupt;
        [Export]
        public Array<StateCondition> transitionConditions;
        [Export]
        Array<Action> Actions;
        public void Activate(Node context)
        {
            foreach (Action action in Actions)
            {
                action.Execute(context);
            }
        }
        public bool CanBeTransition(Node context)
        {
            foreach (StateCondition condition in transitionConditions)
            {
                if (!condition.Verificate(context))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
