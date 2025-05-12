using Godot;


namespace Trinketos.HaciendaSimulator
{
    [GlobalClass]
    public partial class ActionGaveMoney : Action
    {
        public override void Execute(Node context)
        {
            base.Execute(context);
            context.GetParent<Node3D>().GetNode<Player>("Player").money += 10;
        }
    }
}