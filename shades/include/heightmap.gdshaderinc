
float sample_heightmap(sampler2D sampler, vec2 position){
	return texture(sampler,position).r;
}

vec4 encode_height_to_viewport(float h){
	uint u = floatBitsToUint(h);
	return vec4(
		float((u >> 0u) & 255u),
		float((u >> 8u) & 255u),
		float((u >> 16u) & 255u),
		float((u >> 24u) & 255u)
	);
}

float decode_height_from_viewport(vec4 c) {
	uint u = uint(c.r * 255.0)
		| (uint(c.g * 255.0) << 8u)
		| (uint(c.b * 255.0) << 16u)
		| (uint(c.a * 255.0) << 24u);
	return uintBitsToFloat(u);
}

float sample_height_from_viewport(sampler2D screen, vec2 uv){
	ivec2 ts = textureSize(screen,0);
	vec2 norm_to_px = vec2(ts);

	vec2 uv_px_f = uv * norm_to_px - vec2(0.5);

	ivec2 uv_px = ivec2(uv_px_f);

	ivec2 p00 = uv_px;
	ivec2 p10 = uv_px + ivec2(1,0);
	ivec2 p01 = uv_px + ivec2(0, 1);
	ivec2 p11 = uv_px + ivec2(1, 1);

	vec4 c00 = texelFetch(screen, p00, 0);
	vec4 c10 = texelFetch(screen, p10, 0);
	vec4 c01 = texelFetch(screen, p01, 0);
	vec4 c11 = texelFetch(screen, p11, 0);

	float h00 = decode_height_from_viewport(c00);
	float h10 = decode_height_from_viewport(c10);
	float h01 = decode_height_from_viewport(c01);
	float h11 = decode_height_from_viewport(c11);

	vec2 f = fract(uv_px_f);
	float h = mix(mix(h00, h10, f.x), mix(h01, h11, f.x), f.y);
	return h;
}

