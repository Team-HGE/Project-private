
	//#define SNOW_TEXTURE_SCALE 0.1
	#define RELIEF_SAMPLE_COUNT 8
	#define RELIEF_BINARY_SAMPLE_COUNT 8
	#define RELIEF_MAX_RAY_LENGTH 0.5

	sampler2D _MainTex;
    sampler2D _GS_SnowTex;
    sampler2D _GS_SnowNormalsTex;
    sampler2D _GS_NoiseTex;
    float4    _GS_SunDir;		// w = terrain mark distance
	float4    _GS_SnowData1;	// x = relief, y = occlusion, z = glitter, w = brightness
	float4    _GS_SnowData3;    // x = Sun occlusion, y = sun atten, z = ground coverage, w = grass coverage
	float4    _GS_SnowData4;    // x = footprint scale, y = footprint obscurance, z = snow normals strength, w = minimum altitude for terrain including scattering
	float4    _GS_SnowData5;    // x = slope threshold, y = slope sharpness, z = slope noise, w = noise scale

	#define SNOW_RELIEF _GS_SnowData1.x
	#define SNOW_GLITTER _GS_SnowData1.z

	#define SNOW_NORMALS_STRENGTH _GS_SnowData4.z
		
	#define SNOW_TEXTURE_SCALE _GS_SnowData5.w

    float _SnowCoverage, _Scatter, _SnowScale;
    half3 _Color, _SnowTint;
    float _SlopeThreshold, _SlopeNoise, _SlopeSharpness;

	#define dot2(x) dot(x,x)
                
	// get snow coverage
    void SetSnowCoverage(float3 worldPos, inout half4 color, inout float3 normal, inout half3 specular, inout half smoothness) {
	
        half snowCover = _SnowCoverage;
        float3 snowPos = worldPos;
		snowPos.xz *= _SnowScale;
		
		// diffuse
		#if GLOBALSNOW_FLAT_SHADING
			half4 diff = tex2D(_GS_SnowTex, snowPos.xz * 0.02);
		#else
			half4 diff = tex2D(_GS_SnowTex, snowPos.xz * 0.02).gggg;
		#endif

		// get world space normal
		float3 wsNormal = normal;

		// prevent snow on walls and below minimum altitude
		float altG = tex2D(_GS_SnowTex, snowPos.xz * 0.5).g;
		float altNoise = diff.r * altG - 0.9;
	    float ny = wsNormal.y - _SlopeThreshold;
	    float flatSurface = saturate( (ny + altNoise * _SlopeNoise) * (5.0 + _SlopeSharpness));

		float minAltitude = saturate( 1.0 - altNoise * _Scatter);
		snowCover *= minAltitude * flatSurface;

		if (snowCover <= 0) {	
			return;
		}

		const float snowHeight = SNOW_RELIEF; // Range: 0.001 .. 0.3 or will distort
		float3 rayDir = normalize(_WorldSpaceCameraPos - snowPos);

		// relief
		#if GLOBALSNOW_RELIEF || GLOBALSNOW_OCCLUSION
		float height = 0;
		if (snowCover > 0.1) {
			// 3D surface mapping
			rayDir.y = max(0.3, rayDir.y);
			snowPos.y = 0;
			float3 snowPos0 = snowPos + rayDir * RELIEF_MAX_RAY_LENGTH;
			float3 rayStep = (snowPos - snowPos0) / (float)RELIEF_SAMPLE_COUNT; 
			// locate hit point
			UNITY_UNROLL
			for (int k=0;k<RELIEF_SAMPLE_COUNT;k++) {
				float h1 = tex2Dlod(_GS_NoiseTex, float4(snowPos0.xz * SNOW_TEXTURE_SCALE, 0, 0)).x * snowHeight;	// use tex2Dlod to prevent artifacts due to quantization of step
				if (h1>snowPos0.y) {
					snowPos = snowPos0;
					snowPos0 -= rayStep;
					break;
				}
				snowPos0 += rayStep;
			}
			// binary search
			UNITY_UNROLL
			for (int j=0;j<RELIEF_BINARY_SAMPLE_COUNT;j++) {
				float3 occ = (snowPos0 + snowPos) * 0.5;
				height = tex2D(_GS_NoiseTex, occ.xz * SNOW_TEXTURE_SCALE).x;	// don't use tex2Dlod to prevent occlusion artifacts - we need an average here
				if (height * snowHeight>occ.y) {
					snowPos = occ;
				} else {
					snowPos0 = occ;
				}
			}

		}
		#if GLOBALSNOW_OCCLUSION
			// fake occlusion (optional)
			float height2 = tex2D(_GS_NoiseTex, (snowPos.xz + normalize(_GS_SunDir.xz) * 0.03) * SNOW_TEXTURE_SCALE).x;
			diff.a = saturate(height-height2);
		#endif
		#endif // RELIEF
	
		// surface normal at point
		#if GLOBALSNOW_FLAT_SHADING
		    float3 norm = float3(0,1,0);
		#else
		    float4 texNorm1 = tex2D(_GS_SnowNormalsTex, snowPos.xz * SNOW_TEXTURE_SCALE);
		    float3 norm1 = UnpackNormal(texNorm1);
		    // perturb normal (optional)
		    float4 texNorm2 = tex2D(_GS_SnowNormalsTex, snowPos.xz * SNOW_TEXTURE_SCALE * 64.0);
		    float3 norm2 = UnpackNormal(texNorm2);

		    float3 norm = (norm1 + norm2) * 0.5;
			
			// rotate snow normal in world space
			float3 axis = normalize(float3(norm.y, 0, -norm.x));
			float angle = acos(norm.z);
			float s, c;
			sincos(angle, s, c);
    		float oc = 1.0 - c;
    		float3x3 rot = float3x3(oc * axis.x * axis.x + c, oc * axis.x * axis.y - axis.z * s,  oc * axis.z * axis.x + axis.y * s,
                oc * axis.x * axis.y + axis.z * s, oc * axis.y * axis.y + c, oc * axis.y * axis.z - axis.x * s,
                oc * axis.z * axis.x - axis.y * s, oc * axis.y * axis.z + axis.x * s, oc * axis.z * axis.z + c);

	        norm = lerp(mul(rot, wsNormal), norm.xzy, wsNormal.y);

			norm = lerp(wsNormal, norm, snowCover * SNOW_NORMALS_STRENGTH);

			norm = normalize(norm);

			// glitter (optional)
			float randomFacet = frac(norm.x * 97.13) - 0.975;
			float normalGloss = saturate(dot(-_GS_SunDir.xyz, norm));
			float glitter = saturate(randomFacet * 33.0) * normalGloss * SNOW_GLITTER;
			glitter *= 1.0 / (1.0 + dot2(_WorldSpaceCameraPos - snowPos) * 0.01);
			norm = lerp(norm, -_GS_SunDir.xyz, saturate(glitter));
		#endif

		// pass color data to output shader
		half3 snowAlbedo;
		#if GLOBALSNOW_OCCLUSION
		    snowAlbedo = (diff.rgb + glitter.xxx) * _GS_SnowData1.w * saturate (1.0 - diff.a * _GS_SnowData1.y);
		#elif GLOBALSNOW_FLAT_SHADING
		    snowAlbedo = diff.rgb * _GS_SnowData1.w;
		#else
		    snowAlbedo = (diff.rgb + glitter.xxx) * _GS_SnowData1.w;
		#endif
        snowAlbedo *= _SnowTint;

		specular = lerp(specular, diff.rgb * 0.025, snowCover);
		smoothness = lerp(smoothness, _GS_SnowData3.x * saturate(0.65 + _GS_SunDir.y), snowCover);
    	color = lerp(color, half4(snowAlbedo, snowCover), snowCover);
		normal = norm;
	}

