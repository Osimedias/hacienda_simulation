using Godot;
using Godot.Collections;


namespace Trinketos.HaciendaSimulator
{
    [GlobalClass]
    public partial class MapEditorPopupMenu : PopupMenu
    {
        [Export]
        OptionsList options;


        public override void _Ready()
        {
            base._Ready();
            Array<string> keys = (Array<string>)options.Elements.Keys;
            Array<Variant> elements = (Array<Variant>)options.Elements.Values;

            for (int i = 0; i < keys.Count; i++)
            {
                AddItem(elements[i].ToString(), i);
            }
        }
    }
}
