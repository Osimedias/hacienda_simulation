using Godot;



namespace Trinketos.HaciendaSimulator
{
    [GlobalClass]
    public partial class InternalLuaScript : Resource
    {
        [Export(PropertyHint.MultilineText)]
        public string Code;

        public InternalLuaScript()
        {
            Code = "print_message('Hola')";
        }
    }
}
