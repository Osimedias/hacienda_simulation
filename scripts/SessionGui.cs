using Godot;

namespace Trinketos.HaciendaSimulator
{
    public partial class SessionGui : Control
    {
        [Export]
        Camera3D miniMapCamera;

        [Export]
        Control BuildingsPanel;
        [Export]
        Control HaciendaPanel;
        [Export]
        Control StockpilePanel;
        [Export]
        Control GranneryPanel;
        [Export]
        Control MarketPanel;
        [Export]
        Control BarracksPanel;

        [Export]
        Control ResumePanel;
        [Export]
        Control OptionsPanel;
        [Export]
        Control PausePanel;
        [Export]
        Control SurrenderPanel;
        [Export]
        Control ObjetivesPanel;

        MenuButton menuButton;
        private string _currentContext = "building";
        public string currentContext {
            get {return _currentContext;}
            set {_currentContext = value;}
        }


        public enum Context {
            Hacienda,
            Barracks,
            Stockpile,
            Grannery,
            Buildings,
            Market
        }

        public Context context = Context.Buildings;

        SceneTransition st;

        [Signal]
        public delegate void ChangeContextEventHandler(string context);
        
        public override void _Ready()
        {
            miniMapCamera.Size = GetParent().GetNode<Terrain>("Terrain").GetChild<MeshInstance3D>(0).GetAabb().Size.X;
            menuButton = GetNode<MenuButton>("MenuPanel/HBoxContainer/Menu");
            PopupMenu popupMenu = menuButton.GetPopup();
            popupMenu.IdPressed += OnMenuIdPressed;
            st = GetNode<SceneTransition>("/root/SceneTransition");
            PausePanel.Hide();
            SurrenderPanel.Hide();
            ObjetivesPanel.Hide();
        }
        public void ContextManager()
        {
            switch (context)
            {
                case Context.Hacienda:
                BuildingsPanel.Hide();
                HaciendaPanel.Show();
                StockpilePanel.Hide();
                GranneryPanel.Hide();
                MarketPanel.Hide();
                BarracksPanel.Hide();
                break;
                case Context.Barracks:
                BuildingsPanel.Hide();
                HaciendaPanel.Hide();
                StockpilePanel.Hide();
                GranneryPanel.Hide();
                MarketPanel.Hide();
                BarracksPanel.Show();
                break;
                case Context.Stockpile:
                BuildingsPanel.Hide();
                HaciendaPanel.Hide();
                StockpilePanel.Show();
                GranneryPanel.Hide();
                MarketPanel.Hide();
                BarracksPanel.Hide();
                break;
                case Context.Grannery:
                BuildingsPanel.Hide();
                HaciendaPanel.Hide();
                StockpilePanel.Hide();
                GranneryPanel.Show();
                MarketPanel.Hide();
                BarracksPanel.Hide();
                break;
                case Context.Buildings:
                BuildingsPanel.Show();
                HaciendaPanel.Hide();
                StockpilePanel.Hide();
                GranneryPanel.Hide();
                MarketPanel.Hide();
                BarracksPanel.Hide();
                break;
                case Context.Market:
                BuildingsPanel.Show();
                HaciendaPanel.Hide();
                StockpilePanel.Hide();
                GranneryPanel.Hide();
                MarketPanel.Show();
                BarracksPanel.Hide();
                break;
                default:
                break;
            }
        }

        public void OnMenuIdPressed(long id)
        {
            switch(id)
            {
                case 0:
                GD.Print("Save is pressed");
                break;
                case 1:
                ResumePanel.Show();
                break;
                case 2:
                OptionsPanel.Show();
                break;
                case 3:
                if(GetTree().Paused)
                {
                    GetTree().Paused = false;
                    PausePanel.Hide();
                }
                else
                {
                    GetTree().Paused = true;
                    PausePanel.Show();
                }
                break;
                case 4:
                SurrenderPanel.Show();
                break;
                case 5:
                st.GoToScene("res://scenes/main_menu.tscn");
                break;
                default:
                GD.Print($"{id} is not here");
                break;
            }
        }

    }
}