#ifndef GLOBALSNOW_FROST_PASS
#define GLOBALSNOW_FROST_PASS

	sampler2D _FrostTex;
	sampler2D _FrostNormals;
	float3 _FrostIntensity;
	half4 _FrostTintColor;

	half4 FragFrost(VaryingsSimple i) : SV_Target {

	    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
		float2 uv  = UnityStereoTransformScreenSpaceTex(i.uv);

		float2 da = uv - UnityStereoTransformScreenSpaceTex(0.5.xx);
		float dd = dot(da, da) * 2.0;
		dd = saturate(pow(dd, _FrostIntensity.y)) * _FrostIntensity.x;
		half4 frost = half4(0,0,0,0);
		if (_FrostIntensity.x>0) {
			frost = tex2Dlod(_FrostTex, float4(uv, 0, 0));
			#if UNITY_COLORSPACE_GAMMA
				frost.rgb = SRGBToLinear(frost.rgb);
			#endif
			half4 norm = tex2Dlod(_FrostNormals, float4(uv, 0, 0));
			norm.rgb = UnpackNormal(norm);
			float2 disp = norm.xy * _FrostIntensity.z * dd;
			uv.xy += disp;
		}

		half4 pixel = SAMPLE_TEXTURE2D_X(_MainTex, sampler_PointClamp, uv);

        frost.rgb *= dd;
        pixel.rgb = frost.rgb * _FrostTintColor.rgb + pixel.rgb * (1.0 - frost.g);

		return pixel; 
	}

	half4 FragCopyExact(VaryingsSimple i) : SV_Target {

	    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
		float2 uv  = UnityStereoTransformScreenSpaceTex(i.uv);

		half4 pixel = SAMPLE_TEXTURE2D_X(_MainTex, sampler_PointClamp, uv);
		return pixel;

	}


#endif // GLOBALSNOW_FROST_PASS