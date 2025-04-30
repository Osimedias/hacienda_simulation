using Godot;
using Godot.Collections;

/*
    file: MenuOptionButton.cs.
    author: Saúl Rodríguez Martínez (Trinketos)
    date: 1:04 PM 27/04/25

    This code is part of Hacienda Simulation(Shity name xdxd).
    So the owner of this code is me Trinketos.

    This code manager all posible options for this cain of controler, it use a Resource to populate the items(less code).
*/

namespace Trinketos.HaciendaSimulator 
{
    public partial class MenuOptionButton : HBoxContainer
    {
        [Export]
        public string Title;
        [Export]
        OptionsList Options;


        private OptionButton _OptionsButton;
        private Label _Title;
        public override void _Ready()
        {
            base._Ready();
            _Title = GetNode<Label>("Title");
            _OptionsButton = GetNode<OptionButton>("OptionButton");
            _Title.Text = Title;
            if(Options != null)
            {
                CreateButtonsList();
            }
        }


        void CreateButtonsList()
        {
            Array<string> keys = (Array<string>)Options.Elements.Keys;
            Array<Variant> elements = (Array<Variant>)Options.Elements.Values;

            for (int i = 0; i < keys.Count; i++)
            {
            AddItem(elements[i]);
            }
        }


        void AddItem(Variant value)
        {
            _OptionsButton.AddItem(value.ToString());
        }

        void OnItemSelected(int index)
        {
            _OptionsButton.GetItemId(index);
        }
    }
}