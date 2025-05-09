using System.Collections.Generic;
using System.Linq;
using Godot;


namespace Trinketos.HaciendaSimulator
{
    public partial class AStarPathfinder : Node3D
    {
        [Export(PropertyHint.Layers3DPhysics)]
        uint LayerMask;
        AStar3D aStar3D;
        Dictionary<int,Vector3> pathNodes;


        public AStarPathfinder()
        {
            aStar3D = new AStar3D();
            pathNodes = new Dictionary<int, Vector3>();
        }
        public void GeneratePathNodes(Image heightmap, int gridSize)
        {
            int index = 0;
            int width = heightmap.GetWidth();
            int height = heightmap.GetHeight();

            for (int x = 0; x < width; x += gridSize)
            {
                for (int z = 0; z < height; z += gridSize)
                {
                    float y = heightmap.GetPixel(x, z).R * 60;
                    Vector3 position = new Vector3(x, y, z);
                    aStar3D.AddPoint(index, position);
                    pathNodes[index] = position;
                    index++;
                }
            }
        }

        
        public void ConnectPathNodes(int gridSize, Image heightmap, float maxHeightDifference)
        {
            foreach (var node in pathNodes)
            {
                int index = node.Key;
                Vector3 pos = node.Value;

                foreach (var neighbor in GetNeighbors(pos, gridSize, heightmap, maxHeightDifference))
                {
                    int neighborIndex = pathNodes.FirstOrDefault(n => n.Value == neighbor).Key;
                    if (neighborIndex != index && aStar3D.HasPoint(neighborIndex))
                    {
                        aStar3D.ConnectPoints(index,neighborIndex);
                    }
                }
            }
        }
        private List<Vector3> GetNeighbors(Vector3 position, int gridSize, Image heightmap, float maxHeightDifference)
        {
            List<Vector3> neighbors = new List<Vector3>();
            int width = heightmap.GetWidth();
            int height = heightmap.GetHeight();

            float baseHeight = heightmap.GetPixel((int)position.X, (int)position.Z).R * 60;

            int[] offsets = { -gridSize, 0, gridSize };

            foreach (int offsetX in offsets)
            {
                foreach (int offsetZ in offsets)
                {
                    if (offsetX == 0 && offsetZ == 0) continue;

                    int neighborX = (int)Mathf.Clamp(position.X + offsetX, 0, width - 1);
                    int neighborZ = (int)Mathf.Clamp(position.Z + offsetZ, 0, height - 1);
                    float neighborHeight = heightmap.GetPixel(neighborX, neighborZ).R * 60;

                    if (Mathf.Abs(baseHeight - neighborHeight) > maxHeightDifference) continue;

                    Vector3 neighborPos = new Vector3(neighborX, neighborHeight, neighborZ);

                    // **Comprobamos si hay un obstáculo en el camino**
                    if (!IsBlocked(neighborPos, position))
                    {
                        neighbors.Add(neighborPos);
                    }
                }
            }

            return neighbors;
        }


        public Vector3[] GetPointPath(Vector3 start, Vector3 end)
        {
            int startIndex = pathNodes.OrderBy(p => p.Value.DistanceTo(start)).First().Key;
            int endIndex = pathNodes.OrderBy(p => p.Value.DistanceTo(end)).First().Key;
            return aStar3D.GetPointPath(startIndex, endIndex);
        }

        public bool IsBlocked(Vector3 start, Vector3 end)
        {
            var space = GetWorld3D().DirectSpaceState;
            PhysicsRayQueryParameters3D query = new PhysicsRayQueryParameters3D
            {
                From = start + Vector3.Up * 1.0f,
                To = end + Vector3.Up * 1.0f,
                CollisionMask = LayerMask
            };
            var result = space.IntersectRay(query);
            return result.Count > 0;
        }
    }
}
