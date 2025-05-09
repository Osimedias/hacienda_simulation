using Godot;
using System;
/*
    file: MainMenu.cs.
    author: Saúl Rodríguez Martínez (Trinketos)
    date: 1:01 PM 24/04/25

    This code is part of Hacienda Simulation(Shity name xdxd).
    So the owner of this code is me Trinketos.

    MainMenu Script nothing else.
*/
namespace Trinketos.HaciendaSimulator
{
    public partial class MainMenu : Control
    {
        [Export(PropertyHint.File, "*.tscn")]
        string newGameScene;
        [Export(PropertyHint.File, "*.tscn")]
        string loadGameScene;
        [Export(PropertyHint.File, "*.tscn")]
        string optionScene;
        [Export(PropertyHint.File, "*.tscn")]
        string mapEditorScene;

        public void OnNewGamePressed()
        {
            GetTree().ChangeSceneToFile(newGameScene);
        }

        public void OnLoadGamePressed()
        {
            GetTree().ChangeSceneToFile(loadGameScene);
        }

        public void OnOptionsPressed()
        {
            GetTree().ChangeSceneToFile(optionScene);
        }

        public void OnMapEditorPressed()
        {
            MapData mapData = GetNode<MapData>("/root/MapData");
            mapData.splatmap = GD.Load<Texture2D>("res://textures/map_editor/splatmap_blank.png");
            mapData.heightmap = GD.Load<Texture2D>("res://textures/map_editor/heightmap_blank.png");
            mapData.watermask = GD.Load<Texture2D>("res://textures/map_editor/water_mask_blank.png");
            GetTree().ChangeSceneToFile(mapEditorScene);
        }

        public void OnExitPressed()
        {
            GetTree().Quit();
        }
    }
}
