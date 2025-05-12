using Godot;

namespace Trinketos.HaciendaSimulator
{
    public partial class MainMenu : Control
    {
        [Export]
        AudioStream backgroundMusic;
        [Export(PropertyHint.File, "*.tscn")]
        string newGameScene;
        [Export(PropertyHint.File, "*.tscn")]
        string loadGameScene;
        [Export(PropertyHint.File, "*.tscn")]
        string optionScene;
        [Export(PropertyHint.File, "*.tscn")]
        string mapEditorScene;
        SceneTransition st;

        public override void _Ready()
        {
            base._Ready();
            SoundManager soundManager = GetNode<SoundManager>("/root/AudioManager");
            st = GetNode<SceneTransition>("/root/SceneTransition");
            soundManager.PlayMusic(backgroundMusic);
        }

        public void OnNewGamePressed()
        {
            st.GoToScene(newGameScene);
        }

        public void OnLoadGamePressed()
        {
            st.GoToScene(loadGameScene);
        }

        public  void OnOptionsPressed()
        {
            st.GoToScene(optionScene);
        }

        public void OnMapEditorPressed()
        {
            MapData mapData = GetNode<MapData>("/root/MapData");
            mapData.splatmap = GD.Load<Texture2D>("res://textures/map_editor/splatmap_blank.png");
            mapData.heightmap = GD.Load<Texture2D>("res://textures/map_editor/heightmap_blank.png");
            mapData.watermask = GD.Load<Texture2D>("res://textures/map_editor/water_mask_blank.png");
            st.GoToScene(mapEditorScene);
        }

        public void OnExitPressed()
        {
            GetTree().Quit();
        }
    }
}
