using Godot;
using Lua;

public partial class LuaTest : Node
{
	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
		var state = LuaState.Create();
		var results = await state.DoStringAsync("return 1 + 1");
		GD.Print(results[0]);
	}
}
