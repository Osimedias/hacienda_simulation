using Godot;

namespace Trinketos.HaciendaSimulator
{
	[GlobalClass]
	public partial class ProductionNode : Node
	{
		[Export]
		public StorageNode Storage;

		public Goods ProcessGood(string name,string description,int amount,int outputAmount)
		{
			Goods goods = Storage.GetGoods(name);

			if(goods != null && goods.Amount >= amount)
			{
				Storage.RemoveGoods(goods,amount);
				return GoodsFactory.CreateGoods(name,null,description,outputAmount);
			}
			return null;
		}
	}
}
