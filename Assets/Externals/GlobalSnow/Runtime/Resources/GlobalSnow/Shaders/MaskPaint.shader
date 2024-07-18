Shader "GlobalSnow/MaskPaint" {
Properties {
	_MainTex ("Main RGBA", 2D) = "black" {}
}

SubShader {
   	ZTest Always Cull Off ZWrite Off
	Blend SrcAlpha OneMinusSrcAlpha

	CGINCLUDE
	#include "UnityCG.cginc"
    
	sampler2D_float _MainTex;

	float4 _MaskPaintData;
	#define TARGET_UV _MaskPaintData.xy
	#define RADIUS_SQR _MaskPaintData.z
	#define OPACITY _MaskPaintData.w

	float3 _MaskPaintData2;
	#define VALUE _MaskPaintData2.x
	#define FUZZINESS _MaskPaintData2.y
	#define SEED _MaskPaintData2.z

	struct v2f {
	    float4 pos : SV_POSITION;
	    float2 uv: TEXCOORD0;
	};
	
	v2f vert( appdata_base v ) {
	    v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
    	o.uv = v.texcoord;
    	return o;
	}

	float GetRandom (float2 uv) {
		return frac((sin(dot(uv,float2(12.9898,78.233))) + SEED) *43758.5453123);
	}

	half4 fragPaint(v2f i) : SV_Target {
		float d = dot(i.uv - TARGET_UV, i.uv - TARGET_UV);

		float op = d / RADIUS_SQR;
		if (op <= 1.0) {
			float threshold = GetRandom(i.uv);
			if (threshold * op < 1.1 - FUZZINESS) {
				return half4(VALUE, VALUE, VALUE, OPACITY);
			}
		}
		return 0;
	}

		
	ENDCG
	
	Pass {
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment fragPaint
		ENDCG
	}

}
}
