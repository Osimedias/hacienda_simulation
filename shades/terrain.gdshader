shader_type spatial;

render_mode diffuse_burley,world_vertex_coords,ensure_correct_normals;

uniform float height_scale = 0.5;
uniform float uv_texture_scale = 120.0;
group_uniforms TerrainMain;
uniform sampler2D heightmap : hint_default_black;
uniform sampler2D normalmap : hint_normal;
uniform sampler2D splatmap : source_color,filter_nearest;
uniform sampler2D roughness : hint_roughness_gray;
group_uniforms TerrainTextures;
uniform sampler2D grass_one : source_color,filter_linear_mipmap_anisotropic;
uniform sampler2D grass_one_norm : hint_normal;
uniform sampler2D grass_two : source_color, filter_linear_mipmap_anisotropic;
uniform sampler2D grass_two_norm : hint_normal;
uniform sampler2D cliff_one : source_color,hint_default_white, filter_linear_mipmap_anisotropic;
uniform sampler2D cliff_one_norm : hint_normal;


vec3 hash(vec3 p){
	p = vec3(dot(p, vec3(127.1, 311.7, 74.7)), dot(p, vec3(269.5,183.3,246.1)), dot(p, vec3(113.5, 271.9, 124.6)));
	p = fract(sin(p) * 43758.5453123);
	return p;
}

float shard_noise(in vec3 p, in float _sharpness) {
    vec3 ip = floor(p);
    vec3 fp = fract(p);

    float v = 0., t = 0.;
    for (int z = -1; z <= 1; z++) {
        for (int y = -1; y <= 1; y++) {
            for (int x = -1; x <= 1; x++) {
                vec3 o = vec3(float(x), float(y), float(z));
                vec3 io = ip + o;
                vec3 h = hash(io);
                vec3 r = fp - (o + h);

                float w = exp2(-TAU*dot(r, r));
                // tanh deconstruction and optimization by @Xor
                float s = _sharpness * dot(r, hash(io + vec3(11, 31, 47)) - 0.5);
                v += w * s*inversesqrt(1.0+s*s);
                t += w;
            }
        }
    }
    return ((v / t) * .5) + .5;
}

void vertex() {
	float height = texture(heightmap,UV).r;
	VERTEX.y += height * height_scale;
}


void fragment() {
	vec3 colors = texture(splatmap,UV).rgb;
	vec3 grass_1 = texture(grass_one,UV*uv_texture_scale).rgb * colors.r;
	vec3 grass_2 = texture(grass_two,UV*uv_texture_scale).rgb * colors.g;
	vec3 cliff_1 = texture(cliff_one,UV*uv_texture_scale).rgb * colors.b;
	vec3 grass_1_norm = texture(grass_one_norm,UV*uv_texture_scale).xyz * colors.r;
	vec3 grass_2_norm = texture(grass_two_norm,UV*uv_texture_scale).xyz * colors.g;
	vec3 cliff_1_norm = texture(cliff_one_norm,UV*uv_texture_scale).xyz * colors.b;

	vec3 final_color = vec3(0.0);
	final_color += grass_1;
	final_color += grass_2;
	final_color += cliff_1;
	vec3 final_normal = vec3(0.0);
	final_normal *= grass_1_norm;
	final_normal *= grass_2_norm;
	final_normal *= cliff_1_norm;
	ALBEDO = final_color.rgb;
	NORMAL_MAP = texture(normalmap, UV).xyz + final_normal.xyz;
	NORMAL_MAP_DEPTH = 1.0;
	AO = texture(heightmap,UV).r;
	AO_LIGHT_AFFECT = 5.5;
	ROUGHNESS = texture(roughness,UV).r * 0.002;
}