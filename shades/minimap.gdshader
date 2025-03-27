shader_type canvas_item;

uniform sampler2D splatmap : source_color;

void fragment() {
	COLOR.rgb = texture(splatmap,UV).rgb;
}
