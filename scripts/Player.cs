using Godot;
using Godot.Collections;

/*
    file: Player.cs.
    author: Saúl Rodríguez Martínez (Trinketos)
    date: 1:10 PM 27/04/25

    This code is part of Hacienda Simulation(Shity name xdxd).
    So the owner of this code is me Trinketos.

    Player class for the redundant player(You).
    Store all Goods and money, population, max population and track all object of the player.
*/


namespace Trinketos.HaciendaSimulator {
    [GlobalClass]
    public partial class Player : Node
    {
        [Export] 
        public StringName name = "Trinketos";
        [Export(PropertyHint.Range,"0,900,1,hide_slider")]
        public int currentPopulation = 0;
        [Export(PropertyHint.Range,"0,99999999,1,hide_slider")]
        public int maxCurrentPopulation = 0;
        [Export(PropertyHint.Range,"0,99999999,1,hide_slider")]
        public int money = 500;
        [Export(PropertyHint.Range,"0,100,1,hide_slider")]
        public int popularity = 100;

        [Export(PropertyHint.Range,"0,100,1,hide_slider")]
        int taxesRate = 50;
        [Export(PropertyHint.Range,"0,100,1,hide_slider")]
        int foodRations = 50;

        public int foodAmount = 0;
        public int stockAmount = 0;
        private Array<Node3D> _Houses;
        private Array<Node3D> _Peasents;

        private Building _Stockpile = null;
        private Building _Grannery = null;

        [Signal]
        public delegate void ChangeFoodValueEventHandler();
        [Signal]
        public delegate void ChangeStockpileValueEventHandler();

        public override void _Ready()
        {
            base._Ready();
            if(GetTree().GetNodeCountInGroup("House") > 0)
            {
                GetHousePopulation();
            }
            GetGlobalPopularity();

        }


        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);

            if(_Stockpile == null || _Grannery == null || _Stockpile == null && _Grannery == null)
            {
                FindStockBuildings();
            }

        }

        public int GetPopularityFromTaxesRates()
        {
            return 1 * taxesRate + (8*currentPopulation/maxCurrentPopulation);
        }
        public int GetPopularityFromFood()
        {
            return 10 * foodRations + (5*currentPopulation);
        }
        public int GetGlobalPopularity()
        {
            int result = taxesRate * foodRations / 2 * currentPopulation;
            return result;
        }

        public int GetHousePopulation()
        {
            if(GetTree().GetNodeCountInGroup("House") == 0)
            {
                return 0;
            }
            
            else {
                int houseRooms = 0;
                for (int i = 0; i < _Houses.Count; i++)
                {
                    houseRooms += 8;
                }
                return houseRooms;
            }
        }
        public void RemoveHouseFromArray()
        {
            _Houses.Remove(_Houses[0]);
        }
        public void RemoveHouseFromArray(int position)
        {
            _Houses.RemoveAt(position);
        }

        public void AddHouseToArray()
        {
            for (int i = 0; i < GetTree().GetNodeCountInGroup("House"); i++)
            {
                _Houses.Add(GetTree().GetNodesInGroup("House")[i] as Node3D);
            }
        }

        public void RemovePeasentFromArray()
        {
            _Peasents.Remove(_Peasents[0]);
        }

        public void RemovePeasentFromArray(int position)
        {
            _Peasents.RemoveAt(position);
        }

        public void AddPeasentToArray(Node3D peasent)
        {
            for (int i = 0; i < GetTree().GetNodeCountInGroup("Peasent"); i++)
            {
                _Peasents.Add(GetTree().GetNodesInGroup("House")[i] as Node3D);
            }
        }

        public void FindStockBuildings()
        {
            if(_Stockpile == null)
                _Stockpile = GetTree().GetFirstNodeInGroup("stockpile") as Building;
            if(_Grannery == null)
                _Grannery = GetTree().GetFirstNodeInGroup("granery") as Building;
        }

        
    }
}