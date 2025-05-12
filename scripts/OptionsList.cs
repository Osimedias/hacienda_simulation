using Godot;
using Godot.Collections;


namespace Trinketos.HaciendaSimulator
{
    [GlobalClass]
    public partial class OptionsList : Resource
    {
        [Export]
        public Dictionary<string, Variant> Elements;
    }
}
