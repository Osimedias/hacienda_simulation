using Godot;
using System;
/*
    file: ResourceShowIcon.cs.
    author: Saúl Rodríguez Martínez (Trinketos)
    date: 1:13 PM 27/04/25

    This code is part of Hacienda Simulation(Shity name xdxd).
    So the owner of this code is me Trinketos.

    A square button in a grid of buttons(is use by context in the game menu,example if you select the stockpile will show you all the goods you have).
*/

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
