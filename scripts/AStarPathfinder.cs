using Godot;
using Godot.Collections;


namespace Trinketos.HaciendaSimulator
{
	public partial class AStarPathfinder : Node3D
	{


		MeshInstance3D Bounds;

		AStar3D aStar3D;


        public override void _Ready()
        {
            base._Ready();
			Bounds = GetChild<MeshInstance3D>(0);
			aStar3D = new AStar3D();
        }

		public void CreateGrid(bool ignoreY)
		{
			for (int x = 0; x < Bounds.GetAabb().Size.X; x++)
			{
				for (int y = 0; y < Bounds.GetAabb().Size.Y; y++)
				{
					for (int z = 0; z < Bounds.GetAabb().Size.Z; z++)
					{
						if(ignoreY)
						{
							aStar3D.AddPoint(1,new Vector3(x,0,z),1);
						}
						else
						{
							aStar3D.AddPoint(1,new Vector3(x,y,z),1);
						}
					}
				}
			}
			aStar3D.ConnectPoints(0,aStar3D.GetPointCount(),true);
		}
	}
}