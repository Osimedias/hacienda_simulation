using Godot;
using System;

public partial class Agent : NavigationAgent3D
{
	[Export]
	public float MovementSpeed {get; set; } = 4.0f;
	private float _movementDelta;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		VelocityComputed += OnVelocityComputed;
	}

	public void SetMovementTarget(Vector3 movementTarget)
	{
		Vector3 closestNavPoint = NavigationServer3D.MapGetClosestPoint(GetNavigationMap(),movementTarget);
		TargetPosition = closestNavPoint;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if (NavigationServer3D.MapGetIterationId(GetNavigationMap()) == 0)
		{
			return;
		}

		if(IsNavigationFinished())
		{
			return;
		}
		_movementDelta = MovementSpeed * (float)delta;
		Vector3 nextPathPosition = GetNextPathPosition();
		Vector3 newVelocity = GetParent<Node3D>().GlobalPosition.DirectionTo(nextPathPosition) * _movementDelta;
		if(AvoidanceEnabled)
		{
			Velocity = newVelocity;
		}
		else
		{
			OnVelocityComputed(newVelocity);
		}
	}


	private void OnVelocityComputed(Vector3 safeVelocity)
	{
		GetParent<Node3D>().GlobalPosition = GetParent<Node3D>().GlobalPosition.MoveToward(GetParent<Node3D>().GlobalPosition + safeVelocity, _movementDelta);
	}
}
