shader_type spatial;

uniform vec3 water_color : source_color;
uniform float metallic : hint_range(0.0, 1.0, 0.1) = 0.1;
uniform float roughness : hint_range(0.0, 1.0, 0.1) = 0.2;
uniform sampler2D normal_one : hint_normal;
uniform sampler2D normal_two : hint_normal;
uniform float normal_map_depth : hint_range(0.0, 10.0, 0.1);

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	
	vec3 normal_blend = mix(texture(normal_one,UV).rgb,texture(normal_two,UV).rgb,0.5);
	
	ALBEDO = water_color;
	METALLIC = metallic;
	ROUGHNESS = roughness;
	NORMAL_MAP = normal_blend;
}

//void light() {
//	// Called for every pixel for every light affecting the material.
//	// Uncomment to replace the default light processing function with this one.
//}
