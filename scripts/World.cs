using Godot;

namespace Trinketos.HaciendaSimulator
{
    [GlobalClass]
    public partial class World : Node3D
    {
        [Export]
        Camera3D debugCamera;
        [Export]
        AudioStreamPlaylist playlist;
        [Export]
        SessionGui sessionGUI;

        AStarPathfinder aStarPathfinder;
        public override void _Ready()
        {
            base._Ready();
            SoundManager soundManager = GetNode<SoundManager>("/root/AudioManager");
            AStarPathfinder aStarPathfinder = GetNode<AStarPathfinder>("/root/AStarPathfinder");
            soundManager.StopMusic();
            soundManager.PlayMusic(playlist);
            MapData mapData = GetNode<MapData>("/root/MapData");
            GetNode<NavigationRegion3D>("NavigationRegion3D").BakeNavigationMesh();
            GetNode<NavigationRegion3D>("NavigationRegion3D2").BakeNavigationMesh();
        }
        void OnGUIChangeContext(string context)
        {
            sessionGUI.currentContext = context;
            switch(context)
            {
                case "building":
                sessionGUI.context = SessionGui.Context.Buildings;
                sessionGUI.ContextManager();
                break;
                case "hacienda":
                sessionGUI.context = SessionGui.Context.Buildings;
                sessionGUI.ContextManager();
                break;
                case "barracks":
                sessionGUI.context = SessionGui.Context.Buildings;
                sessionGUI.ContextManager();
                break;
                case "stockpile":
                sessionGUI.context = SessionGui.Context.Buildings;
                sessionGUI.ContextManager();
                break;
                case "grannery":
                sessionGUI.context = SessionGui.Context.Buildings;
                sessionGUI.ContextManager();
                break;
                case "market":
                sessionGUI.context = SessionGui.Context.Buildings;
                sessionGUI.ContextManager();
                break;
                default:
                break;
            }
        }
    }
}
