#if UNITY_2023_3_OR_NEWER

using UnityEngine;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace GlobalSnowEffect {

    public static partial class ShaderParams {
        public static int CameraDepthTexture = Shader.PropertyToID("_CameraDepthTexture");
    }

    public partial class GlobalSnowRenderFeature : ScriptableRendererFeature {

        partial class SnowRenderPass : ScriptableRenderPass {

            class PassData {
                public SnowRenderPass snowPass;
                public TextureHandle rtAlbedo, rtSpecular, rtNormals, rtLighting, rtCameraDepthTexture;
                public TextureHandle rtAlbedoCopy, rtSpecularCopy, rtNormalsCopy, rtLightingCopy;
            }
            readonly PassData passData = new PassData();

            public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData) {

                UniversalResourceData resourceProvider = frameData.Get<UniversalResourceData>();
                UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();
                var desc = cameraData.cameraTargetDescriptor;

                // prepare copy of lighting texture
                TextureHandle rtLightingCopy = TextureHandle.nullHandle;
                if (snow.preserveGI) {
                    // prepare copy of lighting buffer
                    desc.depthBufferBits = 0;
                    rtLightingCopy = UniversalRenderer.CreateRenderGraphTexture(renderGraph, desc, "_GS_GBuffer3Copy", false);
                }

                // prepare copy of albedo texture
                ConfigureGraphicsFormat(ref desc, m_DeferredLights.GBufferAlbedoIndex);
                desc.depthBufferBits = 0;
                var rtAlbedoCopy = UniversalRenderer.CreateRenderGraphTexture(renderGraph, desc, "_GS_GBuffer0Copy", false);

                // prepare copy of specular texture
                ConfigureGraphicsFormat(ref desc, m_DeferredLights.GBufferSpecularMetallicIndex);
                desc.depthBufferBits = 0;
                var rtSpecularCopy = UniversalRenderer.CreateRenderGraphTexture(renderGraph, desc, "_GS_GBuffer1Copy", false);

                // prepare copy of normals texture
                TextureHandle rtNormalsCopy = TextureHandle.nullHandle;
                if (snow.snowQuality != SnowQuality.FlatShading) {
                    ConfigureGraphicsFormat(ref desc, m_DeferredLights.GBufferNormalSmoothnessIndex);
                    desc.depthBufferBits = 0;
                    rtNormalsCopy = UniversalRenderer.CreateRenderGraphTexture(renderGraph, desc, "_GS_GBuffer2Copy", false);
                }

                using (var builder = renderGraph.AddRasterRenderPass<PassData>("Global Snow Blit Pass RG", out var passData)) {

                    builder.AllowPassCulling(false);

                    passData.snowPass = this;

                    passData.rtAlbedo = resourceProvider.gBuffer[0];
                    if (passData.rtAlbedo.IsValid()) {
                        builder.UseTexture(passData.rtAlbedo);
                        builder.SetRenderAttachment(rtAlbedoCopy, 0, AccessFlags.WriteAll);
                        builder.SetGlobalTextureAfterPass(rtAlbedoCopy, ShaderParams.AlbedoTex);
                    }

                    passData.rtSpecular = resourceProvider.gBuffer[1];
                    if (passData.rtSpecular.IsValid()) {
                        builder.UseTexture(passData.rtSpecular);
                        builder.SetRenderAttachment(rtSpecularCopy, 1, AccessFlags.WriteAll);
                        builder.SetGlobalTextureAfterPass(rtSpecularCopy, ShaderParams.SpecularTex);
                    }

                    bool useNormals = snow.snowQuality != SnowQuality.FlatShading;
                    if (useNormals) {
                        passData.rtNormals = resourceProvider.gBuffer[2];
                        if (passData.rtNormals.IsValid()) {
                            builder.UseTexture(passData.rtNormals);
                            builder.SetRenderAttachment(rtNormalsCopy, 2, AccessFlags.WriteAll);
                            builder.SetGlobalTextureAfterPass(rtNormalsCopy, ShaderParams.NormalsTex);
                        }
                    }

                    if (snow.preserveGI) {
                        passData.rtLighting = resourceProvider.gBuffer[3];
                        if (passData.rtLighting.IsValid()) {
                            builder.UseTexture(passData.rtLighting);
                            builder.SetRenderAttachment(rtLightingCopy, useNormals ? 3: 2, AccessFlags.WriteAll);
                            builder.SetGlobalTextureAfterPass(rtLightingCopy, ShaderParams.LightingTex);
                        }
                    }

                    passData.rtCameraDepthTexture = resourceProvider.gBuffer[4];
                    builder.UseTexture(passData.rtCameraDepthTexture, AccessFlags.Read);

                    builder.SetRenderFunc((PassData passData, RasterGraphContext context) => {
                        passData.snowPass.mat.SetTexture(ShaderParams.MainTex, passData.rtAlbedo);
                        passData.snowPass.mat.SetTexture(ShaderParams.MainTex1, passData.rtSpecular);
                        if (passData.rtNormals.IsValid()) {
                            passData.snowPass.mat.SetTexture(ShaderParams.MainTex2, passData.rtNormals);
                        }
                        if (passData.rtLighting.IsValid()) {
                            passData.snowPass.mat.SetTexture(ShaderParams.MainTex3, passData.rtLighting);
                        }
                        context.cmd.DrawMesh(MeshUtils.fullscreenMesh, Matrix4x4.identity, passData.snowPass.mat, 0, (int)Pass.CopyExactMRT);
                    });
                }

                using (var builder = renderGraph.AddRasterRenderPass<PassData>("Global Snow Draw Pass RG", out var passData)) {

                    builder.AllowPassCulling(false);
                    passData.snowPass = this;

                    passData.rtAlbedoCopy = rtAlbedoCopy;
                    builder.UseTexture(passData.rtAlbedoCopy, AccessFlags.Read);
                    builder.SetRenderAttachment(resourceProvider.gBuffer[0], 0, AccessFlags.Write);

                    passData.rtSpecularCopy = rtSpecularCopy;
                    builder.UseTexture(passData.rtSpecularCopy, AccessFlags.Read);
                    builder.SetRenderAttachment(resourceProvider.gBuffer[1], 1, AccessFlags.Write);

                    if (snow.snowQuality != SnowQuality.FlatShading) {
                        passData.rtNormalsCopy = rtNormalsCopy;
                        builder.UseTexture(passData.rtNormalsCopy, AccessFlags.Read);
                        builder.SetRenderAttachment(resourceProvider.gBuffer[2], 2, AccessFlags.Write);
                    }

                    if (snow.preserveGI) {
                        passData.rtLightingCopy = rtLightingCopy;
                        builder.UseTexture(passData.rtLightingCopy, AccessFlags.Read);
                    }
                    builder.SetRenderAttachment(resourceProvider.gBuffer[3], 3, AccessFlags.Write);

                    passData.rtCameraDepthTexture = resourceProvider.gBuffer[4];
                    builder.UseTexture(passData.rtCameraDepthTexture);

                    builder.SetRenderFunc((PassData passData, RasterGraphContext context) => {
                        GlobalSnow snow = passData.snowPass.snow;
                        Material mat = passData.snowPass.mat;

                        mat.SetTexture(ShaderParams.CameraDepthTexture, passData.rtCameraDepthTexture);
                        mat.SetTexture(ShaderParams.AlbedoTex, passData.rtAlbedoCopy);
                        mat.SetTexture(ShaderParams.SpecularTex, passData.rtSpecularCopy);

                        if (snow.snowQuality != SnowQuality.FlatShading) {
                            mat.SetTexture(ShaderParams.NormalsTex, passData.rtNormalsCopy);
                        }

                        if (snow.preserveGI) {
                            mat.SetTexture(ShaderParams.LightingTex, passData.rtLightingCopy);
                        }

                        // render snow into gbuffers
                        context.cmd.DrawMesh(MeshUtils.fullscreenMesh, Matrix4x4.identity, mat, 0, (int)Pass.SnowDeferred);
                    });
                }
            }
        }

        partial class FrostRenderPass : ScriptableRenderPass {

            class PassData {
                public TextureHandle src;
                public TextureHandle bbColor;
                public FrostRenderPass frostPass;
            }
            readonly PassData passData = new PassData();

            public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData) {

                using (var builder = renderGraph.AddUnsafePass<PassData>("Global Snow RG Camera Frost", out var passData)) {

                    UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();
                    sourceDesc = cameraData.cameraTargetDescriptor;

                    UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
                    passData.src = resourceData.activeColorTexture;
                    passData.bbColor = resourceData.backBufferColor;
                    passData.frostPass = this;

                    builder.UseTexture(resourceData.activeColorTexture, AccessFlags.ReadWrite);

                    builder.SetRenderFunc((PassData passData, UnsafeGraphContext context) => {

                        GlobalSnow snow = passData.frostPass.snow;
                        Material mat = passData.frostPass.mat;

                        if (snow == null || mat == null) return;

                        bool frosted = snow.snowAmount > 0;
                        mat.SetVector(ShaderParams.FrostIntensity, new Vector4(frosted ? snow.cameraFrostIntensity * snow.snowAmount * 5f : 0, 5.1f - snow.cameraFrostSpread, snow.cameraFrostDistortion * 0.01f, 0));
                        mat.SetColor(ShaderParams.FrostTintColor, snow.cameraFrostTintColor);

                        CommandBuffer cmd = CommandBufferHelpers.GetNativeCommandBuffer(context.cmd);
                        cmd.GetTemporaryRT(ShaderParams.FrostedScreenTex, passData.frostPass.sourceDesc);
                        FullScreenBlit(cmd, passData.src, ShaderParams.FrostedScreenTex, mat, Pass.FrostEffect);
                        FullScreenBlit(cmd, ShaderParams.FrostedScreenTex, passData.src, mat, Pass.CopyExact);
                        cmd.SetRenderTarget(passData.bbColor); // Temporary fix
                    });
                }
            }


        }


    }

}

#endif