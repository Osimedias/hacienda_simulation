using Godot;
using Godot.Collections;

//this class need to be parent to a building like a stockpile
namespace Trinketos.HaciendaSimulator
{
	[GlobalClass]
	public partial class StorageNode : Node
	{
		public Dictionary<StringName, Goods> inventory = new Dictionary<StringName, Goods>();
		public void AddGoods(Goods goods)
		{
			if(inventory.ContainsKey(goods.Name))
			{
				inventory[goods.Name].Amount += goods.Amount;
			}
			else
			{
				inventory.Add(goods.Name,goods);
			}
		}

		public bool RemoveGoods(Goods goods, int amount)
		{
			if (inventory.ContainsKey(goods.Name) && inventory[goods.Name].Amount >= amount)
			{
				inventory[goods.Name].Amount -= amount;
				return true;
			}
			return false;
		}

		public Goods GetGoods(string name)
		{
			if(inventory.ContainsKey(name))
			{
				return inventory[name];
			}
			return null;
		}
	}
}