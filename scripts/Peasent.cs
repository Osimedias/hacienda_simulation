using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Trinketos.HaciendaSimulator;

public partial class Peasent : Node3D
{
    [Export]
    public string WorkIn { get; set; } = "Firepit";
    [Export]
    public float MovementSpeed { get; set; } = 4.0f;
    [Export]
    string PeasentName { get; set; }
    [Export]
    Thoughts Thoughts { get; set; }

    Vector3 Target { get; set; }
    public Vector3 WorkingSitePosition { get; set; }
    public Building Store { get; set; }
    public Building BuildingForGettingGoods;

    NavigationAgent3D _navigationAgent;
    private float _movementDelta;

    LuaReader reader;

    AStarPathfinder aStarPathfinder;

    public Vector3[] currentPath;
    public int currentPathIndex = 0;

    public override void _Ready()
    {
        base._Ready();
        aStarPathfinder = GetParent().GetNode<AStarPathfinder>("/root/AStarPathfinder");
    }

    public void SetDestination(Vector3 targetPosition)
    {
        Vector3[] path = aStarPathfinder.GetPointPath(GlobalTransform.Origin, targetPosition);
        if(path == null || path.Length == 0)
        {
            GD.Print("Invalid rute, try to find a new rute");
            path = aStarPathfinder.GetPointPath(GlobalTransform.Origin, targetPosition + Vector3.Right * 2);
            if (path == null || path.Length == 0) path = aStarPathfinder.GetPointPath(GlobalTransform.Origin, targetPosition + Vector3.Left * 2);
            if (path == null || path.Length == 0) path = aStarPathfinder.GetPointPath(GlobalTransform.Origin, targetPosition + Vector3.Forward * 2);
            if (path == null || path.Length == 0) path = aStarPathfinder.GetPointPath(GlobalTransform.Origin, targetPosition + Vector3.Back * 2);

            if (path == null || path.Length == 0)
            {
                GD.PrintErr("Error: Not Invalid path.");
                currentPath = null;
                return;
            }
        }

        currentPath = path;
        currentPathIndex = 0;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (currentPath == null || currentPath.Length == 0) return;
        if (currentPathIndex < 0 || currentPathIndex >= currentPath.Length) return;

        Vector3 targetPoint = currentPath[currentPathIndex];
        Vector3 direction = (targetPoint - GlobalTransform.Origin).Normalized();
        float speed = 5.0f;
        GlobalTransform = GlobalTransform.Translated(direction * speed * (float)delta);

        if (GlobalTransform.Origin.DistanceTo(targetPoint) < 0.5f)
        {
            currentPathIndex++;
            
            // **Si el siguiente punto está bloqueado, recalcular ruta**
            if (currentPathIndex < currentPath.Length && aStarPathfinder.IsBlocked(GlobalTransform.Origin, currentPath[currentPathIndex]))
            {
                GD.Print("Block path, recalculating...");
                SetDestination(currentPath.Last());
            }
        }
    }

}
