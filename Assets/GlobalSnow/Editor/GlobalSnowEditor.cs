using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

namespace GlobalSnowEffect {
    [CustomEditor(typeof(GlobalSnow))]
    public class GlobalSnowEditor : Editor {

        static GUIStyle titleLabelStyle, sectionHeaderStyle, whiteBack;
        static Color titleColor;
        readonly static bool[] expandSection = new bool[6];
        const string SECTION_PREFS = "GlobalSnowExpandSection";
        readonly static string[] sectionNames = new string[] {
                                                "Scene Setup", "Quality Presets", "Coverage", "Appearance", "Features", "Mask Editor"
                                };
        const int SCENE_SETTINGS = 0;
        const int QUALITY_SETTINGS = 1;
        const int COVERAGE_SETTINGS = 2;
        const int APPEARANCE_SETTINGS = 3;
        const int FEATURE_SETTINGS = 4;
        const int MASK_EDITOR = 5;

        SerializedProperty profile, addSnowToGrass;
        SerializedProperty sun, layerMask, zenithalMask, minimumAltitude, altitudeScatter, minimumAltitudeVegetationOffset;
        SerializedProperty useZenithalCoverage, coverageResolution, coverageExtension, snowQuality, reliefAmount;
        SerializedProperty defaultExclusionLayer, exclusionBias, exclusionDoubleSided, exclusionDefaultCutOff, exclusionUseFastMaskShader;
        SerializedProperty coverageMask, coverageMaskTexture, coverageMaskWorldSize, coverageMaskWorldCenter, groundCoverage, groundCoverageRandomization, coverageUpdateMethod, coverageMaskFillOutside, coverageDepthDebug;
        SerializedProperty slopeThreshold, slopeSharpness, slopeNoise;
        SerializedProperty occlusion, occlusionIntensity, glitterStrength, maxExposure, showSnowInSceneView;
        SerializedProperty snowTint;
        SerializedProperty snowNormalsTex, snowNormalsStrength, noiseTex, noiseTexScale;
        SerializedProperty groundCheck, characterController, groundDistance, footprints, footprintsTexture, footprintsDuration, footprintsAutoFPS, footprintsScale, footprintsObscurance;
        SerializedProperty terrainMarks, terrainMarksDuration, terrainMarksDefaultSize, terrainMarksAutoFPS, terrainMarksViewDistance, terrainMarksRoofMinDistance;
        SerializedProperty snowfall, snowfallIntensity, snowfallSpeed, snowfallWind, snowfallDistance, snowfallUseIllumination;
        SerializedProperty snowdustIntensity, snowdustVerticalOffset;
        SerializedProperty cameraFrost, cameraFrostIntensity, cameraFrostSpread, cameraFrostDistortion, cameraFrostTintColor;
        SerializedProperty terrainMarksTextureSize, terrainMarksStepMaxDistance, grassCoverage;
        SerializedProperty showCoverageGizmo, smoothness, preserveGI, minimumGIAmbient, snowAmount, altitudeBlending;
        SerializedProperty maskEditorEnabled, maskTextureResolution, maskBrushMode, maskBrushWidth, maskBrushFuzziness, maskBrushOpacity;

        Texture2D _headerTexture;
        GlobalSnow gs;
        bool mouseIsDown;
        bool cameraFrostedChanged;
        Material matDepthPreview;
        MeshRenderer snowMR;
        static float objectSnowerBorder;
        static float objectSnowerOpacity = 0.25f;
        bool grassSnowChange;
        readonly TextureImporterSettings settings = new TextureImporterSettings();
        bool forceUpdateProperties;

        void OnEnable() {
            titleColor = EditorGUIUtility.isProSkin ? new Color(0.52f, 0.66f, 0.9f) : new Color(0.12f, 0.16f, 0.4f);
            for (int k = 0; k < expandSection.Length; k++) {
                expandSection[k] = EditorPrefs.GetBool(SECTION_PREFS + k, false);
            }
            _headerTexture = Resources.Load<Texture2D>("GlobalSnow_EditorHeader");
            whiteBack = new GUIStyle();
            whiteBack.normal.background = MakeTex(4, 4, Color.white);

            profile = serializedObject.FindProperty("_profile");
            addSnowToGrass = serializedObject.FindProperty("_addSnowToGrass");
            sun = serializedObject.FindProperty("_sun");
            showSnowInSceneView = serializedObject.FindProperty("_showSnowInSceneView");
            showCoverageGizmo = serializedObject.FindProperty("_showCoverageGizmo");
            smoothness = serializedObject.FindProperty("_smoothness");
            preserveGI = serializedObject.FindProperty("_preserveGI");
            minimumGIAmbient = serializedObject.FindProperty("_minimumGIAmbient");
            snowAmount = serializedObject.FindProperty("_snowAmount");
            defaultExclusionLayer = serializedObject.FindProperty("_defaultExclusionLayer");
            exclusionBias = serializedObject.FindProperty("_exclusionBias");
            exclusionDoubleSided = serializedObject.FindProperty("_exclusionDoubleSided");
            exclusionDefaultCutOff = serializedObject.FindProperty("_exclusionDefaultCutOff");
            exclusionUseFastMaskShader = serializedObject.FindProperty("_exclusionUseFastMaskShader");
            layerMask = serializedObject.FindProperty("_layerMask");
            zenithalMask = serializedObject.FindProperty("_zenithalMask");
            minimumAltitude = serializedObject.FindProperty("_minimumAltitude");
            altitudeScatter = serializedObject.FindProperty("_altitudeScatter");
            snowTint = serializedObject.FindProperty("_snowTint");
            snowNormalsTex = serializedObject.FindProperty("snowNormalsTex");
            snowNormalsStrength = serializedObject.FindProperty("_snowNormalsStrength");
            noiseTex = serializedObject.FindProperty("noiseTex");
            noiseTexScale = serializedObject.FindProperty("_noiseTexScale");
            altitudeBlending = serializedObject.FindProperty("_altitudeBlending");
            minimumAltitudeVegetationOffset = serializedObject.FindProperty("_minimumAltitudeVegetationOffset");
            useZenithalCoverage = serializedObject.FindProperty("_useZenithalCoverage");
            coverageResolution = serializedObject.FindProperty("_coverageResolution");
            coverageExtension = serializedObject.FindProperty("_coverageExtension");
            coverageMask = serializedObject.FindProperty("_coverageMask");
            coverageUpdateMethod = serializedObject.FindProperty("_coverageUpdateMethod");
            coverageDepthDebug = serializedObject.FindProperty("coverageDepthDebug");
            coverageMaskFillOutside = serializedObject.FindProperty("_coverageMaskFillOutside");
            groundCoverage = serializedObject.FindProperty("_groundCoverage");
            groundCoverageRandomization = serializedObject.FindProperty("_groundCoverageRandomization");
            coverageMaskTexture = serializedObject.FindProperty("_coverageMaskTexture");
            coverageMaskWorldSize = serializedObject.FindProperty("_coverageMaskWorldSize");
            coverageMaskWorldCenter = serializedObject.FindProperty("_coverageMaskWorldCenter");
            slopeThreshold = serializedObject.FindProperty("_slopeThreshold");
            slopeSharpness = serializedObject.FindProperty("_slopeSharpness");
            slopeNoise = serializedObject.FindProperty("_slopeNoise");
            snowQuality = serializedObject.FindProperty("_snowQuality");
            reliefAmount = serializedObject.FindProperty("_reliefAmount");
            occlusion = serializedObject.FindProperty("_occlusion");
            occlusionIntensity = serializedObject.FindProperty("_occlusionIntensity");
            glitterStrength = serializedObject.FindProperty("_glitterStrength");
            groundCheck = serializedObject.FindProperty("_groundCheck");
            groundDistance = serializedObject.FindProperty("_groundDistance");
            characterController = serializedObject.FindProperty("_characterController");
            footprints = serializedObject.FindProperty("_footprints");
            footprintsTexture = serializedObject.FindProperty("_footprintsTexture");
            footprintsDuration = serializedObject.FindProperty("_footprintsDuration");
            footprintsAutoFPS = serializedObject.FindProperty("_footprintsAutoFPS");
            footprintsScale = serializedObject.FindProperty("_footprintsScale");
            footprintsObscurance = serializedObject.FindProperty("_footprintsObscurance");
            terrainMarks = serializedObject.FindProperty("_terrainMarks");
            terrainMarksDuration = serializedObject.FindProperty("_terrainMarksDuration");
            terrainMarksDefaultSize = serializedObject.FindProperty("_terrainMarksDefaultSize");
            terrainMarksAutoFPS = serializedObject.FindProperty("_terrainMarksAutoFPS");
            terrainMarksViewDistance = serializedObject.FindProperty("_terrainMarksViewDistance");
            terrainMarksTextureSize = serializedObject.FindProperty("terrainMarksTextureSize");
            terrainMarksStepMaxDistance = serializedObject.FindProperty("_terrainMarksStepMaxDistance");
            terrainMarksRoofMinDistance = serializedObject.FindProperty("_terrainMarksRoofMinDistance");
            snowfall = serializedObject.FindProperty("_snowfall");
            snowfallIntensity = serializedObject.FindProperty("_snowfallIntensity");
            snowfallSpeed = serializedObject.FindProperty("_snowfallSpeed");
            snowfallWind = serializedObject.FindProperty("_snowfallWind");
            snowfallDistance = serializedObject.FindProperty("_snowfallDistance");
            snowfallUseIllumination = serializedObject.FindProperty("_snowfallUseIllumination");
            snowdustIntensity = serializedObject.FindProperty("_snowdustIntensity");
            snowdustVerticalOffset = serializedObject.FindProperty("_snowdustVerticalOffset");
            maxExposure = serializedObject.FindProperty("_maxExposure");
            cameraFrost = serializedObject.FindProperty("_cameraFrost");
            cameraFrostIntensity = serializedObject.FindProperty("_cameraFrostIntensity");
            cameraFrostSpread = serializedObject.FindProperty("_cameraFrostSpread");
            cameraFrostDistortion = serializedObject.FindProperty("_cameraFrostDistortion");
            cameraFrostTintColor = serializedObject.FindProperty("_cameraFrostTintColor");
            grassCoverage = serializedObject.FindProperty("_grassCoverage");

            maskEditorEnabled = serializedObject.FindProperty("_maskEditorEnabled");
            maskTextureResolution = serializedObject.FindProperty("_maskTextureResolution");
            maskBrushMode = serializedObject.FindProperty("_maskBrushMode");
            maskBrushWidth = serializedObject.FindProperty("_maskBrushWidth");
            maskBrushFuzziness = serializedObject.FindProperty("_maskBrushFuzziness");
            maskBrushOpacity = serializedObject.FindProperty("_maskBrushOpacity");

            gs = (GlobalSnow)target;
        }

        void OnDestroy() {
            // Save folding sections state
            for (int k = 0; k < expandSection.Length; k++) {
                EditorPrefs.SetBool(SECTION_PREFS + k, expandSection[k]);
            }
        }

        public override void OnInspectorGUI() {

            forceUpdateProperties = false;
            bool profileChanged = false;

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

            EditorGUILayout.Separator();
            TextAnchor oldAnchor = GUI.skin.label.alignment;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            EditorGUILayout.BeginHorizontal(whiteBack);
            GUILayout.Label(_headerTexture, GUILayout.ExpandWidth(true));
            GUI.skin.label.alignment = oldAnchor;
            GUILayout.EndHorizontal();


            UniversalRenderPipelineAsset pipe = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
            if (pipe == null) {
                EditorGUILayout.HelpBox("Universal Rendering Pipeline asset is not set in 'Project Settings / Graphics' !", MessageType.Error);
                EditorGUILayout.Separator();
                GUI.enabled = false;
            } else if (!GlobalSnowRenderFeature.installed) {
                EditorGUILayout.HelpBox("GlobalSnow Render Feature must be added to the rendering pipeline renderer.", MessageType.Error);
                if (GUILayout.Button("Go to Universal Rendering Pipeline Asset")) {
                    Selection.activeObject = pipe;
                }
                EditorGUILayout.Separator();
                GUI.enabled = false;
            } else if (GlobalSnowRenderFeature.renderingMode != RenderingMode.Deferred) {
                EditorGUILayout.HelpBox("Global Snow requires deferred rendering path.", MessageType.Error);
                if (GUILayout.Button("Go to Universal Rendering Pipeline Asset")) {
                    Selection.activeObject = pipe;
                }
                EditorGUILayout.Separator();
                GUI.enabled = false;
            }

            EditorGUILayout.BeginHorizontal();
            expandSection[SCENE_SETTINGS] = EditorGUILayout.Foldout(expandSection[SCENE_SETTINGS], sectionNames[SCENE_SETTINGS], sectionHeaderStyle);
            if (GUILayout.Button("Help", GUILayout.Width(40))) {
                if (!EditorUtility.DisplayDialog("Global Snow", "To learn more about a property in this inspector move the mouse over the label for a quick description (tooltip).\n\nPlease check README file in the root of the asset for details and contact support.\n\nIf you like Global Snow, please rate it on the Asset Store. For feedback and suggestions visit our support forum on kronnect.com.", "Close", "Visit Support Forum")) {
                    Application.OpenURL("https://kronnect.com/support");
                }
            }
            EditorGUILayout.EndHorizontal();
            if (expandSection[SCENE_SETTINGS]) {
                EditorGUILayout.PropertyField(sun, new GUIContent("Sun", "Used to compute basic lighting over snow."));
                EditorGUILayout.PropertyField(showSnowInSceneView, new GUIContent("Show In Scene View", "Enabled rendering of snow in the Scene View."));
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(addSnowToGrass, new GUIContent("Add Snow To Grass", "Overrides default URP grass shader with a custom shader that supports snow."));
                if (EditorGUI.EndChangeCheck()) grassSnowChange = true;
                if (grassSnowChange) {
                    EditorGUILayout.HelpBox("Grass shader changed. Stop and start scene to refresh changes.", MessageType.Info);
                }
                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(profile, new GUIContent("Snow Profile", "Assign a custom profile to reuse its settings."));
                if (GUILayout.Button("Create", GUILayout.Width(80))) {
                    CreateSnowProfile();
                }
                if (EditorGUI.EndChangeCheck() && profile.objectReferenceValue != null) {
                    profileChanged = true;
                }
                EditorGUILayout.EndHorizontal();
                if (profile.objectReferenceValue != null) {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(GUIContent.none, GUILayout.Width(EditorGUIUtility.labelWidth));
                    if (GUILayout.Button(new GUIContent("Revert", "Load settings from profile"))) {
                        LoadProfile();
                    }
                    if (GUILayout.Button(new GUIContent("Apply", "Save current settings into this profile."))) {
                        SaveToProfile();
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.Separator();
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
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(layerMask, new GUIContent("Layer Mask", "Optionally exclude some objects from being covered by snow. Alternatively you can add the script GlobalSnowIgnoreCoverage to any number of gameobjects to be exluded without changing their layer."));
                if (GUILayout.Button("Refresh", GUILayout.Width(80))) {
                    forceUpdateProperties = true;
                }
                EditorGUILayout.EndHorizontal();

                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(defaultExclusionLayer, new GUIContent("Default Exclusion Layer", "This is the layer used to exclude temporary objects marked as not covered by snow. Use a layer number that you don't use."));
                EditorGUILayout.PropertyField(exclusionBias, new GUIContent("Exclusion Depth Bias", "Adjust depth comparison for exclusion purposes."));
                EditorGUILayout.PropertyField(exclusionDoubleSided, new GUIContent("Exclusion Double Sided", "Enable this option when excluding double sided objects from snow."));
                EditorGUILayout.PropertyField(exclusionUseFastMaskShader, new GUIContent("Use Fast Exclusion Shader", "If enabled, Global Snow will use a fast mask shader to exclude snow from designed objects. If disabled, Global Snow will use the object material to perform the exclusion. If the object has vertex animation (like trees or vegetation), you may want to disable this option."));
                if (exclusionUseFastMaskShader.boolValue) {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(exclusionDefaultCutOff, new GUIContent("Default Cut Off", "Default cut-off value used when excluding snow from objects. The IgnoreCoverage script can be used on specific objects to specify custom cut-off values per object."));
                    EditorGUI.indentLevel--;
                }
                EditorGUI.indentLevel--;

                EditorGUILayout.PropertyField(minimumAltitude, new GUIContent("Minimum Altitude", "Specify snow level."));
                EditorGUILayout.PropertyField(useZenithalCoverage, new GUIContent("Use Zenithal Coverage", "Automatically computes snow coverage."));
                if (useZenithalCoverage.boolValue) {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(zenithalMask, new GUIContent("Zenithal Mask", "Specify which objects are considered for top-down occlusion. Objects on top prevent snow on objects beneath them. Make sure to exclude any particle system to improve performance and avoid coverage issues."));
                    EditorGUILayout.PropertyField(coverageUpdateMethod, new GUIContent("Coverage Update", "Specifies when the snow coverage needs to be computed. Every frame, Discrete (every 50 meters of player movement), or Manual (requires manual call to UpdateSnowCoverage function)."));
                    if (coverageUpdateMethod.intValue != (int)SnowCoverageUpdateMethod.Disabled) {
                        EditorGUILayout.PropertyField(coverageDepthDebug, new GUIContent("Show Coverage Depth", "Shows zenithal depth texture."));
                        if (coverageDepthDebug.boolValue) {
                            if (matDepthPreview == null) {
                                matDepthPreview = new Material(Shader.Find("Hidden/GlobalSnow/Editor/DepthTexPreview"));
                            }
                            Rect space = EditorGUILayout.BeginVertical();
                            GUILayout.Space(EditorGUIUtility.currentViewWidth * 0.9f);
                            EditorGUILayout.EndVertical();
                            EditorGUI.DrawPreviewTexture(space, Texture2D.whiteTexture, matDepthPreview, ScaleMode.ScaleToFit);
                        }

                        if (Application.isPlaying && GUILayout.Button("Update Coverage Now")) {
                            forceUpdateProperties = true;
                        }
                    }
                    EditorGUILayout.PropertyField(groundCoverage, new GUIContent("Ground Coverage", "Increase or reduce snow coverage under opaque objects."));
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(groundCoverageRandomization, new GUIContent("Perimeter Randomization", "Applies randomization to ground coverage."));
                    EditorGUI.indentLevel--;
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(coverageExtension, new GUIContent("Coverage Extension", "Area included in the snow coverage. 1 = 512 meters, 2 = 1024 meters. Note that greater extension reduces quality."));
                    GUILayout.Label(Mathf.Pow(2, 8f + coverageExtension.intValue).ToString(), GUILayout.Width(50));
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.PropertyField(showCoverageGizmo, new GUIContent("Show Coverage Boundary", "Shows a rectangle in SceneView which encloses the coverage area."));
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(coverageResolution, new GUIContent("Coverage Quality", "Resolution of the coverage texture (1=512 pixels, 2=1024 pixels, 3=2048 pixels)."));
                    GUILayout.Label(Mathf.Pow(2, 8f + coverageResolution.intValue).ToString(), GUILayout.Width(50));
                    EditorGUILayout.EndHorizontal();
                    EditorGUI.indentLevel--;
                }
                if (addSnowToGrass.boolValue) {
                    EditorGUILayout.LabelField("Grass Coverage");
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(minimumAltitudeVegetationOffset, new GUIContent("Altitude Offset", "Applies a vertical offset to the minimum altitude only to grass and trees. This option is useful to avoid showing full grass covered with snow when altitude scattered is used and there's little snow on ground which causes unnatural visuals."));
                    EditorGUILayout.PropertyField(grassCoverage, new GUIContent("Coverage", "Amount of snow over grass objects."));
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.PropertyField(coverageMask, new GUIContent("Coverage Mask", "Uses red channel of a custom texture as snow coverage mask."));
                if (coverageMask.boolValue) {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(coverageMaskTexture, new GUIContent("Texture (R)", "Snow coverage mask. A value of alpha of zero means no snow."));
                    CheckCoverageTextureImportSettings((Texture2D)coverageMaskTexture.objectReferenceValue);
                    EditorGUILayout.PropertyField(coverageMaskWorldSize, new GUIContent("World Size", "Mapping of the texture against the world in world units. Usually this should match terrain size."));
                    EditorGUILayout.PropertyField(coverageMaskWorldCenter, new GUIContent("World Center", "Mapping of the texture center against the world in world units. Use this as an offset to apply coverage mask over a certain area."));
                    EditorGUILayout.PropertyField(coverageMaskFillOutside, new GUIContent("Fill Outside Mask", "Fill with snow positions outside of mask world coverage."));
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
                EditorGUILayout.PropertyField(minimumGIAmbient, new GUIContent("Minimum Ambient Intensity"));
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
                if (footprints.boolValue || terrainMarks.boolValue) {
                    if (coverageUpdateMethod.intValue == (int)SnowCoverageUpdateMethod.Disabled) {
                        EditorGUILayout.HelpBox("Coverage Update is disabled. Footprints and Terrain Marks may not render correctly.", MessageType.Warning);
                    }
                }
                EditorGUILayout.PropertyField(footprints, new GUIContent("Footprints", "Enable footprints on snow surface."));
                if (footprints.boolValue) {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(footprintsAutoFPS, new GUIContent("Active", "Add automatic footprints when camera moves (use only in FPS camera). If disabled, no new footprints will be added."));
                    EditorGUILayout.PropertyField(groundCheck, new GUIContent("Ground Check", "How to detect if player is on ground."));
                    if (groundCheck.intValue == (int)GroundCheck.CharacterController) {
                        EditorGUILayout.PropertyField(characterController, new GUIContent("Controller", "The character controller."));
                    } else if (groundCheck.intValue == (int)GroundCheck.RayCast) {
                        EditorGUILayout.PropertyField(groundDistance, new GUIContent("Max Distance", "Max distance to the ground."));
                    }
                    EditorGUILayout.PropertyField(footprintsTexture, new GUIContent("Texture", "Texture for the footprint stamp."), true);
                    EditorGUILayout.PropertyField(footprintsDuration, new GUIContent("Duration", "Duration of the footprints in seconds before fading out completely."));
                    EditorGUILayout.PropertyField(footprintsScale, new GUIContent("Scale", "Increase to reduce the size of the footprints."));
                    EditorGUILayout.PropertyField(footprintsObscurance, new GUIContent("Obscurance", "Makes the footprints darker."));
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.PropertyField(terrainMarks, new GUIContent("Terrain Marks", "Enable terrain marks based on collisions."));
                if (terrainMarks.boolValue) {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(terrainMarksDuration, new GUIContent("Duration", "Duration of the terrain marks in seconds before fading out completely."));
                    EditorGUILayout.PropertyField(terrainMarksDefaultSize, new GUIContent("Default Size", "Default size for a marks produced by automatic collisions. You can call MarkSnowAt() method to specify a custom size."));
                    EditorGUILayout.PropertyField(terrainMarksTextureSize, new GUIContent("Extents", "Size of the internal texture that holds terrain mark data."));
                    EditorGUILayout.PropertyField(terrainMarksViewDistance, new GUIContent("View Distance", "Maximum terrain marks render distance to camera. Reduce to avoid marks repetitions or increase extents parameter."));
                    EditorGUILayout.PropertyField(terrainMarksAutoFPS, new GUIContent("FPS Marks", "Add automatic terrain mark when camera moves (use only in FPS camera)"));
                    EditorGUILayout.PropertyField(terrainMarksStepMaxDistance, new GUIContent("Max Step", "Maximum object distance between positions in 2 consecutive frames. If an object changes position and the new position is further than this value, no trail will be left behind."));
                    EditorGUILayout.PropertyField(terrainMarksRoofMinDistance, new GUIContent("Min Roof Distance", "Minimum distance from stamp position to roof. This setting allows you to fine-control when terrain marks should be placed under a roof."));
                    EditorGUI.indentLevel--;
                }
                GUI.enabled = true;
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
                if (snowdustIntensity.floatValue > 0) {
                    EditorGUI.indentLevel++;

                    if (!pipe.supportsCameraDepthTexture) {
                        EditorGUILayout.HelpBox("Snow Dust particles require Depth Texture option in Universal Rendering Pipeline asset!", MessageType.Error);
                        if (GUILayout.Button("Go to Universal Rendering Pipeline Asset")) {
                            Selection.activeObject = pipe;
                        }
                        EditorGUILayout.Separator();
                    }

                    EditorGUILayout.PropertyField(snowdustVerticalOffset, new GUIContent("Vertical Offset", "Vertical offset for the emission volume with respect to the camera altitude."));
                    EditorGUI.indentLevel--;
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
                if (prevBool != cameraFrost.boolValue) cameraFrostedChanged = true;
            }
            EditorGUILayout.Separator();


            if (targets.Length == 1) {
                expandSection[MASK_EDITOR] = EditorGUILayout.Foldout(expandSection[MASK_EDITOR], sectionNames[MASK_EDITOR], sectionHeaderStyle);
                if (expandSection[MASK_EDITOR]) {
                    EditorGUILayout.PropertyField(maskEditorEnabled, new GUIContent("Enable Editor", "Activates terrain brush to paint/remove snow intensity at custom locations."));
                    if (maskEditorEnabled.boolValue) {
                        if (!coverageMask.boolValue) {
                            EditorGUILayout.BeginVertical(GUI.skin.box);
                            EditorGUILayout.LabelField("Coverage Mask feature is disabled. Enable it?");
                            if (GUILayout.Button("Enable Coverage Mask")) coverageMask.boolValue = true;
                            EditorGUILayout.EndVertical();
                            EditorGUILayout.Separator();
                            GUI.enabled = false;
                        }
                        EditorGUI.BeginChangeCheck();
                        EditorGUILayout.PropertyField(coverageMaskTexture, new GUIContent("Mask Texture", "Snow coverage mask. A value of alpha of zero means no snow."));
                        Texture2D tex = (Texture2D)coverageMaskTexture.objectReferenceValue;
                        CheckCoverageTextureImportSettings(tex);
                        if (tex != null) {
                            if (EditorGUI.EndChangeCheck()) {
                                if (tex.isReadable) {
                                    gs.maskColors = tex.GetPixels32();
                                    maskTextureResolution.intValue = tex.width;
                                }
                            }
                            EditorGUI.indentLevel++;
                            EditorGUILayout.LabelField("Texture Size", tex.width.ToString());
                            EditorGUILayout.LabelField("Texture Path", AssetDatabase.GetAssetPath(tex));
                            EditorGUI.indentLevel--;
                        }
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.Space();
                        EditorGUILayout.BeginVertical(GUI.skin.box);
                        EditorGUILayout.PropertyField(maskTextureResolution, new GUIContent("Resolution", "Resolution of the mask texture. Higher resolution allows more detail but it can be slower."));
                        if (GUILayout.Button("Create New Mask Texture")) {
                            if (EditorUtility.DisplayDialog("Create Mask Texture", "A texture asset will be created with a size of " + maskTextureResolution.intValue + "x" + maskTextureResolution.intValue + ".\n\nContinue?", "Ok", "Cancel")) {
                                CreateNewMaskTexture();
                            }
                        }
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.Space();
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Separator();
                        EditorGUILayout.LabelField("Coverage Mask Mapping");
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(coverageMaskWorldSize, new GUIContent("World Size", "Mapping of the texture against the world in world units. Usually this should match terrain size."));
                        EditorGUILayout.PropertyField(coverageMaskWorldCenter, new GUIContent("World Center", "Mapping of the texture center against the world in world units. Use this as an offset to apply coverage mask over a certain area."));
                        EditorGUI.indentLevel--;
                        EditorGUILayout.PropertyField(maskBrushMode, new GUIContent("Brush Mode", "Select brush operation mode."));
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(maskBrushWidth, new GUIContent("Width", "Width of the snow editor brush."));
                        EditorGUILayout.PropertyField(maskBrushFuzziness, new GUIContent("Fuzziness", "Solid vs spray brush."));
                        EditorGUILayout.PropertyField(maskBrushOpacity, new GUIContent("Opacity", "Stroke opacity."));
                        EditorGUILayout.BeginHorizontal();
                        if (tex == null) GUI.enabled = false;
                        if (GUILayout.Button("Fill Mask")) {
                            FillMaskTexture(255);
                        }
                        if (GUILayout.Button("Clear Mask")) {
                            FillMaskTexture(0);
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                        EditorGUILayout.Separator();
                        snowMR = (MeshRenderer)EditorGUILayout.ObjectField(new GUIContent("Object Snower", "Add snow on the selected object space."), snowMR, typeof(MeshRenderer), true);
                        GUI.enabled = snowMR != null;
                        EditorGUI.indentLevel++;
                        objectSnowerBorder = EditorGUILayout.FloatField("Padding", objectSnowerBorder);
                        objectSnowerOpacity = EditorGUILayout.Slider("Opacity", objectSnowerOpacity, 0, 1f);
                        EditorGUILayout.BeginHorizontal();
                        if (GUILayout.Button("Cover With Snow")) {
                            FillObjectWithSnow(255, objectSnowerOpacity, objectSnowerBorder);
                        }
                        if (GUILayout.Button("Clear Snow")) {
                            FillObjectWithSnow(0, objectSnowerOpacity, objectSnowerBorder);
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                        GUI.enabled = true;
                        EditorGUILayout.Separator();
                    }
                }
            }

            EditorGUILayout.Separator();

            if (Event.current.type == EventType.ExecuteCommand || Event.current.type == EventType.ValidateCommand) {
                forceUpdateProperties = true;
            }

            if (serializedObject.ApplyModifiedProperties() || forceUpdateProperties) {
                if (profileChanged) {
                    gs.profile.ApplyTo(gs);
                } else {
                    gs.UpdateMaterialPropertiesNow();
                }
                EditorUtility.SetDirty(gs);
                if (cameraFrostedChanged) {
                    cameraFrostedChanged = false;
                    GUIUtility.ExitGUI();
                }
            }
        }

        Texture2D MakeTex(int width, int height, Color col) {
            Color[] pix = new Color[width * height];

            for (int i = 0; i < pix.Length; i++)
                pix[i] = col;

            TextureFormat tf = SystemInfo.SupportsTextureFormat(TextureFormat.RGBAFloat) ? TextureFormat.RGBAFloat : TextureFormat.RGBA32;
            Texture2D result = new Texture2D(width, height, tf, false);
            result.hideFlags = HideFlags.DontSave;
            result.SetPixels(pix);
            result.Apply();

            return result;
        }


        private void OnSceneGUI() {
            Event e = Event.current;
            if (gs == null || !maskEditorEnabled.boolValue || e == null) return;

            Camera sceneCamera = null;
            SceneView sceneView = SceneView.lastActiveSceneView;
            if (sceneView != null) sceneCamera = sceneView.camera;
            if (sceneCamera == null) return;

            Vector2 mousePos = Event.current.mousePosition;
            if (mousePos.x < 0 || mousePos.x > sceneCamera.pixelWidth || mousePos.y < 0 || mousePos.y > sceneCamera.pixelHeight) return;

            Selection.activeGameObject = gs.gameObject;
            gs.UpdateMaterialProperties();

            Ray ray = HandleUtility.GUIPointToWorldRay(mousePos);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo)) {
                float handleSize = HandleUtility.GetHandleSize(hitInfo.point);
                Handles.color = new Color(0, 0, 1, 0.5f);
                Handles.SphereHandleCap(0, hitInfo.point, Quaternion.identity, handleSize, EventType.Repaint);
                HandleUtility.Repaint();

                if (e.isMouse && e.button == 0) {
                    var controlID = GUIUtility.GetControlID(FocusType.Passive);
                    var eventType = e.GetTypeForControl(controlID);

                    if (eventType == EventType.MouseDown) {
                        GUIUtility.hotControl = controlID;
                        mouseIsDown = true;
                        PaintOnMaskPosition(hitInfo.point);
                    } else if (eventType == EventType.MouseUp) {
                        GUIUtility.hotControl = controlID;
                        mouseIsDown = false;
                    }

                    if (mouseIsDown && eventType == EventType.MouseDrag) {
                        GUIUtility.hotControl = controlID;
                        PaintOnMaskPosition(hitInfo.point);
                    }
                }
            }
        }


        #region Mask Texture support functions


        public void CheckCoverageTextureImportSettings(Texture2D tex) {
            if (Application.isPlaying || tex == null)
                return;
            string path = AssetDatabase.GetAssetPath(tex);
            if (string.IsNullOrEmpty(path))
                return;
            TextureImporter imp = TextureImporter.GetAtPath(path) as TextureImporter;
            if (imp == null)
                return;
            imp.ReadTextureSettings(settings);
            if (settings.textureType != TextureImporterType.SingleChannel || settings.singleChannelComponent != TextureImporterSingleChannelComponent.Alpha || settings.sRGBTexture || settings.wrapMode != TextureWrapMode.Clamp || settings.filterMode != FilterMode.Bilinear) {
                EditorGUILayout.HelpBox("Texture has invalid import settings.", MessageType.Warning);
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Fix Texture Import Settings", GUILayout.Width(200))) {
                    settings.textureType = TextureImporterType.SingleChannel;
                    settings.singleChannelComponent = TextureImporterSingleChannelComponent.Alpha;
                    settings.sRGBTexture = false;
                    settings.wrapMode = TextureWrapMode.Clamp;
                    settings.filterMode = FilterMode.Bilinear;
                    settings.alphaSource = TextureImporterAlphaSource.FromInput;
                    settings.alphaIsTransparency = true;
                    settings.readable = true;
                    settings.aniso = 0;
                    imp.SetTextureSettings(settings);
                    imp.SaveAndReimport();
                    forceUpdateProperties = true;
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Separator();
            }
        }

        private void CreateNewMaskTexture() {
            int res = Mathf.Clamp(maskTextureResolution.intValue, 256, 8192);
            Texture2D tex = new Texture2D(res, res, TextureFormat.R8, false, true);
            tex.wrapMode = TextureWrapMode.Clamp;
            coverageMaskTexture.objectReferenceValue = tex;
            coverageMask.boolValue = true;
            serializedObject.ApplyModifiedProperties();
            FillMaskTexture(255);
            AssetDatabase.CreateAsset(tex, "Assets/SnowMaskTexture.asset");
            AssetDatabase.SaveAssets();
        }

        private void PaintOnMaskPosition(Vector3 pos) {
            // Get texture location
            Texture2D currentMaskTexture = (Texture2D)coverageMaskTexture.objectReferenceValue;
            if (currentMaskTexture == null) {
                EditorUtility.DisplayDialog("Global Snow Mask Editor", "Create or assign a coverage mask texture in Global Snow inspector before painting!", "Ok");
                return;
            }
            byte value = maskBrushMode.intValue == (int)MaskTextureBrushMode.AddSnow ? (byte)255 : (byte)0;
            gs.MaskPaint(pos, value, maskBrushWidth.floatValue, maskBrushOpacity.floatValue * 0.2f, maskBrushFuzziness.floatValue);
            EditorUtility.SetDirty(currentMaskTexture);
        }

        void FillMaskTexture(byte value) {
            gs.MaskClear(value);
            Texture2D currentMaskTexture = (Texture2D)coverageMaskTexture.objectReferenceValue;
            EditorUtility.SetDirty(currentMaskTexture);
        }

        void FillObjectWithSnow(byte value, float opacity, float border) {
            gs.MaskFillArea(snowMR, value, opacity, border);
            Texture2D currentMaskTexture = (Texture2D)coverageMaskTexture.objectReferenceValue;
            EditorUtility.SetDirty(currentMaskTexture);
        }


        #endregion

        #region Profile

        void CreateSnowProfile() {

            string path = "Assets";
            foreach (Object obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets)) {
                path = AssetDatabase.GetAssetPath(obj);
                if (File.Exists(path)) {
                    path = Path.GetDirectoryName(path);
                }
                break;
            }
            GlobalSnow snow = (GlobalSnow)target;
            GlobalSnowProfile gsp = CreateInstance<GlobalSnowProfile>();
            gsp.name = "New Global Snow Profile";
            gsp.CopyFrom(snow);
            AssetDatabase.CreateAsset(gsp, path + "/" + gsp.name + ".asset");
            AssetDatabase.SaveAssets();
            profile.objectReferenceValue = gsp;
            EditorGUIUtility.PingObject(gsp);
        }

        void LoadProfile() {
            GlobalSnowProfile profile = (GlobalSnowProfile)this.profile.objectReferenceValue;
            if (profile == null) return;
            GlobalSnow snow = (GlobalSnow)target;
            profile.ApplyTo(snow);
            EditorUtility.SetDirty(snow);
        }

        void SaveToProfile() {
            GlobalSnowProfile profile = (GlobalSnowProfile)this.profile.objectReferenceValue;
            if (profile == null) return;
            GlobalSnow snow = (GlobalSnow)target;
            profile.CopyFrom(snow);
            EditorUtility.SetDirty(profile);
        }

        #endregion

    }

}
