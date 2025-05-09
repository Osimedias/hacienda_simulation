using Godot;
/*
    file: SceneTransition.cs.
    author: Saúl Rodríguez Martínez (Trinketos)
    date: 1:17 PM 27/04/25

    This code is part of Hacienda Simulation(Shity name xdxd).
    So the owner of this code is me Trinketos.

    I don't explain what the fuck this script do.Is global scope
	it's a comment that explain use cases
				|

*/

public partial class SceneTransition : CanvasLayer
{
    string currentScene;
    ColorRect _colorRect;
    float _switchDuration = 3.0f;

    public override void _Ready()
    {
        base._Ready();
        _colorRect = GetNode<ColorRect>("ColorRect");
    }
    /*
	Use this function like this:
	SceneTransition st = GetNode<SceneTransition>("/root/SceneTransition");
	Node ev = GetNode("/root/Events");
	st.GoToScene("game",(ev,"game_loaded"));
	or:
	Node ev = GetNode("/root/Events");
	awiat ToSignal(GetTree().CreateTimer(2f),SceneTreeTimer.SignalName.Timeout);
	ev.EmitSignal("game_loaded");
	*/
    public async void GoToScene(string scene, (GodotObject, string)? awaitable = null)
    {
        _colorRect.MouseFilter = Control.MouseFilterEnum.Stop;

        Tween tween = GetTree().CreateTween();
        tween.SetPauseMode(Tween.TweenPauseMode.Process);
        tween.TweenProperty(_colorRect, "modulate", new Color(1, 1, 1, 1), _switchDuration / 2f);
        await ToSignal(tween, Tween.SignalName.Finished);

        GetTree().ChangeSceneToFile(scene);
        if (awaitable != null)
        {
            await ToSignal(awaitable.Value.Item1, awaitable.Value.Item2);
        }
        GetTree().Paused = false;
        currentScene = scene;

        tween = GetTree().CreateTween();
        tween.SetPauseMode(Tween.TweenPauseMode.Process);
        tween.TweenProperty(_colorRect, "modulate", new Color(1, 1, 1, 0), _switchDuration / 2f);
        _colorRect.MouseFilter = Control.MouseFilterEnum.Ignore;
    }
}
