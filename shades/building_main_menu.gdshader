shader_type spatial;

uniform sampler2D albedo : source_color,repeat_enable,filter_linear_mipmap;
uniform sampler2D norm : hint_normal,repeat_enable,filter_linear_mipmap;
uniform vec3 dirt_color : source_color;
uniform float rotate_uv_angle : hint_range(0.0, 360.0, 1.0) = 1.0;

vec2 rotate_uv(vec2 uv, vec2 pivot) {
	float cosa = cos(rotate_uv_angle);
	float sina = sin(rotate_uv_angle);
	uv -= pivot;
	return vec2(
		cosa * uv.x - sina * uv.y,
		cosa * uv.y + sina * uv.x
	) + pivot;
}


void fragment() {
	vec2 uv = rotate_uv(UV,vec2(0.5));
	vec3 color = COLOR.rgb;
	vec3 albedo_channel = texture(albedo,uv*20.0).rgb * color.b;
	vec3 dirt_channel = dirt_color * color.r * 0.6;
	ALBEDO = albedo_channel * dirt_channel;
	NORMAL_MAP = texture(norm,uv*20.0).rgb;
	// Called for every pixel the material is visible on.
}

//void light() {
//	// Called for every pixel for every light affecting the material.
//	// Uncomment to replace the default light processing function with this one.
//}
