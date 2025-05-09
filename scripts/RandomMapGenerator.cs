using System;
using System.Collections.Generic;
using Godot;
namespace Trinketos.HaciendaSimulator
{
    public partial class RandomMapGenerator : Node
    {
        Terrain terrain = null;

        public int mapWidth = 0;
        public int mapHeight = 0;

        bool InWord = false;

        List<Vector2I> hillPositions = new List<Vector2I>();
        List<Vector2I> riverPositions = new List<Vector2I>();

        LuaReader reader;

        public override void _Ready()
        {
            reader = GetNode<LuaReader>("/root/Lua");
            terrain = GetTree().Root.GetNodeOrNull<Terrain>("Terrain");
            reader.RegisterFunction("generate_map", GenerateMap);
            reader.RegisterFunction("generate_terrain", GenerateTerrain);
            reader.RegisterFunction("place_object_at", PlaceObjectAt);
            reader.RegisterFunction("get_map_width", GetMapWidth);
            reader.RegisterFunction("get_map_height", GetMapHeight);
            reader.RegisterFunction("set_map_width", SetMapWidth);
            reader.RegisterFunction("set_map_height", SetMapHeight);
            reader.RegisterGlobalVariable("mapWidth", mapWidth);
            reader.RegisterGlobalVariable("mapHeight", mapHeight);

        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            if (terrain != null)
            {
                InWord = true;
            }
            else
            {
                InWord = false;
            }
        }
        public void GenerateMap()
        {
            GenerateTerrain();
        }
        public void GenerateTerrain()
        {
            MapData mapData = GetNode<MapData>("/root/MapData");
            mapData.heightmap = GenerateHeightmap();
            mapData.splatmap = GenerateSplatmap();
        }
        public void PlaceObjectAt(Vector2 position, string scene)
        {
            terrain.InstanceObjectAtPosition(position.X, position.Y, scene);
        }
        public int GetMapWidth() => mapWidth;
        public int GetMapHeight() => mapHeight;
        public void SetMapWidth(int value) => mapWidth = value;
        public void SetMapHeight(int value) => mapHeight = value;

        public Texture2D GenerateSplatmap()
        {
            Image splatmap = Image.CreateEmpty(mapWidth, mapHeight, false, Image.Format.Rgb8);
            splatmap.Fill(Colors.Green);
            FastNoiseLite noise = new FastNoiseLite();
            GD.Seed(GD.Randi());
            noise.Seed = (int)GD.Randi();
            noise.NoiseType = FastNoiseLite.NoiseTypeEnum.SimplexSmooth;
            noise.FractalType = FastNoiseLite.FractalTypeEnum.Fbm;
            noise.FractalOctaves = 8;
            noise.FractalGain = 0.45f;
            noise.FractalLacunarity = (float)GD.RandRange(0, 5.5);

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    Color heightColor = splatmap.GetPixel(x, y);
                    float height = noise.GetNoise2D(x, y); // La altura está en el canal rojo

                    Color splatColor = new Color();

                    // Aplicar reglas de materiales según altura
                    if (height < 0.05f)
                        splatColor = Colors.Blue; // Arena en tierras bajas
                    else if (height < 0.5f)
                        splatColor = Colors.Green; // Césped en llanuras
                    else if (height < 0.6f)
                        splatColor = Colors.Red; // Rocas en montañas
                    else
                        splatColor = Colors.White;
                    splatmap.SetPixel(x, y, splatColor);
                }
            }
            return ImageTexture.CreateFromImage(splatmap);
        }
        public Texture2D GenerateHeightmap()
        {
            Image image = Image.CreateEmpty(mapWidth, mapHeight, false, Image.Format.Rf);
            image.Fill(Colors.Red);
            FastNoiseLite noise = new FastNoiseLite();
            GD.Seed(GD.Randi());
            noise.Seed = (int)GD.Randi();

            noise.FractalOctaves = 4;
            noise.FractalLacunarity = 64.0f;

            for(int x = 0; x < mapWidth;x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    float color = noise.GetNoise2D(x,y);
                    image.SetPixel(x, y, new Color(color,color,color));
                }
            }

            foreach(Vector2I pos in hillPositions)
                AdjustHeight(image,pos, 1.2f);
            foreach(Vector2I pos in riverPositions)
                AdjustHeight(image, pos, -0.3f);

            return ImageTexture.CreateFromImage(image);
        }
        /*
        Not used for now
        private List<Vector2I> GetNeighbors(Vector2I position, int radius = 1)
        {
            List<Vector2I> neighbors = new List<Vector2I>();
            for (int dx = -radius; dx <= radius; dx++)
            {
                for (int dy = -radius; dy <= radius; dy++)
                {
                    if (dx == 0 && dy == 0) continue;//center is omited

                    Vector2I neighbor = new Vector2I(position.X + dx, position.Y + dy);

                    if (neighbor.X >= 0 && neighbor.X < mapWidth && neighbor.Y >= 0 && neighbor.Y < mapHeight)
                    {
                        neighbors.Add(neighbor);
                    }
                }
            }
            return neighbors;
        }
        */

        private void AdjustHeight(Image image, Vector2I pos, float adjustment)
        {
            if (pos.X < 0 || pos.X >= mapWidth || pos.Y < 0 || pos.Y >= mapHeight) return;

            Color currentColor = image.GetPixel(pos.X, pos.Y);
            float currentHeight = currentColor.R;
            float newHeight = Mathf.Clamp(currentHeight + adjustment, 0f, 1f);
            image.SetPixel(pos.X, pos.Y, new Color(newHeight, newHeight, newHeight));
        }

        public Texture2D GenerateRiver(Vector2I start, Vector2I end, Image image)
        {

            List<Vector2I> riverPath = new List<Vector2I>();

            int dx = Math.Abs(end.X - start.X);
            int dy = Math.Abs(end.Y - start.Y);
            int sx = (start.X < end.X) ? 1 : -1;
            int sy = (start.Y < end.Y) ? 1 : -1;
            int err = dx - dy;

            int x = start.X;
            int y = start.Y;

            while (true)
            {
                riverPath.Add(new Vector2I(x, y));

                if (x == end.X && y == end.Y)
                {
                    break;
                }

                int e2 = 2 * err;
                if (e2 > -dy) { err -= dy; x += sx; }
                if (e2 < dx) { err += dx; y += sy; }
            }
            ApplyRiverPath(riverPath, image);
            return ImageTexture.CreateFromImage(image);
        }
        private void ApplyRiverPath(List<Vector2I> riverPath, Image image)
        {
            foreach (Vector2I pos in riverPath)
            {
                image.SetPixel(pos.X, pos.Y, new Color(0.1f, 0.1f, 0.1f)); // Rebajar altura para el río
            }
        }
    }
}
