using System.Net;
using Godot;
using Godot.Collections;

/*
    file: Building.cs.
    author: Saúl Rodríguez Martínez (Trinketos)
    date: 12:57 PM 27/04/25

    This code is part of Hacienda Simulation(Shity name xdxd).
    So the owner of this code is me Trinketos.

    This is the main class of all buildings in the game.
*/

namespace Trinketos.HaciendaSimulator {
    [GlobalClass]
    public partial class Building : StaticBody3D
    {
        [Export(PropertyHint.Layers3DPhysics)]
        uint collisionMask;

        [Export]
        Array<Goods> StoreGoods;

        [Export]
        Recipe recipie;

        [Export]
        Material BuildingMaterial;
        [Export]
        Material GhostMaterial;

        [Export]
        Node3D NullWorkerGraphic;

        [Export]
        public int WorkersNeeded;

        [Export]
        MeshInstance3D rayDraw;

        int currentWorkers = 0;
        public bool IsSelected = false;
        bool IsOnValidTerrain = false;
        bool IsInmovile = false;

        const int RayLength = 1000;

        MeshInstance3D Instance;
        Area3D DetectorArea;
        CollisionShape3D Shape;

        Color red = Colors.Red;
        Color green = Colors.Green;

        ImmediateMesh mesh;


        public override void _Ready()
        {
            base._Ready();
            IsOnValidTerrain = true;

            Instance = GetChildOrNull<MeshInstance3D>(0);
            DetectorArea = GetChildOrNull<Area3D>(2);
            Shape = DetectorArea.GetChild<CollisionShape3D>(0);

            if(NullWorkerGraphic == null)
            {
                GD.Print("No null worker graphics");
            }
/*
            if(IsInmovile)
            {
                Instance.MaterialOverride = BuildingMaterial;
            }
            else
            {
                Instance.MaterialOverride = GhostMaterial;
                if(GhostMaterial is StandardMaterial3D)
                {
                    StandardMaterial3D material = Instance.MaterialOverride as StandardMaterial3D;
                    material.AlbedoColor = green;
                }
            }*/
        }

        public override void _PhysicsProcess(double delta)
        {
            //IsOnValidTerrain = OnValidTerrain();
            base._PhysicsProcess(delta);
            PhysicsDirectSpaceState3D spaceState = GetWorld3D().DirectSpaceState;
            Camera3D camera = GetViewport().GetCamera3D();
            Vector2 mousePosition = GetViewport().GetMousePosition();

            Vector3 origin = camera.ProjectRayOrigin(mousePosition);
            Vector3 end = origin + camera.ProjectRayNormal(mousePosition) * RayLength;
            PhysicsRayQueryParameters3D query = PhysicsRayQueryParameters3D.Create(origin,end,collisionMask);
            query.CollideWithAreas = false;
            query.CollideWithBodies = true;

            var result = spaceState.IntersectRay(query);

            if(result.ContainsKey("position") && IsInmovile == false)
            {
                GlobalPosition = (Vector3)result["position"];
            }

           if(NullWorkerGraphic != null)
           {
                if(currentWorkers != WorkersNeeded || currentWorkers <= 0)
                {
                    NullWorkerGraphic.Show();
                }
                else
                {
                    NullWorkerGraphic.Hide();
                }
           }
        }

        public override void _Input(InputEvent @event)
        {
            base._Input(@event);
            if(@event is InputEventMouseButton e)
            {
                if(e.ButtonIndex == MouseButton.Left)
                {
                    /*
                    if(IsOnValidTerrain)
                    {
                        IsInmovile = true;
                        Instance.MaterialOverride = BuildingMaterial;
                    }*/
                    IsInmovile = true;
                    Instance.MaterialOverride = BuildingMaterial;
                }
            }
        }

        bool OnValidTerrain()
        {
            StandardMaterial3D material = Instance.MaterialOverride as StandardMaterial3D;
            material.AlbedoColor = red;

            if(DetectorArea.HasOverlappingBodies())
            {
                GD.PushWarning("DetectorArea is clipping against something.");
                return false;
            }

            BoxShape3D areaCollisionShape = Shape.GetShape() as BoxShape3D;
            Vector3 areaSize = areaCollisionShape.Size * 0.5f;
            Array<Vector3> pointsToCheck = [
                Shape.GlobalTransform.Origin + new Vector3(areaSize.X, -areaSize.Y, areaSize.Z),
                Shape.GlobalTransform.Origin + new Vector3(areaSize.X, -areaSize.Y, -areaSize.Z),
                Shape.GlobalTransform.Origin + new Vector3(-areaSize.X, -areaSize.Y, -areaSize.Z),
                Shape.GlobalTransform.Origin + new Vector3(-areaSize.X, -areaSize.Y, areaSize.Z)
            ];

            Array<float> yDistances = new Array<float>();

            int i = 0;

            foreach (var point in pointsToCheck)
            {
                Vector3 rayFrom = pointsToCheck[i];
                Vector3 rayTo = rayFrom + new Vector3(0,-50,0);
                PhysicsRayQueryParameters3D rayParameter = PhysicsRayQueryParameters3D.Create(rayFrom,rayTo);
                rayParameter.CollisionMask = CollisionMask;
                var rayCastResult = GetWorld3D().DirectSpaceState.IntersectRay(rayParameter);
                rayDraw.Mesh = CreateRayDraw(pointsToCheck[i],new Vector3(0,-150,0));

                if(rayCastResult.ContainsKey("position"))
                {
                    Vector3 position = (Vector3)rayCastResult["position"];
                    float yDistance = rayFrom.Y + position.Y;
                    yDistances.Add(yDistance);
                }
                else
                {
                    GD.PushWarning("A raycast failed to hit the ground.");
                    return false;
                }
                i += 1;
            }

            foreach(var yDistance in yDistances)
            {
                if(yDistance > 2.0f)
                {
                    GD.PushWarning("Not plannar enough, raycast failed on Y check.");
                    return false;
                }
            }
            GD.Print("Everythings is good! You can now place the building!");
            material.AlbedoColor = green;
            return true;
        }

        ImmediateMesh CreateRayDraw(Vector3 position1,Vector3 position2)
        {
            ImmediateMesh line = new ImmediateMesh();
            line.SurfaceBegin(Mesh.PrimitiveType.Lines);
            line.SurfaceAddVertex(position1);
            line.SurfaceAddVertex(position2);
            line.SurfaceEnd();

            return line;
        }
    }
}