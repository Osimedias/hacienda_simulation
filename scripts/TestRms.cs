using Godot;
using System;

namespace Trinketos.HaciendaSimulator
{
    public partial class TestRms : Control
    {
        [Export]
        TextureRect texture;
        void OnGeneratePressed()
        {
            RandomMapGenerator map = GetNode<RandomMapGenerator>("/root/RandomMapScript");
            map.mapWidth = 512;
            map.mapHeight = 512;
            Texture2D texture2D = map.GenerateHeightmap();
            Texture2D river = map.GenerateRiver(new Vector2I(50, 10), new Vector2I(200, 240), texture2D.GetImage());
            texture2D = river;
            texture.Texture = texture2D;
        }
    }
}
