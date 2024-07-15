    uniform float4    _GS_SnowData1;	// x = relief, y = occlusion, z = glitter, w = brightness
	uniform float4    _GS_SnowData2;	// x = minimum altitude, y = altitude scattering, z = coverage extension
	uniform float4    _GS_SnowData3;	// x = Sun occlusion, y = sun atten, z = ground coverage, w = grass coverage
    uniform float4    _GS_SnowCamPos;
    uniform sampler2D _GS_DepthTexture;

    uniform sampler2D _GS_DepthMask;
    uniform float4 _GS_DepthMaskWorldSize;

	// get snow coverage on grass
	half3 AddSnowCoverage(half3 color, float3 worldPos, float2 uv) { 
		// prevent snow on sides and below minimum altitude
		float minAltitude = saturate( worldPos.y - _GS_SnowData2.x);
		float snowCover   = minAltitude * saturate(uv.y + _GS_SnowData3.w);

        // zenithal coverage support; commented out since grass usually doesn't need it
/*        #if GLOBALSNOW_ZENITHAL_COVERAGE
            float4 st = float4(worldPos.xz - _GS_SnowCamPos.xz, 0, 0);
            st = (st * _GS_SnowData2.z) + 0.5;
			float zmask = SAMPLE_TEXTURE2D(_GS_DepthTexture, sampler_LinearClamp, st).r;
			#if UNITY_REVERSED_Z
				zmask = 1.0 - zmask;
			#endif
			if (any(floor(st)!=0)) zmask = 1.0;
		    float y = max(_GS_SnowCamPos.y - worldPos.y, 0.001) / _GS_SnowCamPos.w;
		    float zd = min(zmask + GROUND_COVERAGE, y);
		    snowCover *= saturate( ((zd / y) - 0.9875) * 110.0);
		  #endif
 */

        #if GLOBALSNOW_COVERAGE_MASK
            float2 maskUV = (worldPos.xz - _GS_DepthMaskWorldSize.yw) / _GS_DepthMaskWorldSize.xz + 0.5.xx;
            half mask = tex2D(_GS_DepthMask, maskUV).r;
			if (any(floor(maskUV)!=0)) mask = 1.0;
            snowCover *= mask;
        #endif

        color = lerp(color, _GS_SnowData1.www, snowCover * 0.96);
        return color;
	}
	
	
	
