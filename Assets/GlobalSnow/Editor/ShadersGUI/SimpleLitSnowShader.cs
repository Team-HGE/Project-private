using System;
using UnityEngine;

namespace UnityEditor.Rendering.Universal.ShaderGUI
{
    internal class SimpleLitSnowShader : BaseShaderGUI
    {
        // Properties
        private SimpleLitGUI.SimpleLitProperties shadingModelProperties;

        MaterialProperty snowTint, snowCoverage, snowScale;
        MaterialProperty scatter, slopeThreshold, slopeNoise, slopeSharpness;

        // collect properties from the material properties
        public override void FindProperties(MaterialProperty[] properties)
        {
            base.FindProperties(properties);
            shadingModelProperties = new SimpleLitGUI.SimpleLitProperties(properties);

            snowTint = FindProperty("_SnowTint", properties);
            snowCoverage = FindProperty("_SnowCoverage", properties);
            snowScale = FindProperty("_SnowScale", properties);
            scatter = FindProperty("_Scatter", properties);
            slopeThreshold = FindProperty("_SlopeThreshold", properties);
            slopeNoise = FindProperty("_SlopeNoise", properties);
            slopeSharpness = FindProperty("_SlopeSharpness", properties);
        }

        // material changed check
        public override void ValidateMaterial(Material material)
        {
            SetMaterialKeywords(material, SimpleLitGUI.SetMaterialKeywords);
        }

        // material main surface options
        public override void DrawSurfaceOptions(Material material)
        {
            if (material == null)
                throw new ArgumentNullException("material");

            // Use default labelWidth
            EditorGUIUtility.labelWidth = 0f;

            base.DrawSurfaceOptions(material);

            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Snow Properties", EditorStyles.boldLabel);
            materialEditor.ColorProperty(snowTint, "Tint Color");
            materialEditor.RangeProperty(snowCoverage, "Coverage");
            materialEditor.FloatProperty(snowScale, "Scale");
            materialEditor.RangeProperty(scatter, "Scatter");
            materialEditor.RangeProperty(slopeThreshold, "Slopw Threshold");
            materialEditor.RangeProperty(slopeNoise, "Slope Noise");
            materialEditor.RangeProperty(slopeSharpness, "Slope Sharpness");
        }

        // material main surface inputs
        public override void DrawSurfaceInputs(Material material)
        {
            base.DrawSurfaceInputs(material);
            SimpleLitGUI.Inputs(shadingModelProperties, materialEditor, material);
            DrawEmissionProperties(material, true);
            DrawTileOffset(materialEditor, baseMapProp);
        }

        public override void DrawAdvancedOptions(Material material)
        {
            SimpleLitGUI.Advanced(shadingModelProperties);
            base.DrawAdvancedOptions(material);
        }

        public override void AssignNewShaderToMaterial(Material material, Shader oldShader, Shader newShader)
        {
            if (material == null)
                throw new ArgumentNullException("material");

            // _Emission property is lost after assigning Standard shader to the material
            // thus transfer it before assigning the new shader
            if (material.HasProperty("_Emission"))
            {
                material.SetColor("_EmissionColor", material.GetColor("_Emission"));
            }

            base.AssignNewShaderToMaterial(material, oldShader, newShader);

            if (oldShader == null || !oldShader.name.Contains("Legacy Shaders/"))
            {
                SetupMaterialBlendMode(material);
                return;
            }

            SurfaceType surfaceType = SurfaceType.Opaque;
            BlendMode blendMode = BlendMode.Alpha;
            if (oldShader.name.Contains("/Transparent/Cutout/"))
            {
                surfaceType = SurfaceType.Opaque;
                material.SetFloat("_AlphaClip", 1);
            }
            else if (oldShader.name.Contains("/Transparent/"))
            {
                // NOTE: legacy shaders did not provide physically based transparency
                // therefore Fade mode
                surfaceType = SurfaceType.Transparent;
                blendMode = BlendMode.Alpha;
            }
            material.SetFloat("_Surface", (float)surfaceType);
            material.SetFloat("_Blend", (float)blendMode);
        }
    }
}
