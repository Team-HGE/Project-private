Shader "Hidden/GlobalSnow/DepthBlur" {
Properties {
	_MainTex("Buffer (RGBA)", 2D) = "white" {}
}

SubShader {

	ZTest Always Cull Off ZWrite Off

	CGINCLUDE
	#include "UnityCG.cginc"

    struct appdata {
    	float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
    };


	struct v2fCross {
	    float4 pos : SV_POSITION;
	    float4 uv: TEXCOORD0;
	    float4 uv1: TEXCOORD1;
	    float4 uv2: TEXCOORD2;
	    float4 uv3: TEXCOORD3;
	    float4 uv4: TEXCOORD4;
		float2 xz: TEXCOORD5;
	};
	
	sampler2D _MainTex;
	float4 _MainTex_TexelSize;
	sampler2D _GS_NoiseTex;

	sampler2D _GS_DepthMask;
	float4 _GS_DepthMaskWorldSize;
	float _SnowCoverageSize;
	float3 _GS_SnowCamPos;
	float _GroundCoverageRandomization;
	float _BlurSpread;

	v2fCross vertBlurH(appdata v) {
    	v2fCross o;
		o.pos = UnityObjectToClipPos(v.vertex);
		#if UNITY_UV_STARTS_AT_TOP
    	if (_MainTex_TexelSize.y < 0) {
	        // Texture is inverted WRT the main texture
    	    v.texcoord.y = 1.0 - v.texcoord.y;
    	}
    	#endif     	
    	o.uv = float4(v.texcoord, 0, 0);
		float4 inc  = float4(_MainTex_TexelSize.x * 1.3846153846 * _BlurSpread, 0, 0, 0);	
    	o.uv1 = float4(v.texcoord - inc, 0, 0);	
    	o.uv2 = float4(v.texcoord + inc, 0, 0);	
		float4 inc2 = float4(_MainTex_TexelSize.x * 3.2307692308 * _BlurSpread, 0, 0, 0);	
    	o.uv3 = float4(v.texcoord - inc2, 0, 0);
    	o.uv4 = float4(v.texcoord + inc2, 0, 0);
		o.xz = (o.uv.xy - 0.5) * 2.0 * _SnowCoverageSize + _GS_SnowCamPos.xz;
		return o;
	}	
	
	v2fCross vertBlurV(appdata v) {
    	v2fCross o;
    	o.pos = UnityObjectToClipPos(v.vertex);
		#if UNITY_UV_STARTS_AT_TOP
    	if (_MainTex_TexelSize.y < 0) {
	        // Texture is inverted WRT the main texture
    	    v.texcoord.y = 1.0 - v.texcoord.y;
    	}
    	#endif
    	o.uv = float4(v.texcoord, 0, 0);
    	float4 inc  = float4(0, _MainTex_TexelSize.y * 1.3846153846 * _BlurSpread, 0, 0);	
    	o.uv1 = float4(v.texcoord - inc, 0, 0);
    	o.uv2 = float4(v.texcoord + inc, 0, 0);	
    	float4 inc2 = float4(0, _MainTex_TexelSize.y * 3.2307692308 * _BlurSpread, 0, 0);	
    	o.uv3 = float4(v.texcoord - inc2, 0, 0);	
    	o.uv4 = float4(v.texcoord + inc2, 0, 0);
		o.xz = 0;
    	return o;
	}

    #define DDEPTH(uv) tex2Dlod(_MainTex,uv).r

    half4 fragBlurWithRandomization (v2fCross i): SV_Target {
        half4 pixel0 = tex2Dlod(_MainTex, i.uv);
        half4 pixel = pixel0.r * 0.2270270270
                    + (DDEPTH(i.uv1) + DDEPTH(i.uv2)) * 0.3162162162
                    + (DDEPTH(i.uv3) + DDEPTH(i.uv4)) * 0.0702702703;

		float2 snowPos = i.xz * 0.05;
		half rnd = tex2D(_GS_NoiseTex, snowPos).r;
		rnd = max(0, rnd - 0.5) * _GroundCoverageRandomization * saturate(10 * fwidth(pixel0.r));
		#if UNITY_REVERSED_Z
			rnd = -rnd;
		#endif
		pixel.r -= rnd;

        return half4(pixel.r, 0, 0, 0);
    }   

	 half4 fragBlur (v2fCross i): SV_Target {
        half4 pixel0 = tex2Dlod(_MainTex, i.uv);
        half4 pixel = pixel0.r * 0.2270270270
                    + (DDEPTH(i.uv1) + DDEPTH(i.uv2)) * 0.3162162162
                    + (DDEPTH(i.uv3) + DDEPTH(i.uv4)) * 0.0702702703;
        return half4(pixel.r, pixel0.g, 0, 0);
    }   

	
	ENDCG

	Pass { 
		Name "Blur horizontally"
		CGPROGRAM
		#pragma vertex vertBlurH
		#pragma fragment fragBlurWithRandomization
		#pragma target 3.0
		ENDCG
	}
	
	Pass {
		Name "Blur vertically"
		CGPROGRAM
		#pragma vertex vertBlurV
		#pragma fragment fragBlur
		#pragma target 3.0
		ENDCG
	}

	
}

Fallback Off
}
