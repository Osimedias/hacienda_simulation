shader_type spatial;

#include "include/heightmap.gdshaderinc"

uniform sampler2D u_terrain_heightmap;
uniform sampler2D u_terrain_normalmap;
uniform sampler2D u_terrain_colormap;
uniform sampler2D u_map; // This map will control color
uniform mat4 u_terrain_inverse_transform;
uniform mat3 u_terrain_normal_basis;


varying float v_hole;


vec3 unpack_normal(vec4 rgba){
	vec3 n = rgba.xzy * 2.0 - vec3(1.0);
	n.z *= 1.0;
	return n;
}


void vertex(){
	vec4 wpos = MODEL_MATRIX * vec4(VERTEX,1);
	vec2 cell_coords = (u_terrain_inverse_transform * wpos).xz;
	cell_coords += vec2(0.5);

	UV = cell_coords / vec2(textureSize(u_terrain_heightmap,0));

	float h = sample_heightmap(u_terrain_heightmap,UV);
	VERTEX.y = h;
	wpos.y = h;

	vec4 tint = texture(u_terrain_colormap,UV);
	v_hole = tint.a;
	NORMAL = u_terrain_normal_basis * unpack_normal(texture(u_terrain_normalmap, UV));
}


void fragment() {
	if (v_hole < 0.5) {
		// TODO Add option to use vertex discarding instead, using NaNs
		discard;
	}

	vec3 terrain_normal_world =
		u_terrain_normal_basis * unpack_normal(texture(u_terrain_normalmap, UV));
	terrain_normal_world = normalize(terrain_normal_world);
	vec3 normal = terrain_normal_world;

	vec4 value = texture(u_map, UV);
	// TODO Blend toward checker pattern to show the alpha channel

	ALBEDO = value.rgb;
	ROUGHNESS = 0.5;
	NORMAL = (VIEW_MATRIX * (vec4(normal, 0.0))).xyz;
}
