using Godot;
using System;

namespace Trinketos.HaciendaSimulator
{
    public partial class ResourceShowIcon : Button
    {
        [Export]
        TextureRect GoodIcon;
        [Export]
        Goods goods;


        int Amount = 0;
        Label AmountText;
        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            if (goods != null)
            {
                GoodIcon.Texture = goods.Icon;
                TooltipText = goods.Description;
                Amount = goods.Amount;
            }
            AmountText = GetNode<Label>("TextureRect/Label");
            AmountText.Text = Amount.ToString();
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
            AmountText.Text = Amount.ToString();
        }
    }
}
