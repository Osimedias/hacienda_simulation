using Godot;



namespace Trinketos.HaciendaSimulator
{
    public partial class LoadGameMenu : Control
    {
        SceneTransition st;
        public override void _Ready()
        {
            base._Ready();
            st = GetNode<SceneTransition>("/root/SceneTransition");
        }
        void OnLoadPressed()
        {
            st.GoToScene("res://scenes/world.tscn");
        }
        void OnBackPressed()
        {
            st.GoToScene("res://scenes/main_menu.tscn");
        }
    }
}
