using Godot;



namespace Trinketos.HaciendaSimulator
{
	public partial class ObjetivesPanel : ColorRect
	{
		public string Text;
		[Export]
		private RichTextLabel ObjetivesText;




        public override void _Ready()
        {
            base._Ready();
			ObjetivesText.Text = Text;
        }



		void OnClosePressed()
		{
			Hide();
		}
	}
}