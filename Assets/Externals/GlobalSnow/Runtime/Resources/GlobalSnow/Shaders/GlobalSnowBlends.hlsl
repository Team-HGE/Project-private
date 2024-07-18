#ifndef GLOBALSNOW_BLENDS
#define GLOBALSNOW_BLENDS
	
	half4 FragCopyExact (VaryingsSimple i): SV_Target {
    	UNITY_SETUP_INSTANCE_ID(i);
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
		i.uv     = UnityStereoTransformScreenSpaceTex(i.uv);
		half4 pixel = SAMPLE_TEXTURE2D_X(_MainTex, sampler_PointClamp, i.uv);
        return pixel;
	}

	struct FragMRTOutput
	{
		half4 gbuffer0 : SV_Target0;
		half4 gbuffer1 : SV_Target1;
		#if !GLOBALSNOW_FLAT_SHADING || GLOBALSNOW_PRESERVE_GI
			half4 gbuffer2 : SV_Target2;
			#if !GLOBALSNOW_FLAT_SHADING && GLOBALSNOW_PRESERVE_GI
				half4 gbuffer3 : SV_Target3;
			#endif
		#endif
	};

	FragMRTOutput FragCopyExactMRT (VaryingsSimple i)
	{
		UNITY_SETUP_INSTANCE_ID(i);
		UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
		i.uv     = UnityStereoTransformScreenSpaceTex(i.uv);
		FragMRTOutput result;
		result.gbuffer0 = SAMPLE_TEXTURE2D_X(_MainTex, sampler_PointClamp, i.uv);
		result.gbuffer1 = SAMPLE_TEXTURE2D_X(_MainTex1, sampler_PointClamp, i.uv);
		#if !GLOBALSNOW_FLAT_SHADING || GLOBALSNOW_PRESERVE_GI
			result.gbuffer2 = SAMPLE_TEXTURE2D_X(_MainTex2, sampler_PointClamp, i.uv);
			#if !GLOBALSNOW_FLAT_SHADING && GLOBALSNOW_PRESERVE_GI
				result.gbuffer3 = SAMPLE_TEXTURE2D_X(_MainTex3, sampler_PointClamp, i.uv);
			#endif
		#endif
		return result;
	}

#endif

	