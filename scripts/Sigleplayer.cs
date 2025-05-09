using Godot;
using Godot.Collections;


/*
    file: Singleplayer.cs.
    author: Saúl Rodríguez Martínez (Trinketos)
    date: 1:20 PM 27/04/25

    This code is part of Hacienda Simulation(Shity name xdxd).
    So the owner of this code is me Trinketos.

    Show all avaliable maps in a Tree node you select a element of that list and a preview and descriptions is show in the right panel.
    Also populates the MapData.cs whit related shit of the terrain and the position of the entities of the map.
*/

namespace Trinketos.HaciendaSimulator
{
    public partial class Sigleplayer : Control
    {
        [Export]
        Tree MapList;
        [Export]
        TextureRect MapView;
        [Export]
        RichTextLabel Description;

        Array<string> MapsStringName = [];
        int item_selected = 0;
        MapData mapData;
        public override void _Ready()
        {
            base._Ready();
            MapList.GuiInput += OnGuiInput;
            mapData = GetNode<MapData>("/root/MapData");
            GetMapsFromFolder();
            MapList.SetSelected(MapList.GetRoot().GetChild(0), 0);
        }

        public void GetMapsFromFolder()
        {
            TreeItem root = MapList.CreateItem();
            MapList.HideRoot = true;
            DirAccess directory = DirAccess.Open("res://data/maps/");

            if (directory != null)
            {
                directory.ListDirBegin();
                string fileName = directory.GetNext();
                while (fileName != "")
                {
                    if (directory.CurrentIsDir())
                    {

                        GD.Print($"Found directory: {fileName}");
                    }
                    else
                    {
                        if (fileName.GetExtension() == "json")
                        {
                            GD.Print($"Found file: {fileName}");
                            TreeItem item = MapList.CreateItem(root);
                            item.SetText(0, fileName.GetBaseName().Capitalize());
                            MapsStringName.Add(fileName);

                        }
                    }
                    fileName = directory.GetNext();
                }
            }
            else
            {
                GD.Print("An Error occurred when trying to access the path");
            }
        }

        void GetDataFromSelectedItem(TreeItem item)
        {
            FileAccess fileAccess = FileAccess.Open($"res://data/maps/{MapsStringName[item.GetIndex()]}", FileAccess.ModeFlags.Read);
            string json_string = fileAccess.GetAsText();
            Json json = new Json();
            Dictionary<string, Variant> data = Json.ParseString(json_string).AsGodotDictionary<string, Variant>();
            Variant value;
            if (data.TryGetValue("information", out value))
            {
                Dictionary<string, string> information = value.AsGodotDictionary<string, string>();
                string text = $"Name: {information["name"]}\nDescription: {information["description"]}\nNatural Goods: {information["goods"]}";
                Description.Text = text;
            }
            if (data.TryGetValue("textures", out value))
            {
                Dictionary<string, string> textures = value.AsGodotDictionary<string, string>();
                mapData.heightmap = ResourceLoader.Load<Texture2D>(textures["heightmap"]);
                mapData.splatmap = ResourceLoader.Load<Texture2D>(textures["splatmap"]);
                mapData.watermask = ResourceLoader.Load<Texture2D>(textures["water_mask"]);
                mapData.treeDistMask = ResourceLoader.Load<Texture2D>(textures["tree_distribution"]);
                MapView.Texture = mapData.splatmap;
                ShaderMaterial material = MapView.Material as ShaderMaterial;
                material.SetShaderParameter("water_mask", mapData.watermask);
            }

        }

        private void OnGuiInput(InputEvent @event)
        {
            if (@event is InputEventMouseButton e && e.Pressed)
            {
                Vector2 mousePos = e.Position;
                TreeItem item = MapList.GetItemAtPosition(mousePos);
                if (item != null)
                {
                    GD.Print($"TreeItem text selected: {item.GetIndex()}");
                    GetDataFromSelectedItem(item);
                }
                else
                {
                    GD.Print("I don't find eny TreeItem at that position");
                }
            }
        }

        void OnStartPressed()
        {
            GetTree().ChangeSceneToFile("res://scenes/world.tscn");
        }

        void OnBackPressed()
        {
            GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
        }
    }
}
