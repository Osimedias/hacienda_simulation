using Godot;

namespace Trinketos.HaciendaSimulator
{
    [GlobalClass]
    public partial class GoodsFactory : Node
    {
        public static Goods CreateGoods(StringName name, Texture2D icon, string description, int amount)
        {
            return new Goods
            {
                Name = name,
                Icon = icon,
                Description = description,
                Amount = amount,
                CellGUIPosition = 0
            };
        }
    }
}