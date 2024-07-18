Shader "Hidden/GlobalSnow"
{
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
      Name "Copy Exact"
      HLSLPROGRAM
      #pragma vertex VertSimple
      #pragma fragment FragCopyExact
      #include "GlobalSnowBlends.hlsl"
      ENDHLSL
  }

  Pass { // 1
      Name "Snow Deferred Pass"
      HLSLPROGRAM
      #pragma vertex VertSimple
      #pragma fragment FragSnowDeferred
      #pragma multi_compile_fragment _ GLOBALSNOW_ZENITHAL_COVERAGE
      #pragma multi_compile_fragment _ GLOBALSNOW_COVERAGE_MASK
	  #pragma multi_compile_fragment _ GLOBALSNOW_FLAT_SHADING GLOBALSNOW_RELIEF GLOBALSNOW_OCCLUSION
	  #pragma multi_compile_fragment _ GLOBALSNOW_FOOTPRINTS
	  #pragma multi_compile_fragment _ GLOBALSNOW_TERRAINMARKS
      #pragma multi_compile_fragment _ GLOBALSNOW_PRESERVE_GI
      #pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT
      #include "GlobalSnowPass.hlsl"
      ENDHLSL
  }

  Pass { // 2
      Name "Copy Exact MRT"
      HLSLPROGRAM
      #pragma vertex VertSimple
      #pragma fragment FragCopyExactMRT
      #pragma multi_compile_fragment _ GLOBALSNOW_FLAT_SHADING
      #pragma multi_compile_fragment _ GLOBALSNOW_PRESERVE_GI
      #include "GlobalSnowBlends.hlsl"
      ENDHLSL
  }
    
}
}


	