using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DeferredLights = UnityEngine.Rendering.Universal.Internal.DeferredLights;

namespace GlobalSnowEffect {

    public partial class GlobalSnowRenderFeature : ScriptableRendererFeature {

        partial class SnowRenderPass : ScriptableRenderPass {

            enum Pass {
                CopyExact = 0,
                SnowDeferred = 1,
                CopyExactMRT = 2
            }

            Material mat;
            UniversalRenderer renderer;
            DeferredLights m_DeferredLights;

#if UNITY_2022_1_OR_NEWER
            RTHandle[] gbufferHandles;
#endif
            static RenderTargetIdentifier[] gbufferIdentifiers;

            GlobalSnow snow;


            public bool Setup(GlobalSnow snow, ScriptableRenderer renderer) {
                this.snow = snow;
                this.renderer = (UniversalRenderer)renderer;

                DeferredLights deferredLights = this.renderer.deferredLights;
                if (deferredLights == null) return false;
                m_DeferredLights = deferredLights;

                if (mat == null) {
                    mat = CoreUtils.CreateEngineMaterial(Shader.Find("Hidden/GlobalSnow"));
                }

                return true;
            }



#if UNITY_2022_1_OR_NEWER

            RTHandle GetAlbedoFromGbuffer() {
                return m_DeferredLights.GbufferAttachments[m_DeferredLights.GBufferAlbedoIndex];
            }

            RTHandle GetSpecularFromGbuffer() {
                return m_DeferredLights.GbufferAttachments[m_DeferredLights.GBufferSpecularMetallicIndex];
            }

            RTHandle GetNormalsFromGbuffer() {
                return m_DeferredLights.GbufferAttachments[m_DeferredLights.GBufferNormalSmoothnessIndex];
            }

            RTHandle GetLightingFromGbuffer() {
                return m_DeferredLights.GbufferAttachments[m_DeferredLights.GBufferLightingIndex];
            }

#else

            RenderTargetIdentifier GetAlbedoFromGbuffer() {
                return m_DeferredLights.GbufferAttachmentIdentifiers[m_DeferredLights.GBufferAlbedoIndex];
            }

            RenderTargetIdentifier GetSpecularFromGbuffer() {
                return m_DeferredLights.GbufferAttachmentIdentifiers[m_DeferredLights.GBufferSpecularMetallicIndex];
            }

            RenderTargetIdentifier GetNormalsFromGbuffer() {
                return m_DeferredLights.GbufferAttachmentIdentifiers[m_DeferredLights.GBufferNormalSmoothnessIndex];
            }

            RenderTargetIdentifier GetLightingFromGbuffer() {
                return m_DeferredLights.GbufferAttachmentIdentifiers[m_DeferredLights.GBufferLightingIndex];
            }
#endif

            void ConfigureGraphicsFormat(ref RenderTextureDescriptor desc, int bufferIndex) {
                desc.graphicsFormat = m_DeferredLights.GetGBufferFormat(bufferIndex);
            }


#if UNITY_2023_2_OR_NEWER
            [Obsolete]
#endif
            public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor) {


#if UNITY_2022_1_OR_NEWER
                bool flatShading = snow.snowQuality == SnowQuality.FlatShading;
                if (flatShading) {
                    if (gbufferHandles == null || gbufferHandles.Length != 3) {
                        gbufferHandles = new RTHandle[3];
                    }
                    gbufferHandles[0] = GetAlbedoFromGbuffer();
                    gbufferHandles[1] = GetSpecularFromGbuffer();
                    gbufferHandles[2] = GetLightingFromGbuffer();
                } else {
                    if (gbufferHandles == null || gbufferHandles.Length != 4) {
                        gbufferHandles = new RTHandle[4];
                    }
                    gbufferHandles[0] = GetAlbedoFromGbuffer();
                    gbufferHandles[1] = GetSpecularFromGbuffer();
                    gbufferHandles[2] = GetNormalsFromGbuffer();
                    gbufferHandles[3] = GetLightingFromGbuffer();
                }
                int bufferCount = gbufferHandles.Length;
                if (gbufferIdentifiers == null || gbufferIdentifiers.Length != bufferCount) {
                    gbufferIdentifiers = new RenderTargetIdentifier[bufferCount];
                }
                for (int k = 0; k < bufferCount; k++) {
                    gbufferIdentifiers[k] = new RenderTargetIdentifier(gbufferHandles[k].nameID, 0, CubemapFace.Unknown, -1);
                }

                ConfigureTarget(gbufferHandles, m_DeferredLights.DepthAttachmentHandle);
#else
                bool flatShading = snow.snowQuality == SnowQuality.FlatShading;
                if (flatShading) {
                    if (gbufferIdentifiers == null || gbufferIdentifiers.Length != 3) {
                        gbufferIdentifiers = new RenderTargetIdentifier[3];
                    }
                    gbufferIdentifiers[0] = GetAlbedoFromGbuffer();
                    gbufferIdentifiers[1] = GetSpecularFromGbuffer();
                    gbufferIdentifiers[2] = GetLightingFromGbuffer();
                } else {
                    if (gbufferIdentifiers == null || gbufferIdentifiers.Length != 4) {
                        gbufferIdentifiers = new RenderTargetIdentifier[4];
                    }
                    gbufferIdentifiers[0] = GetAlbedoFromGbuffer();
                    gbufferIdentifiers[1] = GetSpecularFromGbuffer();
                    gbufferIdentifiers[2] = GetNormalsFromGbuffer();
                    gbufferIdentifiers[3] = GetLightingFromGbuffer();
                }
                ConfigureTarget(gbufferIdentifiers, m_DeferredLights.DepthAttachmentIdentifier);
#endif
            }

#if UNITY_2023_2_OR_NEWER
            [Obsolete]
#endif
            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData) {

#if !UNITY_2023_3_OR_NEWER
                CommandBuffer cbufExcluded = snow.GetExclusionCommandBuffer();
                if (cbufExcluded != null) {
                    context.ExecuteCommandBuffer(cbufExcluded);
                }
#endif

                CommandBuffer cmd = CommandBufferPool.Get("Global Snow");
                cmd.Clear();

                // copies albedo, specular, normals and lighting gbuffers
                RenderTextureDescriptor desc = renderingData.cameraData.cameraTargetDescriptor;
                desc.stencilFormat = UnityEngine.Experimental.Rendering.GraphicsFormat.None;
                desc.depthBufferBits = 0;

                if (snow.preserveGI) {
                    if (desc.graphicsFormat == UnityEngine.Experimental.Rendering.GraphicsFormat.None) {
                        // fallback to use same graphics format than albedo
                        ConfigureGraphicsFormat(ref desc, m_DeferredLights.GBufferAlbedoIndex);
                    }
                    cmd.GetTemporaryRT(ShaderParams.LightingTex, desc);
                    FullScreenBlit(cmd, GetLightingFromGbuffer(), ShaderParams.LightingTex, mat, Pass.CopyExact);
                }

                ConfigureGraphicsFormat(ref desc, m_DeferredLights.GBufferAlbedoIndex);
                cmd.GetTemporaryRT(ShaderParams.AlbedoTex, desc);
                FullScreenBlit(cmd, GetAlbedoFromGbuffer(), ShaderParams.AlbedoTex, mat, Pass.CopyExact);

                ConfigureGraphicsFormat(ref desc, m_DeferredLights.GBufferSpecularMetallicIndex);
                cmd.GetTemporaryRT(ShaderParams.SpecularTex, desc);
                FullScreenBlit(cmd, GetSpecularFromGbuffer(), ShaderParams.SpecularTex, mat, Pass.CopyExact);

                if (snow.snowQuality != SnowQuality.FlatShading) {
                    ConfigureGraphicsFormat(ref desc, m_DeferredLights.GBufferNormalSmoothnessIndex);
                    cmd.GetTemporaryRT(ShaderParams.NormalsTex, desc);
                    FullScreenBlit(cmd, GetNormalsFromGbuffer(), ShaderParams.NormalsTex, mat, Pass.CopyExact);
                }

                // render snow into gbuffers
#if UNITY_2022_1_OR_NEWER
                cmd.SetRenderTarget(gbufferIdentifiers, m_DeferredLights.DepthAttachmentHandle);
#else
                cmd.SetRenderTarget(gbufferIdentifiers, m_DeferredLights.DepthAttachmentIdentifier);
#endif
                cmd.DrawMesh(MeshUtils.fullscreenMesh, Matrix4x4.identity, mat, 0, (int)Pass.SnowDeferred);

                context.ExecuteCommandBuffer(cmd);
                CommandBufferPool.Release(cmd);
            }

            static void FullScreenBlit(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, Material material, Pass pass) {
                destination = new RenderTargetIdentifier(destination, 0, CubemapFace.Unknown, -1);
                cmd.SetRenderTarget(destination);
                cmd.SetGlobalTexture(ShaderParams.MainTex, source);
                cmd.DrawMesh(MeshUtils.fullscreenMesh, Matrix4x4.identity, material, 0, (int)pass);
            }

        }

        partial class FrostRenderPass : ScriptableRenderPass {

            Material mat;
            UniversalRenderer renderer;
            RenderTextureDescriptor sourceDesc;
            GlobalSnow snow;

            enum Pass {
                FrostEffect = 0,
                CopyExact = 1
            }

            public bool Setup(GlobalSnow snow, ScriptableRenderer renderer) {
                if (!snow.cameraFrost) return false;
                this.snow = snow;
                this.renderer = (UniversalRenderer)renderer;

                if (mat == null) {
                    mat = Instantiate(Resources.Load<Material>("GlobalSnow/Materials/ScreenSpaceCameraFrost"));
                }

                return true;
            }

#if UNITY_2023_2_OR_NEWER
            [Obsolete]
#endif
            public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor) {
                sourceDesc = cameraTextureDescriptor;
                ConfigureInput(ScriptableRenderPassInput.Color);
            }

#if UNITY_2023_2_OR_NEWER
            [Obsolete]
#endif
            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData) {

            	GlobalSnow snow = GlobalSnow.instance;
                Camera cam = renderingData.cameraData.camera;
                if (snow == null || mat == null || (!snow.showSnowInSceneView && cam.cameraType == CameraType.SceneView)) {
                    return;
                }

                bool frosted = snow.snowAmount > 0;

                CommandBuffer cmd = CommandBufferPool.Get("Global Snow Frost Effect");
                cmd.Clear();

                mat.SetVector(ShaderParams.FrostIntensity, new Vector4(frosted ? snow.cameraFrostIntensity * snow.snowAmount * 5f : 0, 5.1f - snow.cameraFrostSpread, snow.cameraFrostDistortion * 0.01f, 0));
                mat.SetColor(ShaderParams.FrostTintColor, snow.cameraFrostTintColor);

                cmd.GetTemporaryRT(ShaderParams.FrostedScreenTex, sourceDesc);

#if UNITY_2022_2_OR_NEWER
                RTHandle source = renderer.cameraColorTargetHandle;
#else
                RenderTargetIdentifier source = renderer.cameraColorTarget;
#endif

                FullScreenBlit(cmd, source, ShaderParams.FrostedScreenTex, mat, Pass.FrostEffect);
                FullScreenBlit(cmd, ShaderParams.FrostedScreenTex, source, mat, Pass.CopyExact);

                context.ExecuteCommandBuffer(cmd);
                CommandBufferPool.Release(cmd);
            }


            static void FullScreenBlit(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, Material material, Pass pass) {
                destination = new RenderTargetIdentifier(destination, 0, CubemapFace.Unknown, -1);
                cmd.SetRenderTarget(destination);
                cmd.SetGlobalTexture(ShaderParams.MainTex, source);
                cmd.DrawMesh(MeshUtils. fullscreenMesh, Matrix4x4.identity, material, 0, (int)pass);
            }
        }


        [Tooltip("Which cameras can render Global Snow effect")]
        public LayerMask camerasLayerMask = -1;

        SnowRenderPass snowPass;
        FrostRenderPass frostPass;

        public static bool installed;
        public static RenderingMode renderingMode;
        public static RenderTextureDescriptor mainCameraDescriptor;

        void OnDisable() {
            installed = false;
        }



        public override void Create() {
            snowPass = new SnowRenderPass();
            snowPass.renderPassEvent = RenderPassEvent.AfterRenderingGbuffer;
            frostPass = new FrostRenderPass();
            frostPass.renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData) {
            installed = true;

#if UNITY_2022_1_OR_NEWER
            renderingMode = ((UniversalRenderer)renderer).renderingModeActual;
#else
            renderingMode = ((UniversalRenderer)renderer).actualRenderingMode;
#endif
            if (renderingMode != RenderingMode.Deferred) return;

            GlobalSnow snow = GlobalSnow.instance;
            if (snow == null || !snow.isActiveAndEnabled) return;

            Camera cam = renderingData.cameraData.camera;
            CameraType camType = cam.cameraType;

#if UNITY_EDITOR
            // some editor cameras are wrongly reported as "Game" camera so we have to verify camera name
            if (camType == CameraType.SceneView && (!snow.showSnowInSceneView || !"SceneCamera".Equals(cam.name))) return;
#endif

            if (camType != CameraType.SceneView && camType != CameraType.Game && camType != CameraType.Reflection) return;

            if (renderingData.cameraData.renderType != CameraRenderType.Base) return;

            if ((camerasLayerMask & (1 << cam.gameObject.layer)) == 0) return;

            if (camType == CameraType.Game) {
                mainCameraDescriptor = renderingData.cameraData.cameraTargetDescriptor;
            }

            if (snowPass.Setup(snow, renderer)) {
                renderer.EnqueuePass(snowPass);
            }
            if (frostPass.Setup(snow, renderer)) {
                renderer.EnqueuePass(frostPass);
            }
        }

    }

}

