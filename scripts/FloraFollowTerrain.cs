using Godot;


namespace  Trinketos.HaciendaSimulator
{
	public partial class FloraFollowTerrain : Node3D
	{

		[Export(PropertyHint.Layers3DPhysics)]
		uint LayerMask;


		public void AdjustCoffeePlantsHeight()
		{
			Vector3 position = GlobalTransform.Origin;
			position.Y = GetHeightAtPosition(position);
			GlobalTransform = new Transform3D(Basis.Identity,position);
		}


		private float GetHeightAtPosition(Vector3 position)
		{
			var space = GetWorld3D().DirectSpaceState;
			PhysicsRayQueryParameters3D query = new PhysicsRayQueryParameters3D
			{
				From = position + Vector3.Up * 10,  // Dispara el rayo desde arriba
				To = position + Vector3.Down * 10, // Dispara hacia abajo
				CollisionMask = LayerMask // Asegurar que colisiona solo con el terreno
			};

			var result = space.IntersectRay(query);
			Vector3 resultPosition = (Vector3)result["position"];
			return result.Count > 0 ? resultPosition.Y : position.Y; // Retorna la altura del terreno
		}
	}
}