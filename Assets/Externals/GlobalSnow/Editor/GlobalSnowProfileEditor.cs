using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

namespace GlobalSnowEffect {
    [CustomEditor(typeof(GlobalSnowProfile))]
    public class GlobalSnowProfileEditor : Editor {

        static GUIStyle titleLabelStyle, sectionHeaderStyle;
        static Color titleColor;
        readonly static bool[] expandSection = new bool[6];
        const string SECTION_PREFS = "GlobalSnowExpandSection";
        readonly static string[] sectionNames = new string[] {
                                                "Scene Setup", "Quality", "Coverage", "Appearance", "Features", "Mask Editor"
                                };
        const int SCENE_SETTINGS = 0;
        const int QUALITY_SETTINGS = 1;
        const int COVERAGE_SETTINGS = 2;
        const int APPEARANCE_SETTINGS = 3;
        const int FEATURE_SETTINGS = 4;
        const int MASK_EDITOR = 5;

        SerializedProperty layerMask, zenithalMask, minimumAltitude, altitudeScatter, minimumAltitudeVegetationOffset;
        SerializedProperty coverageResolution, coverageExtension, snowQuality, reliefAmount;
        SerializedProperty useZenithalCoverage, coverageMask, coverageMaskTexture, coverageMaskWorldSize, coverageMaskWorldCenter, groundCoverage, groundCoverageRandomization, coverageUpdateMethod;
        SerializedProperty slopeThreshold, slopeSharpness, slopeNoise;
        SerializedProperty occlusion, occlusionIntensity, glitterStrength, maxExposure, preserveGI;
        SerializedProperty snowTint;
        SerializedProperty snowNormalsTex, snowNormalsStrength, noiseTex, noiseTexScale;
        SerializedProperty detailDistance;
        SerializedProperty snowfall, snowfallIntensity, snowfallSpeed, snowfallWind, snowfallDistance, snowfallUseIllumination;
        SerializedProperty snowdustIntensity, snowdustVerticalOffset;
        SerializedProperty cameraFrost, cameraFrostIntensity, cameraFrostSpread, cameraFrostDistortion, cameraFrostTintColor;
        SerializedProperty smoothness, snowAmount, altitudeBlending;
        SerializedProperty grassCoverage;

        void OnEnable() {
            titleColor = EditorGUIUtility.isProSkin ? new Color(0.52f, 0.66f, 0.9f) : new Color(0.12f, 0.16f, 0.4f);
            for (int k = 0; k < expandSection.Length; k++) {
                expandSection[k] = EditorPrefs.GetBool(SECTION_PREFS + k, false);
            }

            smoothness = serializedObject.FindProperty("smoothness");
            snowAmount = serializedObject.FindProperty("snowAmount");
            layerMask = serializedObject.FindProperty("layerMask");
            zenithalMask = serializedObject.FindProperty("zenithalMask");
            minimumAltitude = serializedObject.FindProperty("minimumAltitude");
            altitudeScatter = serializedObject.FindProperty("altitudeScatter");
            snowTint = serializedObject.FindProperty("snowTint");
            snowNormalsTex = serializedObject.FindProperty("snowNormalsTex");
            snowNormalsStrength = serializedObject.FindProperty("snowNormalsStrength");
            noiseTex = serializedObject.FindProperty("noiseTex");
            noiseTexScale = serializedObject.FindProperty("noiseTexScale");
            altitudeBlending = serializedObject.FindProperty("altitudeBlending");
            minimumAltitudeVegetationOffset = serializedObject.FindProperty("minimumAltitudeVegetationOffset");
            detailDistance = serializedObject.FindProperty("detailDistance");
            useZenithalCoverage = serializedObject.FindProperty("useZenithalCoverage");
            coverageResolution = serializedObject.FindProperty("coverageResolution");
            coverageExtension = serializedObject.FindProperty("coverageExtension");
            coverageMask = serializedObject.FindProperty("coverageMask");
            coverageUpdateMethod = serializedObject.FindProperty("coverageUpdateMethod");
            groundCoverage = serializedObject.FindProperty("groundCoverage");
            groundCoverageRandomization = serializedObject.FindProperty("groundCoverageRandomization");
            coverageMaskTexture = serializedObject.FindProperty("coverageMaskTexture");
            coverageMaskWorldSize = serializedObject.FindProperty("coverageMaskWorldSize");
            coverageMaskWorldCenter = serializedObject.FindProperty("coverageMaskWorldCenter");
            slopeThreshold = serializedObject.FindProperty("slopeThreshold");
            slopeSharpness = serializedObject.FindProperty("slopeSharpness");
            slopeNoise = serializedObject.FindProperty("slopeNoise");
            snowQuality = serializedObject.FindProperty("snowQuality");
            reliefAmount = serializedObject.FindProperty("reliefAmount");
            occlusion = serializedObject.FindProperty("occlusion");
            occlusionIntensity = serializedObject.FindProperty("occlusionIntensity");
            glitterStrength = serializedObject.FindProperty("glitterStrength");
            snowfall = serializedObject.FindProperty("snowfall");
            snowfallIntensity = serializedObject.FindProperty("snowfallIntensity");
            snowfallSpeed = serializedObject.FindProperty("snowfallSpeed");
            snowfallWind = serializedObject.FindProperty("snowfallWind");
            snowfallDistance = serializedObject.FindProperty("snowfallDistance");
            snowfallUseIllumination = serializedObject.FindProperty("snowfallUseIllumination");
            snowdustIntensity = serializedObject.FindProperty("snowdustIntensity");
            snowdustVerticalOffset = serializedObject.FindProperty("snowdustVerticalOffset");
            maxExposure = serializedObject.FindProperty("maxExposure");
            preserveGI = serializedObject.FindProperty("preserveGI");
            cameraFrost = serializedObject.FindProperty("cameraFrost");
            cameraFrostIntensity = serializedObject.FindProperty("cameraFrostIntensity");
            cameraFrostSpread = serializedObject.FindProperty("cameraFrostSpread");
            cameraFrostDistortion = serializedObject.FindProperty("cameraFrostDistortion");
            cameraFrostTintColor = serializedObject.FindProperty("cameraFrostTintColor");
            grassCoverage = serializedObject.FindProperty("grassCoverage");
        }

        void OnDestroy() {
            // Save folding sections state
            for (int k = 0; k < expandSection.Length; k++) {
                EditorPrefs.SetBool(SECTION_PREFS + k, expandSection[k]);
            }
        }

        public override void OnInspectorGUI() {

            serializedObject.Update();

            if (sectionHeaderStyle == null) {
                sectionHeaderStyle = new GUIStyle(EditorStyles.foldout);
            }
            sectionHeaderStyle.SetFoldoutColor();

            if (titleLabelStyle == null) {
                titleLabelStyle = new GUIStyle(EditorStyles.label);
            }
            titleLabelStyle.normal.textColor = titleColor;
            titleLabelStyle.fontStyle = FontStyle.Bold;

            expandSection[QUALITY_SETTINGS] = EditorGUILayout.Foldout(expandSection[QUALITY_SETTINGS], sectionNames[QUALITY_SETTINGS], sectionHeaderStyle);
            if (expandSection[QUALITY_SETTINGS]) {

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button(new GUIContent("Best Quality", "Enables relief, occlusion and better coverage quality."))) {
                    coverageResolution.intValue = 3;
                    snowQuality.intValue = (int)SnowQuality.ReliefMapping;
                    reliefAmount.floatValue = 0.3f;
                    occlusion.boolValue = true;
                    occlusionIntensity.floatValue = 1.2f;
                    glitterStrength.floatValue = 0.75f;
                    preserveGI.boolValue = true;
                }
                if (GUILayout.Button(new GUIContent("Medium", "Enables relief, normal coverage quality and medium distance optimization."))) {
                    coverageResolution.intValue = 2;
                    snowQuality.intValue = (int)SnowQuality.ReliefMapping;
                    reliefAmount.floatValue = 0.3f;
                    occlusion.boolValue = false;
                    glitterStrength.floatValue = 0.75f;
                    preserveGI.boolValue = false;
                }
                if (GUILayout.Button(new GUIContent("Fastest", "Uses optimized snow renderer for distance snow on entire scene."))) {
                    coverageExtension.intValue = 1;
                    coverageResolution.intValue = 1;
                    snowQuality.intValue = (int)SnowQuality.FlatShading;
                    preserveGI.boolValue = false;
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Separator();
            expandSection[COVERAGE_SETTINGS] = EditorGUILayout.Foldout(expandSection[COVERAGE_SETTINGS], sectionNames[COVERAGE_SETTINGS], sectionHeaderStyle);
            if (expandSection[COVERAGE_SETTINGS]) {
                EditorGUILayout.PropertyField(layerMask, new GUIContent("Layer Mask", "Specifies which objects can receive snow. Alternatively you can add the script GlobalSnowIgnoreCoverage to any number of gameobjects to be exluded without changing their layer."));
                EditorGUILayout.PropertyField(minimumAltitude, new GUIContent("Minimum Altitude", "Specify snow level."));
                EditorGUILayout.PropertyField(useZenithalCoverage, new GUIContent("Use Zenithal Coverage", "Automatically computes snow coverage."));
                if (useZenithalCoverage.boolValue) {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(zenithalMask, new GUIContent("Zenithal Mask", "Specify which objects are considered for top-down occlusion. Objects on top prevent snow on objects beneath them. Make sure to exclude any particle system to improve performance and avoid coverage issues."));
                    EditorGUILayout.PropertyField(coverageUpdateMethod, new GUIContent("Coverage Update", "Specifies when the snow coverage needs to be computed. Every frame, Discrete (every 50 meters of player movement), or Manual (requires manual call to UpdateSnowCoverage function)."));
                    EditorGUILayout.PropertyField(groundCoverage, new GUIContent("Ground Coverage", "Increase or reduce snow coverage under opaque objects."));
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(groundCoverageRandomization, new GUIContent("Perimeter Randomization", "Applies randomization to ground coverage."));
                    EditorGUI.indentLevel--;
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(coverageExtension, new GUIContent("Coverage Extension", "Area included in the snow coverage. 1 = 512 meters, 2 = 1024 meters. Note that greater extension reduces quality."));
                    GUILayout.Label(Mathf.Pow(2, 8f + coverageExtension.intValue).ToString(), GUILayout.Width(50));
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(coverageResolution, new GUIContent("Coverage Quality", "Resolution of the coverage texture (1=512 pixels, 2=1024 pixels, 3=2048 pixels)."));
                    GUILayout.Label(Mathf.Pow(2, 8f + coverageResolution.intValue).ToString(), GUILayout.Width(50));
                    EditorGUILayout.EndHorizontal();
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.LabelField("Grass Coverage");
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(minimumAltitudeVegetationOffset, new GUIContent("Altitude Offset", "Applies a vertical offset to the minimum altitude only to grass and trees. This option is useful to avoid showing full grass covered with snow when altitude scattered is used and there's little snow on ground which causes unnatural visuals."));
                EditorGUILayout.PropertyField(grassCoverage, new GUIContent("Coverage", "Amount of snow over grass objects."));
                EditorGUI.indentLevel--;
                EditorGUILayout.PropertyField(coverageMask, new GUIContent("Coverage Mask", "Uses alpha channel of a custom texture as snow coverage mask."));
                if (coverageMask.boolValue) {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(coverageMaskTexture, new GUIContent("Texture (A)", "Snow coverage mask. A value of alpha of zero means no snow."));
                    EditorGUILayout.PropertyField(coverageMaskWorldSize, new GUIContent("World Size", "Mapping of the texture against the world in world units. Usually this should match terrain size."));
                    EditorGUILayout.PropertyField(coverageMaskWorldCenter, new GUIContent("World Center", "Mapping of the texture center against the world in world units. Use this as an offset to apply coverage mask over a certain area."));
                    EditorGUI.indentLevel--;
                }
            }

            EditorGUILayout.Separator();
            expandSection[APPEARANCE_SETTINGS] = EditorGUILayout.Foldout(expandSection[APPEARANCE_SETTINGS], sectionNames[APPEARANCE_SETTINGS], sectionHeaderStyle);

            if (expandSection[APPEARANCE_SETTINGS]) {
                EditorGUILayout.PropertyField(snowAmount, new GUIContent("Snow Amount", "Global snow threshold."));
                EditorGUILayout.PropertyField(snowQuality, new GUIContent("Snow Complexity", "Choose the rendering scheme for the snow."));
                if (snowQuality.intValue == (int)SnowQuality.ReliefMapping) {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(reliefAmount, new GUIContent("Relief Amount", "Relief intensity."));
                    EditorGUILayout.PropertyField(occlusion, new GUIContent("Occlusion", "Enables occlusion effect."));
                    if (occlusion.boolValue) {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(occlusionIntensity, new GUIContent("Intensity", "Occlusion intensity."));
                        EditorGUI.indentLevel--;
                    }
                    EditorGUI.indentLevel--;
                }
                if (snowQuality.intValue != (int)SnowQuality.FlatShading) {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(glitterStrength, new GUIContent("Glitter Strength", "Snow glitter intensity. Set to zero to disable."));
                    EditorGUI.indentLevel--;
                }

                EditorGUILayout.LabelField("Slope Options");
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(slopeThreshold, new GUIContent("Threshold", "The maximum slope where snow can accumulate."));
                EditorGUILayout.PropertyField(slopeSharpness, new GUIContent("Sharpness", "The sharpness (or smoothness) of the snow at terrain borders."));
                EditorGUILayout.PropertyField(slopeNoise, new GUIContent("Noise", "Amount of randomization to fill the transient area between low and high slope (determined by slope threshold)."));
                EditorGUI.indentLevel--;
                GUI.enabled = true;
                EditorGUILayout.PropertyField(altitudeScatter, new GUIContent("Altitude Scatter", "Defines random snow scattering around minimum altitude level."));
                EditorGUILayout.PropertyField(altitudeBlending, new GUIContent("Altitude Blending", "Defines vertical gradient length for snow blending."));
                EditorGUILayout.PropertyField(snowTint, new GUIContent("Snow Tint", "Snow tint color."));
                EditorGUILayout.PropertyField(smoothness, new GUIContent("Smoothness", "Snow PBR smoothness."));
                EditorGUILayout.PropertyField(maxExposure, new GUIContent("Max Exposure", "Controls maximum snow brightness."));
                EditorGUILayout.PropertyField(preserveGI, new GUIContent("Preserve Global Illumination", "Keeps GI on added snow. Disabling this option improves performance."));

                if (snowQuality.intValue != (int)SnowQuality.FlatShading) {
                    EditorGUILayout.ObjectField(snowNormalsTex, new GUIContent("Snow Normals Texture"));
                    EditorGUILayout.PropertyField(snowNormalsStrength, new GUIContent("Snow Normals Strength"));
                    EditorGUILayout.ObjectField(noiseTex, new GUIContent("Noise Texture"));
                    EditorGUILayout.PropertyField(noiseTexScale, new GUIContent("Noise Texture Scale"));
                }
            }

            EditorGUILayout.Separator();
            expandSection[FEATURE_SETTINGS] = EditorGUILayout.Foldout(expandSection[FEATURE_SETTINGS], sectionNames[FEATURE_SETTINGS], sectionHeaderStyle);

            if (expandSection[FEATURE_SETTINGS]) {
                EditorGUILayout.PropertyField(snowfall, new GUIContent("Snowfall", "Enable snowfall."));
                if (snowfall.boolValue) {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(snowfallIntensity, new GUIContent("Intensity", "Snowflakes emission rate."));
                    EditorGUILayout.PropertyField(snowfallSpeed, new GUIContent("Speed", "Snowfall speed."));
                    EditorGUILayout.PropertyField(snowfallWind, new GUIContent("Wind", "Horizontal wind speed."));
                    EditorGUILayout.PropertyField(snowfallDistance, new GUIContent("Emission Distance", "Emission box scale. Reduce to produce more dense snowfall."));
                    EditorGUILayout.PropertyField(snowfallUseIllumination, new GUIContent("Use Illumination", "If enabled, snow particles will be affected by light."));
                    EditorGUILayout.HelpBox("You can customize particle system prefab located in GlobalSnow/Resources/Prefab folder.", MessageType.Info);
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.PropertyField(snowdustIntensity, new GUIContent("Snow Dust", "Snow dust intensity."));
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(snowdustVerticalOffset, new GUIContent("Vertical Offset", "Vertical offset for the emission volume with respect to the camera altitude."));
                EditorGUI.indentLevel--;
                if (snowdustIntensity.floatValue > 0) {
                    EditorGUILayout.HelpBox("Customize additional options like gravity or collision of snow dust in the SnowDustSystem prefab inside GlobalSnow/Resources/Common/Prefabs folder.", MessageType.Info);
                }

                bool prevBool = cameraFrost.boolValue;
                EditorGUILayout.PropertyField(cameraFrost, new GUIContent("Camera Frost", "Enable camera frost effect."));
                if (cameraFrost.boolValue) {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(cameraFrostIntensity, new GUIContent("Intensity", "Intensity of camera frost effect."));
                    EditorGUILayout.PropertyField(cameraFrostSpread, new GUIContent("Spread", "Amplitude of camera frost effect."));
                    EditorGUILayout.PropertyField(cameraFrostDistortion, new GUIContent("Distortion", "Distortion magnitude."));
                    EditorGUILayout.PropertyField(cameraFrostTintColor, new GUIContent("Tint Color", "Tinting color for the frost effect."));
                    EditorGUI.indentLevel--;
                }
            }
            EditorGUILayout.Separator();

            if (serializedObject.ApplyModifiedProperties()) {
                GlobalSnow gs = GlobalSnow.instance;
                GlobalSnowProfile profile = (GlobalSnowProfile)target;
                if (gs != null && gs.profile == profile) {
                    profile.ApplyTo(gs);
                }
            }
        }

    }

}
