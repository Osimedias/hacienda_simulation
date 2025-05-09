shader_type canvas_item;

uniform sampler2D grass_texture : source_color;
uniform sampler2D sand_texture : source_color;
uniform sampler2D cliff_texture : source_color;
uniform sampler2D water_mask;

void fragment() {
	vec4 red = texture(cliff_texture,UV*120.0).rgba * texture(TEXTURE,UV).r;
	vec4 green = texture(grass_texture,UV*120.0).rgba * texture(TEXTURE,UV).g;
	vec4 blue = texture(sand_texture,UV*120.0).rgba * texture(TEXTURE,UV).b;
	red.a = 1.0;
	green.a = 1.0;
	blue.a = 1.0;
	vec4 mask = texture(water_mask,UV);
	vec4 color_mix = red+green+blue;
	float intensity = mask.r;
	vec4 final_color = mix(color_mix,vec4(0.0,0.0,1.0,1.0),intensity);
	COLOR = final_color;
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
