shader_type spatial;

#include "include/heightmap_rgb8_encoding.gdshaderinc"

uniform float max_height;
uniform sampler2D height_map_mask : source_color,hint_default_black,filter_linear;

void vertex() {
	VERTEX.y += texture(height_map_mask,UV).r * max_height;
}
