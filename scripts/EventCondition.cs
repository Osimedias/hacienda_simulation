using Godot;

namespace Trinketos.HaciendaSimulator
{
    [GlobalClass]
    public partial class EventCondition : Resource
    {
        public virtual bool Evaluate(Node context) { return false; }
    }
}
