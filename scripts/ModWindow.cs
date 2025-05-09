using Godot;


namespace Trinketos.HaciendaSimulator.ModLoader
{
    public partial class ModWindow : Control
    {
        [Export]
        Tree tree;
        [Export]
        Tree tree2;
        ModManager modManager;
        LuaReader luaReader;
        TreeItem root;
        TreeItem rootAddedMods;

        public override void _Ready()
        {
            base._Ready();
            modManager = GetNode<ModManager>("/root/ModManager");
            luaReader = GetNode<LuaReader>("/root/Lua");
            modManager.ScanModsFolder();
            root = tree.CreateItem();
            rootAddedMods = tree2.CreateItem();
            root.SetText(0, "Mods");
            root.SetText(1, "Created");
            root.SetText(2, "Description");
            root.SetText(3, "Dependencies");
            rootAddedMods.SetText(0, "Mods");
            rootAddedMods.SetText(1, "Created");
            rootAddedMods.SetText(2, "Description");
            rootAddedMods.SetText(3, "Dependencies");

            foreach (string mod in modManager.mods)
            {
                TreeItem item = tree.CreateItem();
                item.SetText(0, mod);
                item.SetText(1, "2.4.3");
                item.SetText(2, "mod description");
                item.SetText(3, "[0 a.d]");
            }
        }

        void OnCancelPressed()
        {
            GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
        }

        void OnSaveConfigurationPressed()
        {
            modManager.LoadModList();
        }
    }
}
