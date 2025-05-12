using Godot;

//This class need to be in peasents class as a child
namespace Trinketos.HaciendaSimulator
{
	[GlobalClass]
	public partial class TransportNode : Node
	{
		[Export]
		public StorageNode SourceStorage;
		[Export]
		public Node Destination;

		public async void MoveGoods(string name,string description,int amount)
		{
			Goods goods = SourceStorage.GetGoods(name);
			if(goods != null && goods.Amount >= amount)
			{
				SourceStorage.RemoveGoods(goods,amount);
				GetParent<Peasent>().Position = SourceStorage.GetParent<Building>().Position;
				GetParent<Peasent>().GetNode<Agent>("Agent").SetMovementTarget(Destination.GetParent<Building>().Position);
				await ToSignal(GetTree().CreateTimer(2.0f), "timeout");
				if(Destination is ProductionNode productionNode)
				{
					Goods processedGood = productionNode.ProcessGood(name,description,amount,amount);
					ReturnToStorage(processedGood);
				}
			}
		}

		public void ReturnToStorage(Goods processedGoods)
		{
			if(processedGoods != null)
			{
				GetParent<Peasent>().GetNode<Agent>("Agent").SetMovementTarget(SourceStorage.GetParent<Building>().Position);
				SourceStorage.AddGoods(processedGoods);
			}
		}
	}
}