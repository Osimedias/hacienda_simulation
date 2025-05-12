using Godot;
using Godot.Collections;

namespace Trinketos.HaciendaSimulator
{
    public partial class SaveMap : GodotObject
    {


        public static void Save(string path)
        {
            Array<string> files = _GetFilesInPath(path);

            foreach (string file in files)
            {
                switch (file)
                {
                    case "heightmap.png":
                        GD.Print("I have a heightmap to pack");
                        break;
                    case "splatmap.png":
                        GD.Print("I have a splatmap to pack");
                        break;
                    case "colormap.png":
                        GD.Print("I have a colormap to pack");
                        break;
                    case "forest_density_map.png":
                        GD.Print("I have a forest density to pack");
                        break;
                    case "map_data.json":
                        GD.Print("I have a map_data to pack");
                        break;
                    default:
                        break;
                }
            }
        }

        public static void Load(string path)
        {

        }

        private static Array<string> _GetFilesInPath(string path)
        {
            Array<string> list = [];
            using var dir = DirAccess.Open(path);

            if (dir == null)
            {
                GD.Print("Error the Directory are empty");
                return [];
            }
            else
            {
                dir.ListDirBegin();
                string fileName = dir.GetNext();
                while (fileName != "")
                {
                    if (dir.CurrentIsDir())
                    {
                        GD.Print($"Found directory: {fileName}");
                    }
                    else
                    {

                        list.Add(fileName);
                    }
                }
            }
            return list;
        }
    }
}
