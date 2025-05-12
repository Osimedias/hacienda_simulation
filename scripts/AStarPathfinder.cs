using System.Collections.Generic;
using Godot;

namespace Trinketos.HaciendaSimulator
{
    public partial class AStarPathfinder : Node3D
    {
        private float GridStep = 2.0f;
        private float GridY = 0.0f;
        private Godot.Collections.Dictionary<Vector3, int> Points = new Godot.Collections.Dictionary<Vector3, int>();
        private AStar3D Astar = new AStar3D();

        public void ActivateAStarPathfinder()
        {
            var pathables = GetTree().GetNodesInGroup("terrain");
            AddPoints(pathables);
            ConnectPoints();
        }

        private void AddPoints(Godot.Collections.Array<Node> pathables)
        {
            foreach (Node node in pathables)
            {
                if (node is Node3D pathable)
                {
                    MeshInstance3D mesh = pathable.GetChild<MeshInstance3D>(0);
                    HeightMapShape3D heightShape = pathable.GetChild<CollisionShape3D>(1).Shape as HeightMapShape3D;
                    var aabb = mesh.GetAabb();
                    GD.Print("HeightmapShape3D.MapData is equal to aabb size: ",heightShape.MapData.Length == aabb.Size.X * aabb.Size.Z);
                    var startPoint = aabb.Position;
                    var xSteps = aabb.Size.X / GridStep;
                    var zSteps = aabb.Size.Z / GridStep;

                    for (int x = 0; x < xSteps; x++)
                    {
                        for (int z = 0; z < zSteps; z++)
                        {
                            var nextPoint = startPoint + new Vector3(x * GridStep, 0, z * GridStep);
                            AddPoint(nextPoint);
                        }
                    }
                }
            }
        }

        private void AddPoint(Vector3 point)
        {
            point.Y = GridY;
            int id = (int)Astar.GetAvailablePointId();
            Astar.AddPoint(id, point);
            Points[WorldToAstar(point)] = id;
        }

        private void ConnectPoints()
        {
            foreach (KeyValuePair<Vector3,int> kvp in Points)
            {
                Vector3 worldPos = kvp.Key;
                var adjacentPoints = GetAdjacentPoints(worldPos);
                int currentId = kvp.Value;

                foreach (int neighborId in adjacentPoints)
                {
                    if (!Astar.ArePointsConnected(currentId, neighborId))
                    {
                        Astar.ConnectPoints(currentId, neighborId);
                    }
                }
            }
        }
        private Godot.Collections.Array<int> GetAdjacentPoints(Vector3 worldPoint)
        {
            var adjacentPoints = new Godot.Collections.Array<int>();
            var searchCoords = new float[] { -GridStep, 0, GridStep };

            foreach (float x in searchCoords)
            {
                foreach (float z in searchCoords)
                {
                    var searchOffset = new Vector3(x, 0, z);
                    if (searchOffset == Vector3.Zero) continue;

                    var potentialNeighbor = WorldToAstar(worldPoint + searchOffset);
                    if (Points.ContainsKey(potentialNeighbor))
                        adjacentPoints.Add(Points[potentialNeighbor]);
                }
            }
            return adjacentPoints;
        }

        public void HandleObstacleAdded(Node3D obstacle)
        {
            var normalizedOrigin = obstacle.GlobalTransform.Origin;
            normalizedOrigin.Y = GridY;

            var pointKey = WorldToAstar(normalizedOrigin);
            int astarId = Points[pointKey];

            if (!Astar.IsPointDisabled(astarId))
            {
                Astar.SetPointDisabled(astarId, true);
            }
        }

        public void HandleObstacleRemoved(Node3D obstacle)
        {
            var normalizedOrigin = obstacle.GlobalTransform.Origin;
            normalizedOrigin.Y = GridY;

            var pointKey = WorldToAstar(normalizedOrigin);
            int astarId = Points[pointKey];

            if (Astar.IsPointDisabled(astarId))
            {
                Astar.SetPointDisabled(astarId, false);
            }
        }

        public Vector3[] GetPointPath(Vector3 from, Vector3 to)
        {
            int startId = (int)Astar.GetClosestPoint(from);
            int endId = (int)Astar.GetClosestPoint(to);
            return Astar.GetPointPath(startId, endId);
        }

        private Vector3 WorldToAstar(Vector3 world)
        {
            float x = Mathf.Snapped(world.X, GridStep);
            float y = GridY;
            float z = Mathf.Snapped(world.Z, GridStep);
            return new Vector3(x,y,z);
        }
    }
}