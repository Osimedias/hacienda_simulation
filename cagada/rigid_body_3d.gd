extends RigidBody3D


func _integrate_forces(state: PhysicsDirectBodyState3D) -> void:
	if(state.get_contact_count() >= 1):
		var local_position_pos = state.get_contact_local_position(0)
		print(local_position_pos)
