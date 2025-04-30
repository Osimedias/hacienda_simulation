shader_type spatial;

///Used like a heightmap
uniform sampler2D water_mask;
//water height
uniform float water_height = 60.0;

uniform sampler2D water_color : source_color;
uniform sampler2D water_normal_a : hint_normal;
uniform sampler2D water_normal_b : hint_normal;

void vertex() {
	VERTEX.y += texture(water_mask,UV).r * water_height;
}

void fragment() {
	ALBEDO = texture(water_color,UV*120.0).rgb;
	NORMAL_MAP = texture(water_normal_a,UV*120.0).rgb + texture(water_normal_b,UV*120.0).rgb;
}
