using Godot;
using Godot.Collections;


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
            if (Options != null)
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
