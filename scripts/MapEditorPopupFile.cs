using Godot;

namespace Trinketos.HaciendaSimulator
{
    public partial class MapEditorPopupFile : MapEditorPopupMenu
    {
        void OnIdPressed(int id)
        {
            if (id == 0)
            {
                GD.Print("New Map Pressed");
            }
            if (id == 1)
            {
                GD.Print("Open Map Pressed");
            }
            if (id == 2)
            {
                GD.Print("Save Map Pressed");
            }
            if (id == 3)
            {
                GD.Print("Save As Map Pressed");
            }
            if (id == 4)
            {
                GD.Print("Import Heightmap pressed");
            }
            if (id == 5)
            {
                GD.Print("Exit pressed");
            }
        }
    }
}
