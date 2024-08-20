#ifndef THOMAS_NOISE_INCLUDE
#define THOMAS_NOISE_INCLUDE

float hash(float p)
{
	p = frac(p * 0.011);
	p *= p + 7.5;
	p *= p + p;
	return frac(p);
}
float hash(float2 p)
{
	float3 p3 = frac(float3(p.xyx) * 0.13);
	p3 += dot(p3, p3.yzx + 3.333);
	return frac((p3.x + p3.y) * p3.z);
}

float noise(in float3 st)
{
	const float3 step = float3(110, 241, 171);

	float3 i = floor(st);
	float3 f = frac(st);
 
	float n = dot(i, step);

	float3 u = f * f * (3.0 - 2.0 * f);
	return lerp(lerp(lerp(hash(n + dot(step, float3(0, 0, 0))), hash(n + dot(step, float3(1, 0, 0))), u.x),
                lerp(hash(n + dot(step, float3(0, 1, 0))), hash(n + dot(step, float3(1, 1, 0))), u.x), u.y),
				lerp(lerp(hash(n + dot(step, float3(0, 0, 1))), hash(n + dot(step, float3(1, 0, 1))), u.x),
                lerp(hash(n + dot(step, float3(0, 1, 1))), hash(n + dot(step, float3(1, 1, 1))), u.x), u.y), u.z);
}

float fbm(in float3 st,
        in int octaves = 2, in float lacunarity = 2.0, in float gain = 0.5)
{
	float value = 0.0;
	float amplitude = 0.5;
	float frequency = 0.0;
	for (int i = 0; i < octaves; i++)
	{
		value += amplitude * noise(st);
		st *= lacunarity;
		amplitude *= gain;
	}
	return 0.5 + 0.5 * saturate(value);
}

#endif
