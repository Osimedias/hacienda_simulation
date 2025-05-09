using System.Collections.Generic;
using Godot;

namespace Trinketos.HaciendaSimulator
{
    public partial class ModManager : Node
    {

        string user_data_path = "user://mods/";
        string res_data_path = "res://mods/";

        public List<string> mods;
        public List<string> modsCreatedAtTimes;
        public List<string> modsDescriptions;
        public List<string> modDependences;

        public ModManager()
        {
            mods = new List<string>();
            modsCreatedAtTimes = new List<string>();
            modsDescriptions = new List<string>();
            modDependences = new List<string>();
        }

        public void ScanModsFolder()
        {
            if(!DirAccess.DirExistsAbsolute(user_data_path))
            {
                GD.PrintErr($"Error: The folder of mods '{user_data_path}' do not exists.");
                return;
            }
            DirAccess access = DirAccess.Open(user_data_path);

            if (access != null)
            {
                access.ListDirBegin();
                string fileName = "";
                while ((fileName = access.GetNext()) != "")
                {
                    if (fileName.EndsWith(".pck") || fileName.EndsWith(".zip"))
                    {
                        mods.Add(fileName);
                    }
                }
                access.ListDirEnd();
            }
        }

        public void LoadModList()
        {
            foreach (string mod in mods)
            {
                bool success = ProjectSettings.LoadResourcePack(ProjectSettings.GlobalizePath(user_data_path + mod));

                if (success)
                {
                    GD.Print("Loaded mod: " + mod);
                }
                else
                {
                    GD.PrintErr("Error at loading mod: " + mod);
                }
                if (ResourceLoader.Exists($"res://mods/{mod}/mod.json"))
                {
                    FileAccess file = FileAccess.Open($"res://mods/{mod}/mod.json", FileAccess.ModeFlags.Read);
                    string jsonText = file.GetAsText();
                    file.Close();

                    Godot.Collections.Dictionary<string,string> jsonData = Json.ParseString(jsonText).AsGodotDictionary<string,string>();
                    modsDescriptions.Add(jsonData["description"].ToString());
                    modDependences.Add(jsonData["dependencies"]);
                    GD.Print("The mod.json is ready");
                }
            }
        }
        public void LoadModScripts(string modName)
        {
            string modPath = $"res://mods/{modName}/scripts/";
                
            DirAccess dir = DirAccess.Open(modPath);
            if(dir == null) return;

            dir.ListDirBegin();
            string fileName = "";
            while ((fileName = dir.GetNext()) != "")
            {
                if(fileName.EndsWith(".lua"))
                {
                    GD.Print($"Loading lua script: {fileName}");
                        
                }
            }
            dir.ListDirEnd();
        }

        public void LoadLuaScript(string scriptPath)
        {
            FileAccess file = FileAccess.Open(scriptPath, FileAccess.ModeFlags.Read);
            if(file == null)
            {
                GD.PrintErr($"Error at loading lua script: {scriptPath}");
                return;
            }

            string scriptCountent = file.GetAsText();
            file.Close();

            LuaReader reader = GetNode<LuaReader>("/root/Lua");
            reader.RunScriptFromString(scriptCountent);
        }
    }




}
