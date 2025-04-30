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
        RichTextLabel Description;

        Array<string> MapsStringName = [];

        int item_selected = 0;
        MapData mapData;
        public override void _Ready()
        {
            base._Ready();
            mapData = (MapData)GetNode<MapData>("/root/MapData");
            GetMapsFromFolder();
            
        }

        public override void _Input(InputEvent @event)
        {
            base._Input(@event);
            if(@event is InputEventMouseButton e)
            {
                if(e.IsPressed() && e.ButtonIndex == MouseButton.Left)
                {
                    if(MapList.GetRect().HasPoint(e.Position))
                    {
                        GetDataFromSelectedItem(e.Position);
                    }
                }
            }
        }

        public void GetMapsFromFolder()
        {
            TreeItem root = MapList.CreateItem();
            MapList.HideRoot = true;
            DirAccess directory = DirAccess.Open("res://data/maps/");

            if(directory != null)
            {
                directory.ListDirBegin();
                string fileName = directory.GetNext();
                while(fileName != "")
                {
                    if(directory.CurrentIsDir())
                    {

                        GD.Print($"Found directory: {fileName}");
                    }
                    else
                    {
                        if(fileName.GetExtension() == "json")
                        {
                            GD.Print($"Found file: {fileName}");
                            TreeItem item = MapList.CreateItem(root);
                            item.SetText(0,fileName.GetBaseName().Capitalize());
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

            GD.Print(MapsStringName);
        }

        void GetDataFromSelectedItem(Vector2 position)
        {
            FileAccess fileAccess = FileAccess.Open($"res://data/maps/{MapsStringName[(int)MapList.GetItemAtPosition(position).GetInstanceId()]}",FileAccess.ModeFlags.Read);
            Dictionary json = (Dictionary) Json.ParseString(fileAccess.GetAsText());
            Description.Text = Json.Stringify(json);
            //TODO: Populate MapDataGlobalNode whit json data.
        }

        void OnItemSelected()
        {
            //GetDataFromSelectedItem();
        }

        void OnStartPressed()
        {

        }

        void OnBackPressed()
        {
            GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
        }
    }
}
