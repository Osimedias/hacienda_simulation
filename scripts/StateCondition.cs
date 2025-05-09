using Godot;

namespace Trinketos.HaciendaSimulator
{
    [GlobalClass]
    public partial class StateCondition : Resource
    {
        public virtual bool Verificate(Node context) { return true; }
    }
}
