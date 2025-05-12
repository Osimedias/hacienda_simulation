using System.Threading.Tasks;
using Godot;


namespace Trinketos.HaciendaSimulator
{
    public partial class SceneTransition : CanvasLayer
    {
        //private float _time = 0.0f;
        string currentScene;
        ColorRect _colorRect;
        ProgressBar progressBar;

        float progressValue = 0.0f;
        string path;

        [Signal]
        public delegate void SceneLoadedEventHandler(string path);

        public override void _Ready()
        {
            base._Ready();
            _colorRect = GetNode<ColorRect>("ColorRect");
            progressBar = GetNode<ProgressBar>("ProgressBar");
            _colorRect.Modulate = new Color(0,0,0,0);
            progressBar.Hide();
        }
        public override void _Process(double delta)
        {
            base._Process(delta);
            if(path == "")
            {
                GD.Print($"{path} is empty");
                return;
            }
            Godot.Collections.Array progress = new Godot.Collections.Array();
            
            ResourceLoader.ThreadLoadStatus status = ResourceLoader.LoadThreadedGetStatus(path,progress);
            if(status == ResourceLoader.ThreadLoadStatus.InProgress)
            {
                progressValue = (float)progress[0]*100.0f;
                progressBar.Value = Mathf.MoveToward(progressBar.Value,progressValue,delta* 20);
            }
            if(status == ResourceLoader.ThreadLoadStatus.Loaded)
            {
                //_time = 1.0f;
                progressBar.Value = Mathf.MoveToward(progressBar.Value,100.0, delta * 150);
                if(progressBar.Value >= 100)
                {
                   EmitSignal(SignalName.SceneLoaded,path);
                   PackedScene scene = (PackedScene)ResourceLoader.LoadThreadedGet(path);
                   GetTree().ChangeSceneToPacked(scene);
                }
            }
        }
        public async void GoToScene(string scene)
        {
           await _TransitionIn();
           path = scene;
           ResourceLoader.LoadThreadedRequest(path);
           _TransitionOut();
        }

        private async Task _TransitionIn()
        {
            progressBar.Show();
            Tween tween = GetTree().CreateTween();
            tween.TweenProperty(_colorRect,"modulate:a",1.0f,2.0 / 2f);
            await ToSignal(tween,Tween.SignalName.Finished);
        }
        private void _TransitionOut()
        {
            progressBar.Hide();
            Tween tween = GetTree().CreateTween();
            tween.TweenProperty(_colorRect,"modulate:a",0.0f,3.5f / 2f);
        }
    }
}