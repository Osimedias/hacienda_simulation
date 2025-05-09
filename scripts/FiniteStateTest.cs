using Godot;


namespace Trinketos.HaciendaSimulator
{
    public partial class FiniteStateTest : StateCondition
    {
        [Export]
        public int money = 50;

        public override bool Verificate(Node context)
        {
            Player player = context.GetParent().GetNode<Player>("Player");
            return player.money <= money;
        }
    }
}
