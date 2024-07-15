#ifndef GLOBALSNOW_DEFERRED_PASS
#define GLOBALSNOW_DEFERRED_PASS

	#define dot2(x) dot(x, x)
	#define RELIEF_SAMPLE_COUNT 8
	#define RELIEF_BINARY_SAMPLE_COUNT 8
	#define RELIEF_MAX_RAY_LENGTH 0.5

	TEXTURE2D_X_FLOAT(_GS_DeferredExclusionBuffer);
    TEXTURE2D_X(_GS_GBuffer0Copy);
    TEXTURE2D_X(_GS_GBuffer1Copy);
    #if GLOBALSNOW_FLAT_SHADING
		#define _GS_GBuffer2Copy _CameraNormalsTexture
    #else
		TEXTURE2D_X(_GS_GBuffer2Copy);
	#endif
	TEXTURE2D_X(_GS_GBuffer3Copy);

	sampler2D _GS_SnowTex;
    sampler2D _GS_SnowNormalsTex;
    sampler2D _GS_NoiseTex;
    TEXTURE2D(_GS_DepthTexture);
    sampler2D _GS_FootprintTex;
    sampler2D _GS_DetailTex;
    sampler2D _GS_DecalTex;
    float4    _GS_DecalTex_TexelSize;

	float4 	  _GS_SnowCamPos;
    float4    _GS_SunDir;		// w = terrain mark distance
	float4    _GS_SnowData1;	// x = relief, y = occlusion, z = glitter, w = brightness
	float4    _GS_SnowData2;	// x = minimum altitude for vegetation and trees, y = altitude scattering, z = coverage extension, w = minimum altitude
	float4    _GS_SnowData3;    // x = Sun occlusion, y = sun atten, z = ground coverage, w = grass coverage
	float4    _GS_SnowData4;    // x = footprint scale, y = footprint obscurance, z = snow normals strength, w = minimum altitude for terrain including scattering
	float4    _GS_SnowData5;    // x = slope threshold, y = slope sharpness, z = slope noise, w = noise scale
	float4    _GS_SnowData6;    // x = _Alpha, y = _Smoothness, z = altitude blending, w = snow thickness
	half4    _GS_SnowTint;

	float _GS_ExclusionBias;
	half _GS_MinimumGIAmbient;

	#define TERRAIN_MARKS_MAX_DISTANCE_SQR _GS_SunDir.w
	#define SNOW_RELIEF _GS_SnowData1.x
	#define SNOW_OCCLUSION _GS_SnowData1.y
	#define SNOW_GLITTER _GS_SnowData1.z
	#define SNOW_BRIGHTNESS _GS_SnowData1.w

	#define SNOW_ALTITUDE_SCATTERING _GS_SnowData2.y
	#define COVERAGE_EXTENSION _GS_SnowData2.z
	#define SNOW_ALTITUDE_MINIMUM _GS_SnowData2.w

	#define SUN_OCCLUSION _GS_SnowData3.x
	#define SUN_ATTENUATION _GS_SnowData3.y
	#define GROUND_COVERAGE _GS_SnowData3.z

	#define FOOTPRINT_SCALE _GS_SnowData4.x
	#define FOOTPRINT_OBSCURANCE _GS_SnowData4.y
	#define SNOW_NORMALS_STRENGTH _GS_SnowData4.z
	#define BILLBOARD_MIN_ALTITUDE _GS_SnowData4.w

	#define SNOW_SLOPE_THRESHOLD _GS_SnowData5.x
	#define SNOW_SLOPE_SHARPNESS _GS_SnowData5.y
	#define SNOW_SLOPE_NOISE _GS_SnowData5.z
	#define SNOW_TEXTURE_SCALE _GS_SnowData5.w

	#define SNOW_REDUCTION _GS_SnowData6.x
	#define SNOW_SMOOTHNESS _GS_SnowData6.y
	#define SNOW_ALTITUDE_BLENDING _GS_SnowData6.z
	#define SNOW_THICKNESS _GS_SnowData6.w

    #if GLOBALSNOW_COVERAGE_MASK
		TEXTURE2D(_GS_DepthMask);
		float4 _GS_DepthMaskWorldSize;
		half _GS_FillOutSideMask;
	#endif


float3 DecodeSceneNormal(float3 normal) {

    #if defined(_GBUFFER_NORMALS_OCT)
		float2 remappedOctNormalWS = Unpack888ToFloat2(normal); // values between [ 0,  1]
		float2 octNormalWS = remappedOctNormalWS.xy * 2.0 - 1.0;    // values between [-1, +1]
		normal = UnpackNormalOctQuadEncode(octNormalWS);
    #endif

    return normal;
}


float3 EncodeSceneNormal(float3 normal) {
	
    #if defined(_GBUFFER_NORMALS_OCT)
        float2 octNormalWS = PackNormalOctQuadEncode(normal);           // values between [-1, +1], must use fp32 on some platforms
        float2 remappedOctNormalWS = saturate(octNormalWS * 0.5 + 0.5);   // values between [ 0,  1]
        normal = PackFloat2To888(remappedOctNormalWS);      // values between [ 0,  1]
    #endif

	return normal;
}

	// get snow coverage
#if GLOBALSNOW_FLAT_SHADING
	void FragSnowDeferred(VaryingsSimple i, out half4 outAlbedo: SV_Target0, out half4 outSpecularMetallic: SV_Target1, out half4 outLighting : SV_Target2 ) {
#else
	void FragSnowDeferred(VaryingsSimple i, out half4 outAlbedo: SV_Target0, out half4 outSpecularMetallic: SV_Target1, out half4 outNormalSmoothness: SV_Target2, out half4 outLighting : SV_Target3 ) {
#endif

        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
		i.uv = UnityStereoTransformScreenSpaceTex(i.uv);

		float depthRaw = GetRawDepth(i.uv);
		float depthExclusionRaw = SAMPLE_TEXTURE2D_X(_GS_DeferredExclusionBuffer, sampler_PointClamp, i.uv).x;
		
		outSpecularMetallic = 0;
		#if !GLOBALSNOW_FLAT_SHADING
			outNormalSmoothness = half4(0,1,0,1);
		#endif
		outLighting = 0;
		outAlbedo = 0;

		float depth01 = RawTo01Depth(depthRaw);

		float depthExclusion01 = RawTo01Depth(depthExclusionRaw) * _GS_ExclusionBias;
        #if defined(EXCLUDE_NEAR_SNOW)
            depth01 = max(depth01, NEAR_DISTANCE_SNOW);
        #endif

		if (depthExclusion01 <= depth01) {
			discard;
			return;
		}

		float3 worldPos = GetWorldPosition(i.uv, depthRaw);

		float3 snowPos  = worldPos;

		half snowCover = 1.0;
        #if GLOBALSNOW_ZENITHAL_COVERAGE
		    float2 st = worldPos.xz - _GS_SnowCamPos.xz;
		    st = (st * COVERAGE_EXTENSION) + 0.5;
            half zmask = SAMPLE_TEXTURE2D(_GS_DepthTexture, sampler_LinearClamp, st).r;
			#if UNITY_REVERSED_Z
				zmask = 1.0 - zmask;
			#endif
			if (any(floor(st)!=0)) zmask = 1.0;
		    float y = max(_GS_SnowCamPos.y - worldPos.y, 0.001) / _GS_SnowCamPos.w;
		    float zd = min(zmask + GROUND_COVERAGE, y);
		    snowCover = saturate( ((zd / y) - 0.9875) * 110.0);
		#endif

        #if GLOBALSNOW_COVERAGE_MASK
            float2 maskUV = (worldPos.xz - _GS_DepthMaskWorldSize.yw) / _GS_DepthMaskWorldSize.xz + 0.5;
            half mask = SAMPLE_TEXTURE2D(_GS_DepthMask, sampler_LinearClamp, maskUV).r;
			if (any(floor(maskUV)!=0)) mask = _GS_FillOutSideMask;
            snowCover *= mask;
        #endif

		// diffuse
		#if GLOBALSNOW_FLAT_SHADING
			half4 diff = tex2D(_GS_SnowTex, snowPos.xz * 0.02);
		#else
			half4 diff = tex2D(_GS_SnowTex, snowPos.xz * 0.02).gggg;
		#endif

		// get world space normal
		float4 rawNormalData = SAMPLE_TEXTURE2D_X(_GS_GBuffer2Copy, sampler_PointClamp, i.uv);
		float3 wsNormal = DecodeSceneNormal(rawNormalData.xyz);

		// prevent snow on walls and below minimum altitude
		float altG = tex2D(_GS_SnowTex, snowPos.xz * 0.5).g;
		float altNoise = diff.r * altG - 0.9;
		float ny = wsNormal.y - SNOW_SLOPE_THRESHOLD;
		float flatSurface = saturate( (ny + altNoise * SNOW_SLOPE_NOISE) * SNOW_SLOPE_SHARPNESS);
		float minAltitude = worldPos.y - SNOW_ALTITUDE_MINIMUM - altNoise * SNOW_ALTITUDE_SCATTERING;
		minAltitude = saturate(minAltitude / SNOW_ALTITUDE_BLENDING);
		
		snowCover = snowCover * minAltitude * flatSurface - SNOW_REDUCTION;

		if (snowCover <= 0) {	
			discard;
			return;
		}
        
		const float snowHeight = SNOW_RELIEF; // Range: 0.001 .. 0.3 or will distort
		float3 rayDir = normalize(_WorldSpaceCameraPos - snowPos);

		// relief
		#if GLOBALSNOW_RELIEF || GLOBALSNOW_OCCLUSION
			float height = 0;
			if (snowCover > 0.1) {
				float snowHeight = SNOW_RELIEF; // Range: 0.001 .. 0.3 or will distort
				snowHeight *= (wsNormal.y - 0.5) * 2; // avoid relief mapping on steep surfaces
				// 3D surface mapping
				snowPos.y = 0;
				float3 snowPos0 = snowPos + float3(rayDir.x, max(0.3, rayDir.y), rayDir.z) * RELIEF_MAX_RAY_LENGTH;
				float3 rayStep = (snowPos - snowPos0) / (float)RELIEF_SAMPLE_COUNT; 
				// locate hit point
				UNITY_UNROLL
				for (int k=0;k<RELIEF_SAMPLE_COUNT;k++) {
					float h1 = tex2Dlod(_GS_NoiseTex, float4(snowPos0.xz * SNOW_TEXTURE_SCALE, 0, 0)).x * snowHeight;
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
					height = tex2Dlod(_GS_NoiseTex, float4(occ.xz * SNOW_TEXTURE_SCALE, 0, 0)).x;
					if (height * snowHeight > occ.y) {
						snowPos = occ;
					} else {
						snowPos0 = occ;
					}
				}
			}
			#if GLOBALSNOW_OCCLUSION
				// fake occlusion (optional)
				float height2 = tex2Dlod(_GS_NoiseTex, float4((snowPos.xz + (_GS_SunDir.xz) * 0.03) * SNOW_TEXTURE_SCALE,0,0)).x;
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

	        norm = lerp(mul(rot, wsNormal), norm.xzy, SNOW_THICKNESS * wsNormal.y);

			norm = lerp(wsNormal, norm, snowCover * SNOW_NORMALS_STRENGTH);

			norm = normalize(norm);

			// glitter (optional)
			float randomFacet = frac(norm.x * 97.13) - 0.975;
			float normalGloss = saturate(dot(-_GS_SunDir.xyz, norm));
			float glitter = saturate(randomFacet * 33.0) * normalGloss * SNOW_GLITTER;
			glitter *= 1.0 / (1.0 + dot2(_WorldSpaceCameraPos - snowPos) * 0.01);
			norm = lerp(norm, -_GS_SunDir.xyz, saturate(glitter));

		#endif


		#if GLOBALSNOW_TERRAINMARKS
			float drawDistSqr = dot2(worldPos - _WorldSpaceCameraPos);
			if (drawDistSqr < TERRAIN_MARKS_MAX_DISTANCE_SQR) {
				const float decalTexScale = 8.0 * _GS_DecalTex_TexelSize.x; // / 2048.0;
				const float texelScale = 0.1;
				const float2 offs  = float2(-0.5, 0.5);
				float2 uv00  = (snowPos.xz + offs.xx * texelScale) * decalTexScale; 
				float2 uv01  = (snowPos.xz + offs.xy * texelScale) * decalTexScale; 
				float2 uv10  = (snowPos.xz - offs.xy * texelScale) * decalTexScale; 
		        float4 decal = tex2D(_GS_DecalTex, uv00);
    		    float  h00   = decal.y;
				float  h01   = tex2D(_GS_DecalTex, uv01).y;
				float  h10   = tex2D(_GS_DecalTex, uv10).y;
				float  sh    = h00+h01+h10;
			    sh *= snowCover;
				if (sh>0) {
					#if !GLOBALSNOW_FLAT_SHADING
						float3 dduv00 = normalize(float3(offs.x, h00, offs.x));
					    float3 dduv01 = normalize(float3(offs.x, h01, offs.y));
					    float3 dduv10 = normalize(float3(offs.y, h10, offs.x));
					    float3 dduvA  = (dduv01 - dduv00);
					    float3 dduvB  = (dduv10 - dduv00);
					    float3 holeNorm = cross(dduvB, dduvA); 
					    holeNorm.y *= -0.5;
					    norm = lerp(norm, holeNorm, saturate( sh/1.75));
					    norm = normalize(norm) + 0.00001.xxx;
					#endif // !FLAT_SHADING

					diff.rgb -= sh / 38.0; // simulate self shadowing - the value of 38.0 can be decreased / increased to change obscurance
					#if GLOBALSNOW_OCCLUSION
					    diff.a = lerp(diff.a, diff.a * 0.5, sh/3.0);
					#endif
				}
			}
		#endif

		#if GLOBALSNOW_FOOTPRINTS
			snowPos.xz = lerp(snowPos.xz, worldPos.xz, 0.9);	// make footprint less flat
			snowPos.xz *= FOOTPRINT_SCALE;
			float4 dd = tex2D(_GS_FootprintTex, snowPos.xz / 2048.0);
	        dd.y *= snowCover;
			if (dd.y) {
				float2 dduv0 = frac(snowPos.xz) - 0.5.xx;
				float4 dduv  = float4(0,0,0,0);
				// rotate decal hit pos
				dduv.x = dot(dduv0, float2(dd.x, -dd.z));
				dduv.y = dot(dduv0, dd.zx);
				dduv.xy += 0.5.xx;
				float4 dt = tex2Dlod(_GS_DetailTex, dduv);
				if (dt.a) { // hole
					#if GLOBALSNOW_OCCLUSION
						diff.a *= 0.35; // soften occlusion inside plain hole
					#endif
					// 3D hole (optional)
					const int holeSamples = 5;
					const float deepness = 0.02; // (option)
					// rotate raydir				
					dduv0.x = dot(-rayDir.xz, float2(dd.x, -dd.z)); // dduv0.x * dd.x - dduv0.y * dd.z;
					dduv0.y = dot(-rayDir.xz, dd.zx); // dduv0.x * dd.z + dduv0.y * dd.x;
					float4 holeStep = float4(dduv0 * deepness, 0, 0);
					float3 holeNorm = float3(0.0,1.0,0.0);
					float holeDeep = 0.75; // (option)
					UNITY_UNROLL
					for (int k=0;k<holeSamples;k++) {
						dt = tex2Dlod(_GS_DetailTex, dduv);
						if (dt.a<=0) { // no hole
							float kh = (float)k / holeSamples;
							#if GLOBALSNOW_OCCLUSION
								diff.a += 0.075 * kh * dd.y;	// add occlusion around edges of hole
							#endif
							holeDeep *= kh;
							holeNorm = rayDir; // * 0.95;
							break;
						}
						dduv += holeStep;
					}
					diff.rgb *= 1.0 - dd.y * FOOTPRINT_OBSCURANCE;
					norm = normalize(lerp(norm, holeNorm, dd.y * holeDeep));
				}
 			}
 		#endif

		// output g-buffer values
		snowCover = saturate(snowCover);

		// Albedo
		half3 snowAlbedo = diff.rgb * _GS_SnowTint.rgb;
		#if GLOBALSNOW_OCCLUSION
			snowAlbedo *= saturate (1.0 - diff.a * SNOW_OCCLUSION);
		#endif
		snowAlbedo *= SNOW_BRIGHTNESS;

		outAlbedo = SAMPLE_TEXTURE2D_X_LOD(_GS_GBuffer0Copy, sampler_PointClamp, i.uv, 0);
		outAlbedo.rgb = lerp(outAlbedo.rgb, snowAlbedo, snowCover);
		
		// Ambient
		half3 ambientLighting = max(_GS_MinimumGIAmbient, SampleSH(norm));
		#if GLOBALSNOW_PRESERVE_GI
			outLighting = SAMPLE_TEXTURE2D_X_LOD(_GS_GBuffer3Copy, sampler_PointClamp, i.uv, 0);
			outLighting.rgb = lerp(outLighting.rgb, ambientLighting.ggg * snowAlbedo, _GS_SnowTint.a * snowCover);
		#else
			ambientLighting = min(ambientLighting, outAlbedo.g);
			outLighting.rgb = ambientLighting * outAlbedo.rgb;
			outLighting.a = 1;
		#endif

		// Specular
		outSpecularMetallic = SAMPLE_TEXTURE2D_X_LOD(_GS_GBuffer1Copy, sampler_PointClamp, i.uv, 0);
	    float gloss = saturate(0.65 + _GS_SunDir.y);
		half3 newSpecular = snowAlbedo * (gloss * SUN_OCCLUSION);
		outSpecularMetallic.rgb = lerp(outSpecularMetallic.rgb, newSpecular, snowCover);

		// Normal & smoothness
		#if !GLOBALSNOW_FLAT_SHADING
			norm = EncodeSceneNormal(norm);
			half smoothness = lerp(rawNormalData.w, SNOW_SMOOTHNESS * SUN_OCCLUSION, snowCover);
			outNormalSmoothness = half4(norm, smoothness);
		#endif

	}

#endif // GLOBALSNOW_DEFERRED_PASS