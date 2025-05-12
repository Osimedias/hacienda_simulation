using Godot;


namespace Trinketos.HaciendaSimulator
{
    [GlobalClass]
    public partial class Goods : Resource
    {
        [Export]
        public Texture2D Icon;
        [Export]
        public StringName Name { get; set; }
        [Export(PropertyHint.MultilineText)]
        public string Description;
        [Export]
        public int Amount { get; set; }
        [Export]
        public int CellGUIPosition { get; set; }


        public Goods()
        {
            Name = "Goods";
            Description = "Put a description here";
            Amount = 0;
            CellGUIPosition = 0;
        }
    }
}
