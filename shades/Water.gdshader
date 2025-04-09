shader_type spatial;

uniform vec3 water_color : source_color;
uniform sampler2D noise_a;
uniform sampler2D noise_b;
uniform sampler2D sun_highlights : hint_default_white;

uniform sampler2D DEPTH_TEXTURE : hint_depth_texture;
uniform sampler2D SCREEN_TEXTURE : hint_screen_texture,repeat_disable;

uniform float depth_distance : hint_range(0.0, 10.0, 0.1) = 0.5;
uniform float water_color_ratio : hint_range(0.0, 1.0, 0.1) = 0.5;
uniform float beers_law : hint_range(0.0, 20.0, 0.1) = 3.0;
uniform float normal_scale : hint_range(0.0, 1.0, 0.1) = 0.5;
uniform float roughness_scale : hint_range(0.0, 1.0, 0.1) = 0.2;
uniform float time_scale : hint_range(0.0, 10.0, 0.1) = 5.0;

uniform float uv_scale : hint_range(0.0, 10.0, 0.1) = 1.0;
// a curve texture can be better
uniform sampler2D abberation : source_color;



varying vec4 world_uv;

vec3 soft_light(vec3 base, vec3 blend){
	vec3 limit = step(0.5,blend);
	return mix(2.0 * base * blend + base * base * (1.0 - 2.0 * blend),sqrt(base) * (2.0 * blend - 1.0) + (2.0*base) * (1.0 - blend),limit);
}


void vertex(){

	world_uv = MODEL_MATRIX * vec4(VERTEX,1.0);
}


void fragment(){
	vec2 _uv = world_uv.xz * uv_scale;
	vec2 _suv = SCREEN_UV;
	_uv.x += sin(TIME * time_scale + (_uv.x + _uv.y) * 25.0) * 0.01;
	_uv.y += sin(TIME * time_scale + (_uv.x - _uv.y) * 25.0) * 0.01;

	_suv.x += sin(TIME * time_scale + (_suv.x + _suv.y) * 25.0) * 0.01;
	_suv.y += sin(TIME * time_scale + (_suv.x - _suv.y) * 25.0) * 0.01;

	float depth_r = textureLod(DEPTH_TEXTURE,SCREEN_UV,0.0).r;
	vec4 world = INV_PROJECTION_MATRIX * vec4(SCREEN_UV,depth_r,1.0);
	world.xyz /= world.w;
	float depth_blend = 1.0 - smoothstep(world.z + depth_distance,world.z,VERTEX.z);
	depth_blend = exp(depth_blend * -beers_law);
	depth_blend = clamp(pow(depth_blend,3.0),0.0,1.0);
	vec3 screen = textureLod(SCREEN_TEXTURE,SCREEN_UV,0.0).rgb;
	vec3 curve = texture(abberation,UV).rgb;

	float r = texture(sun_highlights,_uv * curve.r).r;
	float g = texture(sun_highlights,_uv * curve.g).g;
	float b = texture(sun_highlights,_uv * curve.b).b;


	vec3 blended = soft_light(screen,vec3(r,g,b));

	vec3 water_color_final = mix(water_color.rgb,blended.rgb,0.4);

	ALBEDO = mix(screen * depth_blend,water_color_final,water_color_ratio);
	NORMAL_MAP = mix(texture(noise_a,_uv).rgb,texture(noise_b,_uv).rgb,(sin(TIME * time_scale)*1.0/2.0));
	NORMAL_MAP_DEPTH = 1.0;
	NORMAL += normal_scale;
	ROUGHNESS = roughness_scale;
}