using Godot;
using Godot.Collections;

namespace Trinketos.HaciendaSimulator
{
    [GlobalClass]
    public partial class Player : Node
    {
        [Export]
        public StringName name = "Trinketos";
        [Export(PropertyHint.Range, "0,900,1,hide_slider")]
        public int currentPopulation = 0;
        [Export(PropertyHint.Range, "0,99999999,1,hide_slider")]
        public int maxCurrentPopulation = 0;
        [Export(PropertyHint.Range, "0,99999999,1,hide_slider")]
        public int money = 500;
        [Export(PropertyHint.Range, "0,100,1,hide_slider")]
        public int popularity = 100;

        [Export(PropertyHint.Range, "0,100,1,hide_slider")]
        int taxesRate = 50;
        [Export(PropertyHint.Range, "0,100,1,hide_slider")]
        int foodRations = 50;

        // Get by the sum of all resources in inventory of StoreNode
        public int foodAmount = 0;
        public int stockAmount = 0;

        private Array<Node3D> _Houses;
        private Array<Node3D> _Peasents;

        private StorageNode _Stockpile = null;
        private StorageNode _Grannery = null;

        [Signal]
        public delegate void ChangeFoodValueEventHandler();
        [Signal]
        public delegate void ChangeStockpileValueEventHandler();
        [Signal]
        public delegate void ChangeMoneyValueEventHandler();

        public override void _Ready()
        {
            base._Ready();
            if (GetTree().GetNodeCountInGroup("House") > 0)
            {
                GetHousePopulation();
            }
            GetGlobalPopularity();

        }


        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);

            if (_Stockpile == null || _Grannery == null)
            {
                FindStockBuildings();
            }
            if(_Stockpile != null || _Grannery != null)
            {
                SumAllGoodsInStorageInventory(_Stockpile,stockAmount);
                SumAllGoodsInStorageInventory(_Grannery,stockAmount);
            }

        }

        public int GetPopularityFromTaxesRates()
        {
            return 1 * taxesRate + (8 * currentPopulation / maxCurrentPopulation);
        }
        public int GetPopularityFromFood()
        {
            return 10 * foodRations + (5 * currentPopulation);
        }
        public int GetGlobalPopularity()
        {
            int result = taxesRate * foodRations / 2 * currentPopulation;
            return result;
        }

        public int GetHousePopulation()
        {
            if (GetTree().GetNodeCountInGroup("House") == 0)
            {
                return 0;
            }

            else
            {
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
        }

        public void FindStockBuildings()
        {
            if (_Stockpile == null && GetTree().GetNodeCountInGroup("stockpile") > 0)
                _Stockpile = GetTree().GetFirstNodeInGroup("stockpile").GetNode<StorageNode>("StoreNode");
                
            if (_Grannery == null && GetTree().GetNodeCountInGroup("granery") > 0)
                _Grannery = GetTree().GetFirstNodeInGroup("granery").GetNode<StorageNode>("StoreNode");
        }

        private void SumAllGoodsInStorageInventory(StorageNode storageNode,int valueTo)
        {
            foreach(Goods goods in storageNode.inventory.Values)
            {
                valueTo += goods.Amount;
            }
        }

    }
}
