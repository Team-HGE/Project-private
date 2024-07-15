#ifndef GLOBALSNOW_COMMON
#define GLOBALSNOW_COMMON

    struct AttributesSimple {
        float4 positionOS : POSITION;
        float2 uv : TEXCOORD0;
        UNITY_VERTEX_INPUT_INSTANCE_ID
    };

	struct VaryingsSimple {
	    float4 positionCS : SV_POSITION;
	    float2 uv: TEXCOORD0;
        UNITY_VERTEX_INPUT_INSTANCE_ID
        UNITY_VERTEX_OUTPUT_STEREO
	};

    TEXTURE2D_X(_MainTex);
    SAMPLER(sampler_MainTex);
    TEXTURE2D_X(_MainTex1);
    SAMPLER(sampler_MainTex1);
    TEXTURE2D_X(_MainTex2);
    SAMPLER(sampler_MainTex2);
    TEXTURE2D_X(_MainTex3);
    SAMPLER(sampler_MainTex3);

	VaryingsSimple VertSimple(AttributesSimple v) {
    	VaryingsSimple o;
        UNITY_SETUP_INSTANCE_ID(v);
        UNITY_TRANSFER_INSTANCE_ID(v, o);
        UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

	    o.positionCS = v.positionOS;
		o.positionCS.y *= _ProjectionParams.x;
    	o.uv = v.uv;

    	return o;
	}


    float GetRawDepth(float2 uv) {
        float depth = SAMPLE_TEXTURE2D_X_LOD(_CameraDepthTexture, sampler_PointClamp, uv, 0).r;
        return depth;
    }

    float RawToLinearEyeDepth(float rawDepth) {
        float eyeDepth = LinearEyeDepth(rawDepth, _ZBufferParams);
        #if _ORTHO_SUPPORT
            #if UNITY_REVERSED_Z
                rawDepth = 1.0 - rawDepth;
            #endif
            float orthoEyeDepth = lerp(_ProjectionParams.y, _ProjectionParams.z, rawDepth);
            eyeDepth = lerp(eyeDepth, orthoEyeDepth, unity_OrthoParams.w);
        #endif
        return eyeDepth;
    }

    float GetLinearEyeDepth(float2 uv) {
        float rawDepth = GetRawDepth(uv);
        return RawToLinearEyeDepth(rawDepth);
    }
        
    float RawTo01Depth(float rawDepth) {
        float depth01 = Linear01Depth(rawDepth, _ZBufferParams);
        #if _ORTHO_SUPPORT
            #if UNITY_REVERSED_Z
                rawDepth = 1.0 - rawDepth;
            #endif
            float orthoDepth01 = lerp(_ProjectionParams.y, _ProjectionParams.z, rawDepth);
            depth01 = lerp(depth01, orthoDepth01, unity_OrthoParams.w);
        #endif
        return depth01;
    }

    float Get01Depth(float2 uv) {
        float rawDepth = GetRawDepth(uv);
        return RawTo01Depth(rawDepth);
    }


    float3 GetWorldPosition(float2 uv, float rawDepth) {
        
         #if UNITY_REVERSED_Z
              float depth = rawDepth;
         #else
              float depth = lerp(UNITY_NEAR_CLIP_VALUE, 1, rawDepth);
         #endif

         // Reconstruct the world space positions.
         float3 worldPos = ComputeWorldSpacePosition(uv, depth, UNITY_MATRIX_I_VP);

        return worldPos;
    }

    float3 GetWorldPosition(float2 uv) {
        float rawDepth = GetRawDepth(uv);
        return GetWorldPosition(uv, rawDepth);
    }



#endif // GLOBALSNOW_COMMON
	