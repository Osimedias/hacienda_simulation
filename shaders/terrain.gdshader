shader_type spatial;

uniform float tile_size = 120;
uniform float y_ratio = 60.0;
uniform sampler2D heightmap : hint_default_black,filter_linear;
uniform sampler2D splatmap : source_color,filter_linear;
uniform bool use_color_map;
uniform sampler2D colormap : source_color,filter_linear;
uniform sampler2D grass_texture : source_color;
uniform sampler2D sand_texture : source_color;
uniform sampler2D cliff_texture : source_color;

void vertex() {
	VERTEX.y += texture(heightmap,UV).r * y_ratio;
}

void fragment() {
	if(use_color_map){
		ALBEDO = texture(colormap,UV).rgb;
		ALPHA = texture(colormap,UV).a;
	}
	else
	{
		vec3 red = texture(cliff_texture,UV*tile_size).rgb * texture(splatmap,UV).r;
		vec3 green = texture(grass_texture,UV*tile_size).rgb * texture(splatmap,UV).g;
		vec3 blue = texture(sand_texture,UV*tile_size).rgb * texture(splatmap,UV).b;
		ALBEDO = red + green + blue;
		ALPHA = texture(splatmap,UV).a;
	}
}
