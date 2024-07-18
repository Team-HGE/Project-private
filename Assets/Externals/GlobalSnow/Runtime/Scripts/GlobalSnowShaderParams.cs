using UnityEngine;


namespace GlobalSnowEffect {

    public static partial class ShaderParams {

        // targets
        public static int MainTex = Shader.PropertyToID("_MainTex");
        public static int AlbedoTex = Shader.PropertyToID("_GS_GBuffer0Copy");
        public static int SpecularTex = Shader.PropertyToID("_GS_GBuffer1Copy");
        public static int NormalsTex = Shader.PropertyToID("_GS_GBuffer2Copy");
        public static int LightingTex = Shader.PropertyToID("_GS_GBuffer3Copy");
        public static int GlobalDepthTex = Shader.PropertyToID("_GS_DepthTexture");
        public static int ExclusionTex = Shader.PropertyToID("_GS_DeferredExclusionBuffer");
        public static int FrostedScreenTex = Shader.PropertyToID("_FrostedScreen");
        public static int MainTex1 = Shader.PropertyToID("_MainTex1");
        public static int MainTex2 = Shader.PropertyToID("_MainTex2");
        public static int MainTex3 = Shader.PropertyToID("_MainTex3");

        // textures
        public static int GlobalDecalTex = Shader.PropertyToID("_GS_DecalTex");
        public static int GlobalDepthMaskTexture = Shader.PropertyToID("_GS_DepthMask");
        public static int GlobalDetailTex = Shader.PropertyToID("_GS_DetailTex");
        public static int MaskTex = Shader.PropertyToID("_GS_MaskTexture");
        public static int GlobalSnowTex = Shader.PropertyToID("_GS_SnowTex");
        public static int GlobalNoiseTex = Shader.PropertyToID("_GS_NoiseTex");
        public static int GlobalSnowNormalsTex = Shader.PropertyToID("_GS_SnowNormalsTex");
        public static int GlobalFootprintTex = Shader.PropertyToID("_GS_FootprintTex");

        // uniforms
        public static int WorldPos = Shader.PropertyToID("_WorldPos");
        public static int TargetUV = Shader.PropertyToID("_TargetUV");
        public static int DrawDist = Shader.PropertyToID("_DrawDist");
        public static int EraseSpeed = Shader.PropertyToID("_EraseSpeed");
        public static int TargetCount = Shader.PropertyToID("_TargetCount");
        public static int TargetUVArray = Shader.PropertyToID("_TargetUVArray");
        public static int WorldPosArray = Shader.PropertyToID("_WorldPosArray");
        public static int GlobalSunDir = Shader.PropertyToID("_GS_SunDir");
        public static int GlobalSnowData1 = Shader.PropertyToID("_GS_SnowData1");
        public static int GlobalSnowData2 = Shader.PropertyToID("_GS_SnowData2");
        public static int GlobalSnowData3 = Shader.PropertyToID("_GS_SnowData3");
        public static int GlobalSnowData4 = Shader.PropertyToID("_GS_SnowData4");
        public static int GlobalSnowData5 = Shader.PropertyToID("_GS_SnowData5");
        public static int GlobalSnowData6 = Shader.PropertyToID("_GS_SnowData6");
        public static int GlobalSnowExclusionBias = Shader.PropertyToID("_GS_ExclusionBias");
        public static int GlobalMinimumGIAmbient = Shader.PropertyToID("_GS_MinimumGIAmbient");
        public static int GlobalSnowTint = Shader.PropertyToID("_GS_SnowTint");
        public static int Color = Shader.PropertyToID("_Color");
        public static int MaskCutOff = Shader.PropertyToID("_GS_MaskCutOff");
        public static int EraseCullMode = Shader.PropertyToID("_EraseCullMode");
        public static int GlobalCamPos = Shader.PropertyToID("_GS_SnowCamPos");
        public static int GlobalDepthMaskWorldSize = Shader.PropertyToID("_GS_DepthMaskWorldSize");
        public static int GlobalFillOutSideOfMask = Shader.PropertyToID("_GS_FillOutSideMask");
        public static int CoverageWorldSize = Shader.PropertyToID("_SnowCoverageSize");
        public static int GroundCoverageRandomization = Shader.PropertyToID("_GroundCoverageRandomization");
        public static int FrostIntensity = Shader.PropertyToID("_FrostIntensity");
        public static int FrostTintColor = Shader.PropertyToID("_FrostTintColor");
        public static int BlurSpread = Shader.PropertyToID("_BlurSpread");
        public static int MaskPaintData = Shader.PropertyToID("_MaskPaintData");
        public static int MaskPaintData2 = Shader.PropertyToID("_MaskPaintData2");

        // keywords
        public const string SKW_FLAT_SHADING = "GLOBALSNOW_FLAT_SHADING";
        public const string SKW_RELIEF = "GLOBALSNOW_RELIEF";
        public const string SKW_OCLUSSION = "GLOBALSNOW_OCCLUSION";
        public const string SKW_FOOTPRINTS = "GLOBALSNOW_FOOTPRINTS";
        public const string SKW_TERRAIN_MARKS = "GLOBALSNOW_TERRAINMARKS";
        public const string SKW_PRESERVE_GI = "GLOBALSNOW_PRESERVE_GI";
        public const string SKW_ZENITHAL_COVERAGE = "GLOBALSNOW_ZENITHAL_COVERAGE";
        public const string SKW_COVERAGE_MASK = "GLOBALSNOW_COVERAGE_MASK";

    }

}