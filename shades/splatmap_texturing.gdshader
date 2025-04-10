shader_type spatial;

uniform sampler2D splatmap : source_color;
uniform sampler2D tex_red : source_color,filter_linear_mipmap,repeat_enable;
uniform sampler2D tex_green : source_color,filter_linear_mipmap,repeat_enable;


void fragment() {
	vec3 splat = texture(splatmap,UV).rgb;
	
	vec3 red = texture(tex_red,UV*120.0).rgb * splat.r;
	vec3 green = texture(tex_green,UV*120.0).rgb * splat.g;
	vec3 blue = mix(green,red,0.5);
	vec3 result = red + green + blue;
	ALBEDO = result;
}
