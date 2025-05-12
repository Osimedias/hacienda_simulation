using Godot;

namespace Trinketos.HaciendaSimulator
{
    [GlobalClass]
    public partial class StateConditionGaveMoney : StateCondition
    {
        [Export]
        public string signalName;
        private bool signalReceived = false;
        public override bool Verificate(Node context)
        {
            if(context.GetParent<Node3D>().GetNode<Player>("Player").IsConnected(signalName, Callable.From(OnSignalReceived)))
            {
                context.GetParent<Node3D>().GetNode<Player>("Player").Connect(signalName,Callable.From(() => OnSignalReceived()));
            }
            return signalReceived;
        }

        private void OnSignalReceived()
        {
            signalReceived = true;
            GD.Print($"Signal '{signalName}' received, transition permited");
        }

    }
}