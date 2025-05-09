using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using NLua;

namespace Trinketos.HaciendaSimulator
{
    public partial class LuaReader : Node
    {
        private Lua luaState;
        private Dictionary<string, string> loadedScripts = new Dictionary<string, string>();
        public Dictionary<string, object> globalVariables = new Dictionary<string, object>();
        Dictionary<string, PackedScene> sceneRegistry = new Dictionary<string, PackedScene>();

        public override void _Ready()
        {
            base._Ready();
            luaState = new Lua();
            luaState.DoString(@"
                os.execute = nil;
                os.remove = nil;
                os.open = nil;
            ");
            LoadScrips("res://data/scripts/");
            RegisterFunction("print_message", new Action<string>(PrintMessage));
            RegisterFunction("print_error", new Action<string>(PrintError));
            RegisterFunction("print_waring", new Action<string>(PrintWarnig));
            RegisterFunction("print_rich", new Action<string>(PrintRichMessage));
            RegisterFunction("create_entity", new Func<string, Node, Node>(InstantiateScene));
        }

        public void RegisterFunction(string name, Delegate function)
        {
            //luaState.RegisterFunction(name, this, function.Method);
            luaState.RegisterFunction(name,this, function.Method);
        }

        public void RegisterAction(string name,Action function)
        {
            luaState[name] = function;
        }

        public void RegisterGlobalVariable(string name, object value)
        {
            globalVariables[name] = value;
            luaState[name] = value;
        }

        public void SyncLuaVariables()
        {
            foreach (var key in globalVariables.Keys)
            {
                globalVariables[key] = luaState[key];
            }
        }

        public void RegisterNode(string name, Node node)
        {
            luaState[name] = node;
        }

        public void RegisterScene(string name, string path)
        {
            PackedScene scene = GD.Load<PackedScene>(path);
            if (scene != null)
            {
                sceneRegistry[name] = scene;
            }
        }

        public Node InstantiateScene(string name, Node parent)
        {
            if (sceneRegistry.ContainsKey(name))
            {
                Node instance = sceneRegistry[name].Instantiate();
                parent.AddChild(instance);
                return instance;
            }
            return null;
        }

        public void LoadScrips(string directoryPath)
        {
            DirAccess dir = DirAccess.Open(directoryPath);

            if (dir != null)
            {
                dir.ListDirBegin();
                string fileName;
                while ((fileName = dir.GetNext()) != "")
                {
                    if (fileName.EndsWith(".lua"))
                    {
                        FileAccess file = FileAccess.Open(directoryPath + "/" + fileName, FileAccess.ModeFlags.Read);
                        if (file != null)
                        {
                            string code = file.GetAsText();
                            loadedScripts[fileName] = code;
                            GD.Print($"Script {fileName} cargado.");
                        }
                    }
                }
                dir.ListDirEnd();
            }
        }

        public void RunScript(string scriptName)
        {
            if (loadedScripts.ContainsKey(scriptName))
            {
                luaState.DoString(loadedScripts[scriptName]);
                SyncLuaVariables();
                GD.Print($"Ejecutando script: {scriptName}");
            }
            else
            {
                GD.Print($"Script {scriptName} no encontrado.");
            }
        }

        public async Task RunScriptAsync(string scriptName)
        {
            if (loadedScripts.ContainsKey(scriptName))
            {
                await Task.Run(() => luaState.DoString(loadedScripts[scriptName]));
                SyncLuaVariables();
                GD.Print($"Ejecutando script: {scriptName}");
            }
            else
            {
                GD.Print($"Script {scriptName} no encontrado.");
            }
        }

        public void LoadAndRunScript(string scriptPath)
        {
            FileAccess file = FileAccess.Open(scriptPath, FileAccess.ModeFlags.Read);
            if (file != null)
            {
                string code = file.GetAsText();
                luaState.DoString(code);
                SyncLuaVariables();
                GD.Print($"Ejecutando script desde {scriptPath}");
            }
        }

        private void RunScriptFromResource(string path)
        {
            InternalLuaScript luaScript = ResourceLoader.Load<InternalLuaScript>(path);
            luaState.DoString(luaScript.Code);
        }

        public void RunScriptFromString(string content)
        {
            luaState.DoString(content);
        }

        public void PrintMessage(string message) => GD.Print($"Lua says: {message}");
        public void PrintError(string message) => GD.PrintErr($"Lua says: {message}");
        public void PrintWarnig(string message) => GD.PushWarning($"Lua says: {message}");
        public void PrintRichMessage(string message) => GD.PrintRich($"lua says: {message}");

    }
}
