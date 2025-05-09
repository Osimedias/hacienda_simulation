using Godot;

/*
    file: ResourceLabel.cs.
    author: Saúl Rodríguez Martínez (Trinketos)
    date: 1:11 PM 27/04/25

    This code is part of Hacienda Simulation(Shity name xdxd).
    So the owner of this code is me Trinketos.

    Show to the player all common resources in the top panel.
*/


namespace Trinketos.HaciendaSimulator
{
    public partial class ResourceLabel : HBoxContainer
    {
        [Export(PropertyHint.Enum, "Money,Food,Goods,Popularity,Population")]
        int CounterType;
        [Export]
        Player Player;

        Label Amount;
        TextureRect Icon;

        public override void _Ready()
        {
            base._Ready();
            Amount = GetChild(1) as Label;
            Icon = GetChild(0) as TextureRect;
            switch (CounterType)
            {
                case 0:
                    Amount.Text = Player.money.ToString();
                    break;
                case 1:
                    Amount.Text = Player.foodAmount.ToString();
                    break;
                case 2:
                    Amount.Text = Player.stockAmount.ToString();
                    break;
                case 3:
                    Amount.Text = Player.popularity.ToString();
                    break;
                case 4:
                    Amount.Text = Player.currentPopulation.ToString() + "/" + Player.maxCurrentPopulation.ToString();
                    break;
                default:
                    GD.Print("Money");
                    break;
            }
        }
    }
}
