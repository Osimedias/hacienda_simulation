using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Trinketos.HaciendaSimulator
{
    public partial class WallSegment : Node3D
    {
        /*
		Node structure
		WallManager(Node3D)
		----WallSegment(Node3D)
		-----MeshInstance
		-----WallCollider(StaticBody3D)
		-----ConnectionManager(Node)
		----WallSegment(Node3D)
		*/
        [Export]
        public PackedScene WallStraight;
        public PackedScene WallCorner;

        private List<Vector3> neighborOffets = new List<Vector3>
        {
            new Vector3(1,0,0),
            new Vector3(-1,0,0),
            new Vector3(0,0,1),
            new Vector3(0,0,-1)
        };

        public void GenerateWall(Vector3 position)
        {
            WallSegment newWall;
            bool isCorner = DetectCorner(position);

            if (isCorner)
                newWall = WallCorner.Instantiate<WallSegment>();
            else
                newWall = WallStraight.Instantiate<WallSegment>();
            newWall.GlobalTransform = new Transform3D(newWall.GlobalTransform.Basis, position);
            GetTree().Root.AddChild(newWall);
        }

        public bool DetectCorner(Vector3 position)
        {
            int neighborCount = 0;
            foreach (Vector3 offset in neighborOffets)
            {
                if (GetWallAt(position + offset) != null)
                {
                    neighborCount++;
                }
            }
            return neighborCount >= 2;
        }

        private WallSegment GetWallAt(Vector3 position)
        {
            return GetTree().Root.GetChildren().OfType<WallSegment>().FirstOrDefault(wall => wall.GlobalTransform.Origin == position);
        }
    }
}
