using Godot;

namespace Trinketos.HaciendaSimulator
{
    [GlobalClass]
    public partial class EventState : Resource
    {
        [Export]
        public string Name;
        [Export]
        public EventCondition[] conditions;
        [Export]
        public ActionEvent[] actions;

        public bool Condition(Node context)
        {
            foreach (EventCondition condition in conditions)
            {
                if (!condition.Evaluate(context)) return false;
            }
            return true;
        }

        public void Execute(Node context)
        {
            if (Condition(context))
            {
                foreach (ActionEvent action in actions)
                {
                    action.Execute(context);
                }
            }
        }
    }
}
