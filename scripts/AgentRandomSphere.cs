using System;
using Godot;

public partial class AgentRandomSphere : NavigationAgent3D
{
	[Export]
	bool IsWildAnimal = false;
	[Export]
	float WanderRadius = 10.0f;
	[Export]
	float MinWaitTime = 2.0f;
	[Export]
	float MaxWaitTime = 4.0f;
	Random random = new Random();

	float timer = 0.0f;


    public override void _PhysicsProcess(double delta)
    {
		timer -= (float)delta;
		if(timer <= 0 && IsNavigationFinished() && !IsWildAnimal)
		{
			Vector3 randomPoint = GetRandomPointInSphere();
			Vector3 closestNavPoint = NavigationServer3D.MapGetClosestPoint(GetNavigationMap(),randomPoint);
			TargetPosition = closestNavPoint;
		}
		else if(timer <= 0 && IsNavigationFinished() && IsWildAnimal)
		{
			Vector3 randomPoint = GetRandomPointInTerrain();
			Vector3 closestNavPoint = NavigationServer3D.MapGetClosestPoint(GetNavigationMap(),randomPoint);
			TargetPosition = closestNavPoint;
		}
		timer = (float)(random.NextDouble() * (MaxWaitTime - MinWaitTime) + MinWaitTime);
    }

	public Vector3 GetRandomPointInSphere()
	{
		float x = (float)(random.NextDouble() * 2 - 1);
		float z = (float)(random.NextDouble() * 2 - 1);
		Vector3 direction = new Vector3(x,0,z).Normalized();

		float distance = (float)Mathf.Pow(random.NextDouble(),1.0/3.0) * WanderRadius;
		return direction * distance;
	}

	public Vector3 GetRandomPointInTerrain()
	{
		float x = (float)(random.NextDouble() * 2 - 1) * WanderRadius;
		float z = (float)(random.NextDouble() * 2 - 1) * WanderRadius;
		return new Vector3(x, 0, z);
	}
}
