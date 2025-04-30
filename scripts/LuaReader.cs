using Godot;
using Lua;
using Lua.Standard;

namespace Trinketos.HaciendaSimulator
{
    public partial class LuaReader : GodotObject
    {
        LuaState state = default;
        public void Setup()
        {
            state = LuaState.Create();
            state.OpenStandardLibraries();
        }
    }
}