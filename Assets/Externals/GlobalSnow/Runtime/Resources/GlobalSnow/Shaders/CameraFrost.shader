Shader "GlobalSnow/CameraFrost" {
Properties {
	_FrostTex ("Frost RGBA", 2D) = "white" {}
	_FrostNormals ("Frost Normals RGBA", 2D) = "bump" {}
	_FrostIntensity ("Frost Data", Vector) = (1,5,0)
}
SubShader
{
    ZWrite Off ZTest Always Blend Off Cull Off

    HLSLINCLUDE
    #pragma target 3.0
    #pragma prefer_hlslcc gles
    #pragma exclude_renderers d3d11_9x
    
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Filtering.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/Shaders/PostProcessing/Common.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareNormalsTexture.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityGBuffer.hlsl"
    #include "GlobalSnowOptions.hlsl"
    #include "GlobalSnowCommon.hlsl"
    ENDHLSL

  Pass { // 0
      Name "Camera Frost Effect"
      HLSLPROGRAM
      #pragma vertex VertSimple
      #pragma fragment FragFrost
      #include "CameraFrostPass.hlsl"
      ENDHLSL
  }

  Pass { // 1
      Name "Copy Exact"
      HLSLPROGRAM
      #pragma vertex VertSimple
      #pragma fragment FragCopyExact
      #include "CameraFrostPass.hlsl"
      ENDHLSL
  }

}
}


	