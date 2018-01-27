half _WaveStrength;
half _WaveSpeed;
half _WaveScale;

inline float3 WaveVertex(float3 worldPos, fixed mask)
{
	float3 pos = 0;

	float sinOff = (worldPos.x + worldPos.y + worldPos.z);
	worldPos += sinOff;
	worldPos *= _WaveScale;
	float t = _Time.y * _WaveSpeed;

	worldPos.x += fmod(t*2.11, UNITY_TWO_PI);
	worldPos.y += fmod(t*2.33, UNITY_TWO_PI);
	worldPos.z += fmod(t*2.2, UNITY_TWO_PI);

	pos = sin(worldPos);

	return pos * _WaveStrength * mask;
}

inline bool ShouldWave(fixed mask)
{
	return mask > 0.01;
}

inline float3 WaveVertex2D(float3 worldPos, fixed mask, out float3 normal)
{
	//const float epsilon = 0.01;
	float2 pos = 0;
	float2 normalTemp = 0;

	float sinOff = (worldPos.x + worldPos.z) * _WaveScale;
	worldPos *= _WaveScale;
	worldPos += sinOff;
	float t = _Time.y * _WaveSpeed;

	worldPos.x += fmod(t*2.11, UNITY_TWO_PI);
	//worldPos.y += fmod(t*2.33, UNITY_TWO_PI);
	worldPos.z += fmod(t*2.2, UNITY_TWO_PI);

	sincos(worldPos.xz, pos, normalTemp);

	//float v0 = snoise(worldPos.xz * _WaveScale + sinOff + t) * _WaveStrength * mask;
	//float vx = snoise(worldPos.xz * _WaveScale + sinOff + t + float2(epsilon, 0)) * _WaveStrength * mask;
	//float vy = snoise(worldPos.xz * _WaveScale + sinOff + t + float2(0, epsilon)) * _WaveStrength * mask;
	//normal.xz = -(float2(vx - v0, vy - v0) * 10);

	//pos = snoise_grad(worldPos.xz * _WaveScale + sinOff + t) * _WaveStrength * mask;

	//pos.x = sin(t*1.45 + (worldPos.x + sinOff) * _WaveScale) * _WaveStrength * mask;
	//pos.y = sin(t*3.12 + (worldPos.y + sinOff) * _WaveScale) * _WaveStrength * mask;
	//pos.z = sin(t*2.2 + (worldPos.z + sinOff) * _WaveScale) * _WaveStrength * mask;


	//normal.xz = -pos.xy * 0.1 * _WaveStrength * mask;
	//normal.b *= -1;
	normal.xz = -normalTemp * 0.5 * _WaveStrength * mask * _WaveScale;
	normal.y = sqrt(1.0 - saturate(dot(normal.xz, normal.xz)));

	return float3(0, (pos.x + pos.y + 2) * 0.5 * _WaveStrength * mask, 0);
}