using Godot;


namespace Trinketos.HaciendaSimulator
{
    [GlobalClass]
    public partial class Action : Resource
    {
        public virtual void Execute(Node context) { }
    }
}
