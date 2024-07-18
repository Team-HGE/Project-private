//------------------------------------------------------------------------------------------------------------------
// Global Snow
// Created by Ramiro Oliva (Kronnect)
//------------------------------------------------------------------------------------------------------------------
using UnityEngine;
using UnityEngine.Rendering;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

namespace GlobalSnowEffect {

    public enum MaskTextureBrushMode {
        AddSnow = 0,
        RemoveSnow = 1
    }

    public enum GroundCheck {
        None = 0,
        CharacterController = 1,
        RayCast = 2
    }

    public enum DecalTextureResolution {
        _512 = 512,
        _1024 = 1024,
        _2048 = 2048,
        _4096 = 4096,
        _8192 = 8192
    }


    public enum BordersEffect {
        None,
        Smooth
    }

    public enum SnowQuality {
        FlatShading = 10,
        NormalMapping = 20,
        ReliefMapping = 30
    }

    public enum SnowCoverageUpdateMethod {
        EveryFrame,
        Discrete,
        Manual,
        Disabled
    }

    public enum MaskPaintMethod {
        MainThread,
        BackgroundThread,
        GPU
    }

    struct SnowColliderInfo {
        public Vector3 position;
        public Vector3 forward;
        public float markSize;
    }

    public delegate void OnUpdatePropertiesEvent();
    public delegate void OnUpdateCoverageEvent();

    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    [DisallowMultipleComponent]
    [HelpURL("https://kronnect.com/guides-category/global-snow/")]
    public partial class GlobalSnow : MonoBehaviour {

        const string ZENITH_CAM = "GlobalSnowZenithCam";
        const string REP_CAM = "GlobalSnowRepCam";
        const string SNOW_PARTICLE_SYSTEM = "SnowParticleSystem";
        const string SNOW_DUST_SYSTEM = "SnowDustSystem";
        const int FOOTPRINT_TEXTURE_RESOLUTION = 2048;
        const int SNOW_PARTICLES_LAYER = 2;
        const int UI_LAYER = 5;

        #region Public properties

        public OnUpdatePropertiesEvent OnUpdateProperties;
        public OnUpdateCoverageEvent OnBeforeUpdateCoverage;
        public OnUpdateCoverageEvent OnPostUpdateCoverage;
        static GlobalSnow _snow;

        public static GlobalSnow instance {
            get {
                if (_snow == null) {
                    if (Camera.main != null)
                        _snow = Camera.main.GetComponent<GlobalSnow>();
                    if (_snow == null) {
                        foreach (Camera camera in Camera.allCameras) {
                            _snow = camera.GetComponent<GlobalSnow>();
                            if (_snow != null)
                                break;
                        }
                    }
                }
                return _snow;
            }
        }
        [SerializeField]
        Transform 
            
            blockCelling;

        [SerializeField]
        GameObject
                        _sun;

        public GameObject sun {
            get { return _sun; }
            set {
                if (value != _sun) {
                    _sun = value;
                }
            }
        }


        [SerializeField]
        bool _showSnowInSceneView = true;

        public bool showSnowInSceneView {
            get { return _showSnowInSceneView; }
            set {
                if (value != _showSnowInSceneView) {
                    _showSnowInSceneView = value;
                    UpdateMaterialProperties();
                }
            }
        }



        [SerializeField]
        bool _addSnowToGrass;

        public bool addSnowToGrass {
            get { return _addSnowToGrass; }
            set {
                if (value != _addSnowToGrass) {
                    _addSnowToGrass = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        GlobalSnowProfile _profile;

        public GlobalSnowProfile profile {
            get { return _profile; }
            set {
                if (value != _profile) {
                    _profile = value;
                    if (_profile != null) {
                        _profile.ApplyTo(this);
                    }
                }
            }
        }


        [SerializeField]
        float _minimumAltitude = -10f;

        public float minimumAltitude {
            get { return _minimumAltitude; }
            set {
                if (value != _minimumAltitude) {
                    _minimumAltitude = value;
                    UpdateSnowData2(_minimumAltitude);
                }
            }
        }


        [SerializeField]
        float _minimumAltitudeVegetationOffset = 0f;

        public float minimumAltitudeVegetationOffset {
            get { return _minimumAltitudeVegetationOffset; }
            set {
                if (value != _minimumAltitudeVegetationOffset) {
                    _minimumAltitudeVegetationOffset = value;
                    UpdateSnowData2(_minimumAltitude);
                }
            }
        }

        [SerializeField]
        Color _snowTint = Color.white;

        public Color snowTint {
            get { return _snowTint; }
            set {
                if (value != _snowTint) {
                    _snowTint = value;
                    UpdateSnowTintColor();
                }
            }
        }



        [SerializeField]
        [Range(0, 250f)]
        float _altitudeScatter = 20f;

        public float altitudeScatter {
            get { return _altitudeScatter; }
            set {
                if (value != _altitudeScatter) {
                    _altitudeScatter = value;
                    UpdateSnowData2(_minimumAltitude);
                }
            }
        }


        [SerializeField]
        [Range(0, 500f)]
        float _altitudeBlending = 25f;

        public float altitudeBlending {
            get { return _altitudeBlending; }
            set {
                if (value != _altitudeBlending) {
                    _altitudeBlending = value;
                    UpdateSnowData6();
                }
            }
        }

        [SerializeField]
        bool _useZenithalCoverage = true;

        public bool useZenithalCoverage {
            get { return _showSnowInSceneView; }
            set {
                if (value != _useZenithalCoverage) {
                    _useZenithalCoverage = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        bool _showCoverageGizmo = false;

        public bool showCoverageGizmo {
            get { return _showCoverageGizmo; }
            set {
                if (value != _showCoverageGizmo) {
                    _showCoverageGizmo = value;
                }
            }
        }

        [SerializeField]
        [Range(1, 4)]
        int _coverageExtension = 1;

        public int coverageExtension {
            get { return _coverageExtension; }
            set {
                if (value != _coverageExtension) {
                    _coverageExtension = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        [Range(1, 4)]
        int _coverageResolution = 1;

        public int coverageResolution {
            get { return _coverageResolution; }
            set {
                if (value != _coverageResolution) {
                    _coverageResolution = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0, 0.5f)]
        float _groundCoverage;

        public float groundCoverage {
            get { return _groundCoverage; }
            set {
                if (value != _groundCoverage) {
                    _groundCoverage = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0, 2f)]
        float _groundCoverageRandomization = 0.5f;

        public float groundCoverageRandomization {
            get { return _groundCoverageRandomization; }
            set {
                if (value != _groundCoverageRandomization) {
                    _groundCoverageRandomization = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0, 1f)]
        float _slopeThreshold = 0.7f;

        public float slopeThreshold {
            get { return _slopeThreshold; }
            set {
                if (value != _slopeThreshold) {
                    _slopeThreshold = value;
                    UpdateSnowData5();
                }
            }
        }

        [SerializeField]
        [Range(0, 1f)]
        float _slopeSharpness = 0.5f;

        public float slopeSharpness {
            get { return _slopeSharpness; }
            set {
                if (value != _slopeSharpness) {
                    _slopeSharpness = value;
                    UpdateSnowData5();
                }
            }
        }

        [SerializeField]
        [Range(0, 1f)]
        float _slopeNoise = 0.5f;

        public float slopeNoise {
            get { return _slopeNoise; }
            set {
                if (value != _slopeNoise) {
                    _slopeNoise = value;
                    UpdateSnowData5();
                }
            }
        }




        [SerializeField]
        [Range(0, 2)]
        float _snowNormalsStrength = 1f;

        public float snowNormalsStrength {
            get { return _snowNormalsStrength; }
            set {
                if (value != _snowNormalsStrength) {
                    _snowNormalsStrength = value;
                    UpdateSnowData4();
                }
            }
        }


        [SerializeField]
        float _noiseTexScale = 0.1f;

        public float noiseTexScale {
            get { return _noiseTexScale; }
            set {
                if (value != _noiseTexScale) {
                    _noiseTexScale = value;
                    UpdateSnowData5();
                }
            }
        }


        [SerializeField]
        bool _coverageMask;

        public bool coverageMask {
            get { return _coverageMask; }
            set {
                if (value != _coverageMask) {
                    _coverageMask = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        Texture2D _coverageMaskTexture;

        public Texture2D coverageMaskTexture {
            get { return _coverageMaskTexture; }
            set {
                if (value != _coverageMaskTexture) {
                    _coverageMaskTexture = value;
                    maskColors = null;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        Vector3 _coverageMaskWorldSize = new Vector3(2000, 0, 2000);

        public Vector3 coverageMaskWorldSize {
            get { return _coverageMaskWorldSize; }
            set {
                if (value != _coverageMaskWorldSize) {
                    _coverageMaskWorldSize = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        Vector3 _coverageMaskWorldCenter = new Vector3(0, 0, 0);

        public Vector3 coverageMaskWorldCenter {
            get { return _coverageMaskWorldCenter; }
            set {
                if (value != _coverageMaskWorldCenter) {
                    _coverageMaskWorldCenter = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        bool _coverageMaskFillOutside = true;

        public bool coverageMaskFillOutside {
            get { return _coverageMaskFillOutside; }
            set {
                if (value != _coverageMaskFillOutside) {
                    _coverageMaskFillOutside = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        SnowQuality _snowQuality = SnowQuality.ReliefMapping;

        public SnowQuality snowQuality {
            get { return _snowQuality; }
            set {
                if (value != _snowQuality) {
                    _snowQuality = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        SnowCoverageUpdateMethod _coverageUpdateMethod = SnowCoverageUpdateMethod.Discrete;

        public SnowCoverageUpdateMethod coverageUpdateMethod {
            get { return _coverageUpdateMethod; }
            set {
                if (value != _coverageUpdateMethod) {
                    _coverageUpdateMethod = value;
                }
            }
        }

        public bool coverageDepthDebug;


        [SerializeField]
        [Range(0.05f, 0.3f)]
        float _reliefAmount = 0.3f;

        public float reliefAmount {
            get { return _reliefAmount; }
            set {
                if (value != _reliefAmount) {
                    reliefAmount = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        bool _occlusion = true;

        public bool occlusion {
            get { return _occlusion; }
            set {
                if (value != _occlusion) {
                    _occlusion = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        [Range(0.01f, 5f)]
        float _occlusionIntensity = 1.5f;

        public float occlusionIntensity {
            get { return _occlusionIntensity; }
            set {
                if (value != _occlusionIntensity) {
                    _occlusionIntensity = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        float _glitterStrength = 0.75f;

        public float glitterStrength {
            get { return _glitterStrength; }
            set {
                if (value != _glitterStrength) {
                    _glitterStrength = Mathf.Max(0, value);
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        bool _footprints;

        public bool footprints {
            get { return _footprints; }
            set {
                if (value != _footprints) {
                    _footprints = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        Texture2D _footprintsTexture;

        public Texture2D footprintsTexture {
            get { return _footprintsTexture; }
            set {
                if (value != _footprintsTexture) {
                    _footprintsTexture = value;
                }
            }
        }


        [SerializeField]
        bool _footprintsAutoFPS = true;

        public bool footprintsAutoFPS {
            get { return _footprintsAutoFPS; }
            set {
                if (value != _footprintsAutoFPS) {
                    _footprintsAutoFPS = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(1f, 240f)]
        int _footprintsDuration = 60;

        public int footprintsDuration {
            get { return _footprintsDuration; }
            set {
                if (value != _footprintsDuration) {
                    _footprintsDuration = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        [Range(0.04f, 1f)]
        float _footprintsScale = 1f;

        public float footprintsScale {
            get { return _footprintsScale; }
            set {
                if (value != _footprintsScale) {
                    _footprintsScale = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0.05f, 0.5f)]
        float _footprintsObscurance = 0.1f;

        public float footprintsObscurance {
            get { return _footprintsObscurance; }
            set {
                if (value != _footprintsObscurance) {
                    _footprintsObscurance = value;
                    UpdateMaterialProperties();
                }
            }
        }



        [SerializeField]
        bool _snowfall = true;

        public bool snowfall {
            get { return _snowfall; }
            set {
                if (value != _snowfall) {
                    _snowfall = value;
                    UpdateSnowfallProperties();
                }
            }
        }


        [SerializeField]
        [Range(0.001f, 1f)]
        float _snowfallIntensity = 0.1f;

        public float snowfallIntensity {
            get { return _snowfallIntensity; }
            set {
                if (value != _snowfallIntensity) {
                    _snowfallIntensity = value;
                    UpdateSnowfallProperties();
                }
            }
        }


        [SerializeField]
        float _snowfallSpeed = 1f;

        public float snowfallSpeed {
            get { return _snowfallSpeed; }
            set {
                if (value != _snowfallSpeed) {
                    _snowfallSpeed = value;
                    UpdateSnowfallProperties();
                }
            }
        }




        [SerializeField]
        [Range(0, 2)]
        float _snowfallWind;

        public float snowfallWind {
            get { return _snowfallWind; }
            set {
                if (value != _snowfallWind) {
                    _snowfallWind = value;
                    UpdateSnowfallProperties();
                }
            }
        }


        [SerializeField]
        [Range(10, 200)]
        float _snowfallDistance = 100f;

        public float snowfallDistance {
            get { return _snowfallDistance; }
            set {
                if (value != _snowfallDistance) {
                    _snowfallDistance = value;
                    UpdateSnowfallProperties();
                }
            }
        }


        [SerializeField]
        bool _snowfallUseIllumination = false;

        public bool snowfallUseIllumination {
            get { return _snowfallUseIllumination; }
            set {
                if (value != _snowfallUseIllumination) {
                    _snowfallUseIllumination = value;
                    UpdateSnowfallProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 1f)]
        float _snowdustIntensity;

        public float snowdustIntensity {
            get { return _snowdustIntensity; }
            set {
                if (value != _snowdustIntensity) {
                    _snowdustIntensity = value;
                    UpdateSnowdustProperties();
                }
            }
        }



        [SerializeField]
        [Range(0f, 2f)]
        float _snowdustVerticalOffset = 0.5f;

        public float snowdustVerticalOffset {
            get { return _snowdustVerticalOffset; }
            set {
                if (value != _snowdustVerticalOffset) {
                    _snowdustVerticalOffset = value;
                }
            }
        }



        [SerializeField]
        [Range(0f, 2f)]
        float _maxExposure = 0.85f;

        public float maxExposure {
            get { return _maxExposure; }
            set {
                if (value != _maxExposure) {
                    _maxExposure = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        [Range(0f, 1f)]
        float _smoothness = 0.9f;

        public float smoothness {
            get { return _smoothness; }
            set {
                if (value != _smoothness) {
                    _smoothness = value;
                    UpdateSnowData6();
                }
            }
        }

        [SerializeField]
        [Range(0f, 2f)]
        float _snowAmount = 1f;

        public float snowAmount {
            get { return _snowAmount; }
            set {
                if (value != _snowAmount) {
                    _snowAmount = Mathf.Clamp(value, 0, 2f);
                    UpdateSnowData3();
                    UpdateSnowData4();
                    UpdateSnowData6();
                    UpdateSnowfallProperties();
                }
            }
        }


        [SerializeField]
        bool _cameraFrost = true;

        public bool cameraFrost {
            get { return _cameraFrost; }
            set {
                if (value != _cameraFrost) {
                    _cameraFrost = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        [Range(0.001f, 1.5f)]
        float _cameraFrostIntensity = 0.35f;

        public float cameraFrostIntensity {
            get { return _cameraFrostIntensity; }
            set {
                if (value != _cameraFrostIntensity) {
                    _cameraFrostIntensity = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(1f, 5f)]
        float _cameraFrostSpread = 1.2f;

        public float cameraFrostSpread {
            get { return _cameraFrostSpread; }
            set {
                if (value != _cameraFrostSpread) {
                    _cameraFrostSpread = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        [Range(0f, 1f)]
        float _cameraFrostDistortion = 0.25f;

        public float cameraFrostDistortion {
            get { return _cameraFrostDistortion; }
            set {
                if (value != _cameraFrostDistortion) {
                    _cameraFrostDistortion = value;
                    UpdateMaterialProperties();
                }
            }
        }



        [SerializeField]
        Color _cameraFrostTintColor = Color.white;

        public Color cameraFrostTintColor {
            get { return _cameraFrostTintColor; }
            set {
                if (value != _cameraFrostTintColor) {
                    _cameraFrostTintColor = value;
                    UpdateMaterialProperties();
                }
            }
        }


        public Camera snowCamera { get { return cameraEffect; } }


        [SerializeField]
        [Tooltip("Currently all gameobjects included in the default layer are included. This option is useful to exclude objects belonging to custom layer masks.")]
        LayerMask
                        _layerMask = -1;

        public LayerMask layerMask {
            get { return _layerMask; }
            set {
                if (_layerMask != value) {
                    _layerMask = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        LayerMask
                        _zenithalMask = -1;

        public LayerMask zenithalMask {
            get { return _zenithalMask; }
            set {
                if (_zenithalMask != value) {
                    _zenithalMask = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        int
                        _defaultExclusionLayer = 27;

        public int defaultExclusionLayer {
            get { return _defaultExclusionLayer; }
            set {
                if (_defaultExclusionLayer != value) {
                    _defaultExclusionLayer = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        float _exclusionBias = 0.99f;

        public float exclusionBias {
            get { return _exclusionBias; }
            set {
                if (_exclusionBias != value) {
                    _exclusionBias = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        bool _exclusionDoubleSided;

        public bool exclusionDoubleSided {
            get { return _exclusionDoubleSided; }
            set {
                if (_exclusionDoubleSided != value) {
                    _exclusionDoubleSided = value;
                    needRebuildCommandBuffer = true;
                }
            }
        }



        [SerializeField]
        [Range(0, 1)]
        float _exclusionDefaultCutOff;

        public float exclusionDefaultCutOff {
            get { return _exclusionDefaultCutOff; }
            set {
                if (_exclusionDefaultCutOff != value) {
                    _exclusionDefaultCutOff = value;
                    needRebuildCommandBuffer = true;
                }
            }
        }



        /// <summary>
        /// Only used in deferred
        /// </summary>
        [SerializeField]
        bool _exclusionUseFastMaskShader = true;

        public bool exclusionUseFastMaskShader {
            get { return _exclusionUseFastMaskShader; }
            set {
                if (_exclusionUseFastMaskShader != value) {
                    _exclusionUseFastMaskShader = value;
                    needRebuildCommandBuffer = true;
                }
            }
        }

        [SerializeField]
        bool _preserveGI;

        public bool preserveGI {
            get { return _preserveGI; }
            set {
                if (value != _preserveGI) {
                    _preserveGI = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0, 1)]
        float _minimumGIAmbient = 0.2f;

        public float minimumGIAmbient {
            get { return _minimumGIAmbient; }
            set {
                if (value != _minimumGIAmbient) {
                    _minimumGIAmbient = value;
                    UpdateMaterialProperties();
                }
            }
        }



        [SerializeField]
        bool _terrainMarks;

        public bool terrainMarks {
            get { return _terrainMarks; }
            set {
                if (value != _terrainMarks) {
                    _terrainMarks = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        [Range(1f, 240)]
        int _terrainMarksDuration = 180;

        public int terrainMarksDuration {
            get { return _terrainMarksDuration; }
            set {
                if (value != _terrainMarksDuration) {
                    _terrainMarksDuration = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 1f)]
        float _terrainMarksDefaultSize = 0.25f;

        public float terrainMarksDefaultSize {
            get { return _terrainMarksDefaultSize; }
            set {
                if (value != _terrainMarksDefaultSize) {
                    _terrainMarksDefaultSize = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        float _terrainMarksRoofMinDistance = 0.5f;

        public float terrainMarksRoofMinDistance {
            get { return _terrainMarksRoofMinDistance; }
            set {
                if (value != _terrainMarksRoofMinDistance) {
                    _terrainMarksRoofMinDistance = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        bool _terrainMarksAutoFPS;

        public bool terrainMarksAutoFPS {
            get { return _terrainMarksAutoFPS; }
            set {
                if (value != _terrainMarksAutoFPS) {
                    _terrainMarksAutoFPS = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        [Range(0f, 1024f)]
        float _terrainMarksViewDistance = 200f;

        public float terrainMarksViewDistance {
            get { return _terrainMarksViewDistance; }
            set {
                if (value != _terrainMarksViewDistance) {
                    _terrainMarksViewDistance = value;
                    UpdateMaterialProperties();
                }
            }
        }



        [SerializeField]
        float _terrainMarksStepMaxDistance = 20f;

        public float terrainMarksStepMaxDistance {
            get { return _terrainMarksStepMaxDistance; }
            set {
                if (value != _terrainMarksStepMaxDistance) {
                    _terrainMarksStepMaxDistance = value;
                    UpdateMaterialProperties();
                }
            }
        }



        [SerializeField]
        float _terrainMarksRotationThreshold = 3f;

        public float terrainMarksRotationThreshold {
            get { return _terrainMarksRotationThreshold; }
            set {
                if (value != _terrainMarksRotationThreshold) {
                    _terrainMarksRotationThreshold = value;
                    UpdateMaterialProperties();
                }
            }
        }


        public DecalTextureResolution terrainMarksTextureSize = DecalTextureResolution._2048;


        [SerializeField]
        [Range(0f, 2f)]
        float _billboardCoverage = 1.4f;

        public float billboardCoverage {
            get { return _billboardCoverage; }
            set {
                if (value != _billboardCoverage) {
                    _billboardCoverage = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 1f)]
        float _grassCoverage = 0.75f;

        public float grassCoverage {
            get { return _grassCoverage; }
            set {
                if (value != _grassCoverage) {
                    _grassCoverage = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        GroundCheck _groundCheck = GroundCheck.None;

        public GroundCheck groundCheck {
            get { return _groundCheck; }
            set {
                if (value != _groundCheck) {
                    _groundCheck = value;
                    needFootprintBlit = true;
                }
            }
        }


        [SerializeField]
        CharacterController _characterController;

        public CharacterController characterController {
            get { return _characterController; }
            set {
                if (value != _characterController) {
                    _characterController = value;
                    needFootprintBlit = true;
                }
            }
        }


        [SerializeField]
        float _groundDistance = 1f;

        public float groundDistance {
            get { return _groundDistance; }
            set {
                if (value != _groundDistance) {
                    _grassCoverage = value;
                    UpdateMaterialProperties();
                }
            }
        }



        [SerializeField]
        bool _maskEditorEnabled;

        public bool maskEditorEnabled {
            get { return _maskEditorEnabled; }
            set {
                if (value != _maskEditorEnabled) {
                    _maskEditorEnabled = value;
                }
            }
        }


        [SerializeField]
        int _maskTextureResolution = 1024;

        public int maskTextureResolution {
            get { return _maskTextureResolution; }
            set {
                if (value != _maskTextureResolution) {
                    _maskTextureResolution = value;
                }
            }
        }

        [SerializeField]
        MaskTextureBrushMode _maskBrushMode = MaskTextureBrushMode.RemoveSnow;

        public MaskTextureBrushMode maskBrushMode {
            get { return _maskBrushMode; }
            set {
                if (value != _maskBrushMode) {
                    _maskBrushMode = value;
                }
            }
        }


        [SerializeField, Range(1, 128)]
        float _maskBrushWidth = 20;

        public float maskBrushWidth {
            get { return _maskBrushWidth; }
            set {
                if (value != _maskBrushWidth) {
                    _maskBrushWidth = value;
                }
            }
        }

        [SerializeField, Range(0, 1)]
        float _maskBrushFuzziness = 0.5f;

        public float maskBrushFuzziness {
            get { return _maskBrushFuzziness; }
            set {
                if (value != _maskBrushFuzziness) {
                    _maskBrushFuzziness = value;
                }
            }
        }

        [SerializeField, Range(0, 1)]
        float _maskBrushOpacity = 0.25f;

        public float maskBrushOpacity {
            get { return _maskBrushOpacity; }
            set {
                if (value != _maskBrushOpacity) {
                    _maskBrushOpacity = value;
                }
            }
        }


        #endregion


        // internal fields
        struct ExcludedRenderer {
            public Renderer renderer;
            public bool wasVisible;
            public float exclusionCutOff;
            public bool useFastMaskShader;
        }


        [NonSerialized]
        public Material composeMat, distantSnowMat;
        Material decalMat, blurMat;
        Material snowParticleMat, snowParticleIllumMat;
        Material snowDustMat, snowDustIllumMat;

        GameObject snowCamObj;
        Camera cameraEffect;
        static Camera zenithCam;
        RenderTexture depthTexture, depthTextureBlurred;
        RenderTexture footprintTexture, footprintTexture2, decalTexture, decalTexture2;
        Vector3 lastCameraEffectPosition, lastCameraMarkPosition;
        public Texture2D snowNormalsTex, noiseTex;
        Texture2D snowTex;
        int lastPosX, lastPosZ;
        Vector3 lastTargetPos;

        [NonSerialized]
        public ParticleSystem snowfallSystem;
        float snowfallSystemAltitude = 50f;
        float lastSnowfallIntensity = float.MaxValue;
        float lastSnowfallDistance;
        ParticleSystem.Particle[] m_Particles;
        public ParticleSystem snowdustSystem;

        readonly static List<GlobalSnowIgnoreCoverage> ignoredGOs = new List<GlobalSnowIgnoreCoverage>();
        int currentCoverageResolution, currentCoverageExtension;
        public static bool needUpdateSnowCoverage, needFootprintBlit, needRebuildCommandBuffer;
        bool performFullSceneScan;
        List<Vector4> decalRequests = new List<Vector4>();
        float lastFootprintRemovalTime, lastMarkRemovalTime;
        Quaternion lastSunRotation;
        bool sunOccluded;
        bool needsUpdateProperties;
        CommandBuffer cbufExcluded, cbufCameraMatrices;
        Material cbMatExcludedObjects;
        Vector4[] targetUVArray;
        Vector4[] worldPosArray;

        ExcludedRenderer[] excludedRenderers;
        int excludedRenderersCount;
        readonly Dictionary<Renderer, int> excludedRenderersDict = new Dictionary<Renderer, int>();

        List<GameObject> sceneGameRootGameObjects;
        List<Renderer> sceneRenderers, tmpRenderers;

        bool requestedFootprint;
        Vector3 requestedFootprintPos, requestedFootprintDir;
        int effectStartFrameCount;

        [NonSerialized]
        public Color32[] maskColors;
        bool needMaskUpdate;

        Material matMaskPaint;
        RenderTexture coverageMaskRT;

        #region Game loop events

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void InitStatics() {
            _snow = null;
            zenithCam = null;
            GlobalSnowIgnoreCoverage[] ics = FindObjectsOfType<GlobalSnowIgnoreCoverage>();
            ignoredGOs.Clear();
            ignoredGOs.AddRange(ics);
        }

        void OnEnable() {

            if (!Application.isPlaying) {
                InitStatics();
            }
            cbufCameraMatrices = new CommandBuffer();

            cameraEffect = gameObject.GetComponent<Camera>();

            effectStartFrameCount = Time.frameCount;
            Transform t = transform;
            while (t.parent != null) {
                t = t.parent;
            }
            _characterController = t.GetComponentInChildren<CharacterController>();
            needFootprintBlit = true; // forces blit footprints if enabled

            lastCameraEffectPosition = Vector3.one * 1000;
            SetupSun();
            UpdateMaterialPropertiesNow();

            RenderPipelineManager.beginCameraRendering += RenderPipelineManager_beginCameraRendering;

            needsUpdateProperties = true;
        }

        private void OnDisable() {
            if (snowfallSystem != null) snowfallSystem.Stop();
            if (snowdustSystem != null) snowdustSystem.Stop();
            RenderPipelineManager.beginCameraRendering -= RenderPipelineManager_beginCameraRendering;
        }


        private void RenderPipelineManager_beginCameraRendering(ScriptableRenderContext context, Camera cam) {
#if UNITY_2023_3_OR_NEWER
            // for render graph
            if (cbufExcluded != null) {
                cbufCameraMatrices.Clear();
                cbufCameraMatrices.SetViewProjectionMatrices(cam.worldToCameraMatrix, cam.projectionMatrix);
                context.ExecuteCommandBuffer(cbufCameraMatrices);
                context.ExecuteCommandBuffer(cbufExcluded);
            }
#endif
        }

        private void OnValidate() {
            _glitterStrength = Mathf.Max(0, glitterStrength);
            _exclusionBias = Mathf.Max(0, _exclusionBias);
        }

        void LoadResources() {
            performFullSceneScan = true;
            RebuildCommandBuffer();

            if (decalMat == null) {
                Shader decalShader = Shader.Find("Hidden/GlobalSnow/DecalDraw");
                if (!decalShader.isSupported) {
                    Debug.LogError("Decal Drawing not supported on this platform.");
                    enabled = false;
                    return;
                }
                decalMat = new Material(decalShader);
                decalMat.hideFlags = HideFlags.DontSave;
            }

            if (blurMat == null) {
                Shader blurShader = Shader.Find("Hidden/GlobalSnow/DepthBlur");
                if (!blurShader.isSupported) {
                    Debug.LogError("Blur not supported on this platform.");
                    return;
                }
                blurMat = new Material(blurShader);
                blurMat.hideFlags = HideFlags.DontSave;
            }

            if (matMaskPaint == null) {
                Shader maskPaintShader = Shader.Find("GlobalSnow/MaskPaint");
                if (maskPaintShader != null) {
                    matMaskPaint = new Material(maskPaintShader);
                }
            }

            if (snowTex == null) {
                snowTex = Resources.Load<Texture2D>("GlobalSnow/Textures/Snow");
            }
            if (noiseTex == null) {
                noiseTex = Resources.Load<Texture2D>("GlobalSnow/Textures/Noise5");
            }
            if (snowNormalsTex == null) {
                snowNormalsTex = Resources.Load<Texture2D>("GlobalSnow/Textures/Noise5Normals");
            }

            if (snowParticleMat == null) {
                snowParticleMat = Resources.Load<Material>("GlobalSnow/Materials/SnowParticle");
            }
            if (snowParticleIllumMat == null) {
                snowParticleIllumMat = Resources.Load<Material>("GlobalSnow/Materials/SnowParticleIllum");
            }
            if (snowDustMat == null) {
                snowDustMat = Instantiate(Resources.Load<Material>("GlobalSnow/Materials/SnowDust"));
            }
            if (snowDustIllumMat == null) {
                snowDustIllumMat = Instantiate(Resources.Load<Material>("GlobalSnow/Materials/SnowDustIllum"));
            }

            // Override URP resource settings
#if UNITY_EDITOR
#if UNITY_2023_3_OR_NEWER
            UniversalRenderPipelineEditorShaders customResources = GraphicsSettings.GetRenderPipelineSettings<UniversalRenderPipelineEditorShaders>();
            if (customResources != null) {
                Shader grassShader = Shader.Find(_addSnowToGrass ? "GlobalSnow/TerrainEngine/Details/UniversalPipeline/WavingDoublePass" : "Hidden/TerrainEngine/Details/UniversalPipeline/WavingDoublePass");
                if (grassShader != null) {
                    customResources.terrainDetailGrassShader = grassShader;
                }
                Shader grassBillboardShader = Shader.Find(_addSnowToGrass ? "GlobalSnow/TerrainEngine/Details/UniversalPipeline/BillboardWavingDoublePass" : "Hidden/TerrainEngine/Details/UniversalPipeline/BillboardWavingDoublePass");
                if (grassBillboardShader != null) {
                    customResources.terrainDetailGrassBillboardShader = grassBillboardShader;
                }
            }
#else
            UniversalRenderPipelineEditorResources customResources = Resources.Load<UniversalRenderPipelineEditorResources>("GlobalSnow/Data/URPEditorResources"); ;
            if (customResources != null) {
                UniversalRenderPipelineAsset pipeAsset = (UniversalRenderPipelineAsset)GraphicsSettings.renderPipelineAsset;
                if (pipeAsset != null) {
                    if (_addSnowToGrass) {
                        pipeAsset.m_EditorResourcesAsset = customResources;
                    } else if (pipeAsset.m_EditorResourcesAsset == customResources) {
                        pipeAsset.m_EditorResourcesAsset = null;
                    }
                }
            }
#endif
#endif

            // Set global textures for replacement shader
            Shader.SetGlobalTexture(ShaderParams.GlobalSnowTex, snowTex);
            Shader.SetGlobalTexture(ShaderParams.GlobalSnowNormalsTex, snowNormalsTex);
            Shader.SetGlobalTexture(ShaderParams.GlobalNoiseTex, noiseTex);
        }


        public CommandBuffer GetExclusionCommandBuffer() {
            return cbufExcluded;
        }


        void RebuildCommandBuffer() {
            needRebuildCommandBuffer = false;

            if (cbufExcluded == null) {
                cbufExcluded = new CommandBuffer();
                cbufExcluded.name = "Global Snow Exclusion Mask";
            } else {
                cbufExcluded.Clear();
            }

            if (cbMatExcludedObjects == null) {
                cbMatExcludedObjects = new Material(Shader.Find("Hidden/GlobalSnow/DeferredMaskWrite"));
            }
            cbMatExcludedObjects.SetInt(ShaderParams.EraseCullMode, _exclusionDoubleSided ? (int)CullMode.Off : (int)CullMode.Back);

            ExcludedRenderer excludedRenderer = new ExcludedRenderer();

            Scene scene = SceneManager.GetActiveScene();
            if (!scene.isLoaded) {
                needRebuildCommandBuffer = true;
                performFullSceneScan = true;
            } else {
                // Get all renderers not included in snow layer mask
                if (performFullSceneScan) {
                    FindExcludedRenderersInScene(scene);
                }
                bool isUnsafeTime = IsUnSafeTime();
                // Exclude objects using GlobalSnowIgnoreCoverate script
                int count = ignoredGOs.Count;
                for (int k = 0; k < count; k++) {
                    GlobalSnowIgnoreCoverage ic = ignoredGOs[k];
                    if (ic != null && ic.isActiveAndEnabled && ic.renderers != null) {
                        int rrCount = ic.renderers.Length;
                        for (int j = 0; j < rrCount; j++) {
                            Renderer r = ic.renderers[j];
                            if (r.enabled && r.gameObject.activeInHierarchy) {
                                int index;
                                if (!excludedRenderersDict.TryGetValue(r, out index)) {
                                    if (!ic.receiveSnow) {
                                        excludedRenderer.renderer = r;
                                        excludedRenderer.exclusionCutOff = ic.exclusionCutOff;
                                        excludedRenderer.useFastMaskShader = ic.useFastMaskShader;
                                        excludedRenderer.wasVisible = r.isVisible || isUnsafeTime; // r.enabled && r.gameObject.activeInHierarchy;
                                        GrowIfNeeded(ref excludedRenderers, excludedRenderersCount);
                                        excludedRenderers[excludedRenderersCount] = excludedRenderer;
                                        excludedRenderersDict[r] = excludedRenderersCount;
                                        excludedRenderersCount++;
                                    }
                                } else if (ic.receiveSnow) {
                                    excludedRenderers[index].renderer = null;
                                    excludedRenderersDict.Remove(r);
                                    excludedRenderers[index].exclusionCutOff = 0;
                                } else {
                                    excludedRenderers[index].exclusionCutOff = ic.exclusionCutOff;
                                    excludedRenderers[index].useFastMaskShader = ic.useFastMaskShader;
                                }
                            }
                        }
                    }
                }

                // Exclude scene objects not in layer mask
                RenderTextureDescriptor rtDesc = GlobalSnowRenderFeature.mainCameraDescriptor;
                if (rtDesc.width == 0) {
                    rtDesc = new RenderTextureDescriptor(cameraEffect.pixelWidth, cameraEffect.pixelHeight);
                }
                rtDesc.depthBufferBits = 0;
                rtDesc.sRGB = false;
                rtDesc.msaaSamples = 1;
                rtDesc.useMipMap = false;
                rtDesc.volumeDepth = 1;

                // Draw excluded objects
                RenderTextureDescriptor rtMaskDesc = rtDesc;
                rtMaskDesc.colorFormat = RenderTextureFormat.Depth;
                rtMaskDesc.depthBufferBits = 24;

                cbufExcluded.GetTemporaryRT(ShaderParams.ExclusionTex, rtMaskDesc);
                RenderTargetIdentifier rtExclusion = new RenderTargetIdentifier(ShaderParams.ExclusionTex, 0, CubemapFace.Unknown, -1);
                cbufExcluded.SetRenderTarget(rtExclusion);
                cbufExcluded.ClearRenderTarget(true, false, Color.white);

                for (int k = 0; k < excludedRenderersCount; k++) {
                    Renderer r = excludedRenderers[k].renderer;
                    if (r != null && (r.isVisible || isUnsafeTime)) {
                        AddRendererToCommandBuffer(r, cbMatExcludedObjects, excludedRenderers[k].exclusionCutOff, excludedRenderers[k].useFastMaskShader);
                    }
                }
            }
        }

        void GrowIfNeeded<T>(ref T[] array, int index) {
            if (index >= array.Length) {
                T[] newArray = new T[index * 2];
                Array.Copy(array, newArray, array.Length);
                array = newArray;
            }
        }


        void AddRendererToCommandBuffer(Renderer r, Material mat, float cutOff, bool useFastMaskShader) {
            if (cutOff > 0 && useFastMaskShader) {
                if (r.sharedMaterials != null) {
                    cbufExcluded.SetGlobalFloat(ShaderParams.MaskCutOff, cutOff);
                    for (int k = 0; k < r.sharedMaterials.Length; k++) {
                        Material originalMat = r.sharedMaterials[k];
                        if (originalMat.HasProperty(ShaderParams.MainTex)) {
                            cbufExcluded.SetGlobalTexture(ShaderParams.MaskTex, originalMat.mainTexture);
                        }
                        cbufExcluded.DrawRenderer(r, mat, k);
                    }
                }
            } else {
                cbufExcluded.SetGlobalFloat(ShaderParams.MaskCutOff, 0f);
                int subMeshCount = 0;
                if (r is SkinnedMeshRenderer) {
                    SkinnedMeshRenderer sm = (SkinnedMeshRenderer)r;
                    if (sm.sharedMesh != null) {
                        subMeshCount = sm.sharedMesh.subMeshCount;
                    }
                } else if (r is MeshRenderer) {
                    MeshFilter mf = r.GetComponent<MeshFilter>();
                    if (mf != null && mf.sharedMesh != null) {
                        subMeshCount = mf.sharedMesh.subMeshCount;
                    }
                }
                for (int l = 0; l < subMeshCount; l++) {
                    Material maskMat = mat;
                    if (!useFastMaskShader && l < r.sharedMaterials.Length) {
                        maskMat = r.sharedMaterials[l];
                    }
                    if (maskMat == null) continue;
                    cbufExcluded.DrawRenderer(r, maskMat, l);
                }
            }
        }


        bool IsUnSafeTime() {
            return (Time.frameCount - effectStartFrameCount) < 5;
        }

        void FindExcludedRenderersInScene(Scene scene) {
            if (Time.frameCount >= effectStartFrameCount) { // repeat scan next frames so it gives time to initialization/load of other objects
                performFullSceneScan = false;
            }

            if (excludedRenderers == null) {
                excludedRenderers = new ExcludedRenderer[64];
            }
            excludedRenderersCount = 0;

            if (sceneGameRootGameObjects == null) {
                sceneGameRootGameObjects = new List<GameObject>(256);
            } else {
                sceneGameRootGameObjects.Clear();
            }
            if (tmpRenderers == null) {
                tmpRenderers = new List<Renderer>();
            } else {
                tmpRenderers.Clear();
            }
            if (sceneRenderers == null) {
                sceneRenderers = new List<Renderer>();
            } else {
                sceneRenderers.Clear();
            }
            excludedRenderersDict.Clear();

            scene.GetRootGameObjects(sceneGameRootGameObjects);
            int count = sceneGameRootGameObjects.Count;
            for (int k = 0; k < count; k++) {
                GameObject o = sceneGameRootGameObjects[k];
                o.GetComponentsInChildren(true, tmpRenderers);
                sceneRenderers.AddRange(tmpRenderers);
            }

            ExcludedRenderer excludedRenderer = new ExcludedRenderer();
            count = sceneRenderers.Count;
            for (int k = 0; k < count; k++) {
                Renderer r = sceneRenderers[k];
                if (r.enabled && r.gameObject.activeInHierarchy) {
                    int objLayer = r.gameObject.layer;
                    if (objLayer != 0 && objLayer != SNOW_PARTICLES_LAYER && (objLayer == _defaultExclusionLayer || ((1 << objLayer) & _layerMask.value) == 0)) {
                        excludedRenderer.renderer = r;
                        excludedRenderer.wasVisible = r.isVisible || IsUnSafeTime();
                        excludedRenderer.exclusionCutOff = _exclusionDefaultCutOff;
                        excludedRenderer.useFastMaskShader = _exclusionUseFastMaskShader;
                        GrowIfNeeded(ref excludedRenderers, excludedRenderersCount);
                        excludedRenderersDict[r] = excludedRenderersCount;
                        excludedRenderers[excludedRenderersCount] = excludedRenderer;
                        excludedRenderersCount++;
                    }
                }
            }
        }



        void Reset() {
            SetupSun();
            UpdateMaterialProperties();
        }


        void OnDestroy() {
            CleanUpCoverageMaskRT();
            CleanUpTextureDepth();
            if (snowCamObj != null) {
                DestroyImmediate(snowCamObj);
                snowCamObj = null;
            }
            if (footprintTexture != null) {
                footprintTexture.Release();
                footprintTexture = null;
            }
            if (footprintTexture2 != null) {
                footprintTexture2.Release();
                footprintTexture2 = null;
            }
            if (decalTexture != null) {
                decalTexture.Release();
                decalTexture = null;
            }
            if (decalTexture2 != null) {
                decalTexture2.Release();
                decalTexture2 = null;
            }
            if (snowfallSystem != null) {
                DestroyImmediate(snowfallSystem.gameObject);
                snowfallSystem = null;
            }
            if (snowdustSystem != null) {
                DestroyImmediate(snowdustSystem.gameObject);
                snowdustSystem = null;
            }
            if (blurMat != null) {
                DestroyImmediate(blurMat);
            }
            if (snowDustMat != null) {
                DestroyImmediate(snowDustMat);
            }
            if (snowDustIllumMat != null) {
                DestroyImmediate(snowDustIllumMat);
            }
            if (decalMat != null) {
                DestroyImmediate(decalMat);
            }
            if (zenithCam != null) {
                ReleaseZenithCam();
            }
        }

        void CleanUpCoverageMaskRT() {
            if (coverageMaskRT != null) {
                coverageMaskRT.Release();
                coverageMaskRT = null;
            }
        }

        void UpdateSnowCoverageNow() {

            needUpdateSnowCoverage = false;

            UpdateSnowGlobalCoverageMaskTexture();

            const float camAltitudeOffset = 100f;
            Vector3 currentSnowCamPosition = new Vector3((int)cameraEffect.transform.position.x, cameraEffect.transform.position.y + camAltitudeOffset, (int)cameraEffect.transform.position.z);
            float farClipPlane = Mathf.Max(currentSnowCamPosition.y - _minimumAltitude, 0.01f) + 1f;
            Shader.SetGlobalVector(ShaderParams.GlobalCamPos, new Vector4(currentSnowCamPosition.x, currentSnowCamPosition.y, currentSnowCamPosition.z, farClipPlane));

            if (!_useZenithalCoverage) {
                ReleaseZenithCam();
                return;
            }

            // Setup zenith cam
            if (currentCoverageResolution != _coverageResolution || currentCoverageExtension != _coverageExtension) {
                ReleaseZenithCam();
            }
            if (zenithCam == null) {
                GameObject camGO = GameObject.Find(ZENITH_CAM);
                if (camGO == null) {
                    camGO = new GameObject(ZENITH_CAM);
                    zenithCam = camGO.AddComponent<Camera>();
                } else {
                    zenithCam = camGO.GetComponent<Camera>();
                }
                camGO.hideFlags = HideFlags.DontSave; // | HideFlags.HideInHierarchy;
                zenithCam.enabled = false;
                zenithCam.renderingPath = RenderingPath.Forward;
                zenithCam.orthographic = true;
                zenithCam.depthTextureMode = DepthTextureMode.None;
                zenithCam.clearFlags = CameraClearFlags.SolidColor;
                zenithCam.allowMSAA = false;
                zenithCam.backgroundColor = new Color(1f, 0f, 0f, 0f);
                currentCoverageResolution = _coverageResolution;
                currentCoverageExtension = _coverageExtension;

                UniversalAdditionalCameraData camData = zenithCam.GetComponent<UniversalAdditionalCameraData>();
                if (camData == null) {
                    camData = zenithCam.gameObject.AddComponent<UniversalAdditionalCameraData>();
                }
                if (camData != null) {
                    camData.dithering = false;
                    camData.renderPostProcessing = false;
                    camData.renderShadows = false;
                    camData.requiresColorTexture = false;
                    camData.requiresDepthTexture = false;
                    camData.stopNaN = false;
                    camData.volumeLayerMask = 0;
                    CheckAndAssignDepthRenderer(camData);
                }
            }
            float coverageWorldSize = Mathf.Pow(2, 7f + _coverageExtension);
            zenithCam.orthographicSize = coverageWorldSize;
            zenithCam.cullingMask = zenithalMask & ~(1 << SNOW_PARTICLES_LAYER) & ~(1 << _defaultExclusionLayer);
            zenithCam.nearClipPlane = 1f;
            zenithCam.farClipPlane = farClipPlane;
            zenithCam.transform.position = currentSnowCamPosition;
            zenithCam.transform.rotation = Quaternion.Euler(90, 0, 0);

            // Render from above
            int res = (int)Mathf.Pow(2, _coverageResolution + 8);
            if (depthTexture == null) {
                depthTexture = new RenderTexture(res, res, 24, RenderTextureFormat.Depth, RenderTextureReadWrite.Linear);
                depthTexture.antiAliasing = 1;
                depthTexture.hideFlags = HideFlags.DontSave;
                depthTexture.filterMode = FilterMode.Bilinear;
                depthTexture.wrapMode = TextureWrapMode.Clamp;
                depthTexture.Create();
            }
            zenithCam.targetTexture = depthTexture;
            float prevLODBias = QualitySettings.lodBias;
            QualitySettings.lodBias = 1000;
            zenithCam.Render();
            QualitySettings.lodBias = prevLODBias;

            // smooth coverage
            RenderTexture active = RenderTexture.active;
            RenderTexture rt1 = RenderTexture.GetTemporary(res, res, 0, RenderTextureFormat.RHalf, RenderTextureReadWrite.Linear);
            blurMat.SetFloat(ShaderParams.CoverageWorldSize, coverageWorldSize);
            blurMat.SetFloat(ShaderParams.GroundCoverageRandomization, _groundCoverageRandomization);
            blurMat.SetFloat(ShaderParams.BlurSpread, 1.0f + _groundCoverageRandomization);
            Graphics.Blit(depthTexture, rt1, blurMat, 0);
            if (depthTextureBlurred == null) {
                depthTextureBlurred = new RenderTexture(res, res, 0, RenderTextureFormat.RHalf, RenderTextureReadWrite.Linear);
                depthTextureBlurred.antiAliasing = 1;
                depthTextureBlurred.hideFlags = HideFlags.DontSave;
                depthTextureBlurred.filterMode = FilterMode.Bilinear;
                depthTextureBlurred.wrapMode = TextureWrapMode.Clamp;
            }
            Graphics.Blit(rt1, depthTextureBlurred, blurMat, 1);

            RenderTexture.ReleaseTemporary(rt1);
            RenderTexture.active = active;
            Shader.SetGlobalTexture(ShaderParams.GlobalDepthTex, depthTextureBlurred);
        }

        UniversalRendererData depthRendererData;
        void CheckAndAssignDepthRenderer(UniversalAdditionalCameraData camData) {
            UniversalRenderPipelineAsset pipe = (UniversalRenderPipelineAsset)GraphicsSettings.currentRenderPipeline;
            if (pipe == null) return;

            if (depthRendererData == null) {
                depthRendererData = Resources.Load<UniversalRendererData>("GlobalSnow/Shaders/GlobalSnowZenithalRenderer");
                if (depthRendererData == null) {
                    Debug.LogError("Volumetric Fog Depth Renderer asset not found.");
                    return;
                }
                depthRendererData.postProcessData = null;
            }
            int depthRendererIndex = -1;
            for (int k = 0; k < pipe.m_RendererDataList.Length; k++) {
                if (pipe.m_RendererDataList[k] == depthRendererData) {
                    depthRendererIndex = k;
                    break;
                }
            }
            if (depthRendererIndex < 0) {
                depthRendererIndex = pipe.m_RendererDataList.Length;
                System.Array.Resize<ScriptableRendererData>(ref pipe.m_RendererDataList, depthRendererIndex + 1);
                pipe.m_RendererDataList[depthRendererIndex] = depthRendererData;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(pipe);
#endif
            }
            camData.SetRenderer(depthRendererIndex);
        }


        void RenderFootprints() {
            if (footprintTexture == null || footprintTexture2 == null) {
                footprintTexture = new RenderTexture(FOOTPRINT_TEXTURE_RESOLUTION, FOOTPRINT_TEXTURE_RESOLUTION, 0, RenderTextureFormat.ARGBFloat);
                footprintTexture.filterMode = FilterMode.Point;
                footprintTexture.wrapMode = TextureWrapMode.Repeat;
                footprintTexture.hideFlags = HideFlags.DontSave;
                footprintTexture2 = new RenderTexture(FOOTPRINT_TEXTURE_RESOLUTION, FOOTPRINT_TEXTURE_RESOLUTION, 0, RenderTextureFormat.ARGBFloat);
                footprintTexture2.filterMode = FilterMode.Point;
                footprintTexture2.wrapMode = TextureWrapMode.Repeat;
                footprintTexture2.hideFlags = HideFlags.DontSave;
                Shader.SetGlobalTexture(ShaderParams.GlobalFootprintTex, footprintTexture2);
            }
            Vector3 targetPos = requestedFootprint ? requestedFootprintPos : cameraEffect.transform.position;
            targetPos.x *= 1f / _footprintsScale;
            targetPos.z *= 1f / _footprintsScale;
            int posX = Mathf.FloorToInt(targetPos.x);
            int posZ = Mathf.FloorToInt(targetPos.z);
            bool doBlit = false;
            if (requestedFootprint || (_footprintsAutoFPS && (posX != lastPosX || posZ != lastPosZ || needFootprintBlit))) {
                Vector2 fracPos = new Vector2(targetPos.x - posX, targetPos.z - posZ);
                bool validStep = (lastPosX != posX && lastPosZ != posZ);
                if (!validStep && fracPos.x > 0.25f && fracPos.x < 0.75f && fracPos.y >= 0.25f && fracPos.y < 0.75f) {
                    validStep = true;
                }
                // check correct path
                if (!validStep) {
                    Vector2 vdir = new Vector2(targetPos.x - lastTargetPos.x, targetPos.z - lastTargetPos.z).normalized;
                    Vector2 tdir = new Vector2(posX - lastPosX, posZ - lastPosZ).normalized;
                    const float angleThreshold = 0.9f;
                    if (Vector2.Dot(vdir, tdir) > angleThreshold) {
                        validStep = true;
                    }
                }
                if (validStep) {
                    // check player is grounded
                    if (_groundCheck == GroundCheck.RayCast) {
                        validStep = Physics.Linecast(targetPos, new Vector3(targetPos.x, targetPos.y - _groundDistance, targetPos.z));
                    } else if (_groundCheck == GroundCheck.CharacterController && _characterController != null) {
                        validStep = _characterController.isGrounded;
                    }
                }
                if (validStep || needFootprintBlit) {
                    Vector3 targetDir = requestedFootprint ? requestedFootprintDir : cameraEffect.transform.forward;
                    Vector2 dirxz = new Vector2(targetDir.x, targetDir.z).normalized;
                    float angle = ((UnityEngine.Random.value - 0.5f) * 8f - GetAngle(Vector2.up, dirxz)) * Mathf.Deg2Rad;
                    Vector2 vpos = new Vector2(((posX + FOOTPRINT_TEXTURE_RESOLUTION) % FOOTPRINT_TEXTURE_RESOLUTION) + 0.5f, ((posZ + FOOTPRINT_TEXTURE_RESOLUTION) % FOOTPRINT_TEXTURE_RESOLUTION) + 0.5f);
                    Vector4 targetUV = new Vector4(vpos.x / FOOTPRINT_TEXTURE_RESOLUTION, vpos.y / FOOTPRINT_TEXTURE_RESOLUTION, Mathf.Cos(angle), Mathf.Sin(angle));
                    decalMat.SetVector(ShaderParams.TargetUV, targetUV);
                    if (zenithCam == null) {
                        decalMat.SetVector(ShaderParams.WorldPos, new Vector3(targetPos.x, 0, targetPos.z));
                    } else {
                        decalMat.SetVector(ShaderParams.WorldPos, new Vector3(posX, (zenithCam.transform.position.y - targetPos.y) / zenithCam.farClipPlane, posZ));
                    }
                    float decalRadius = 0.5f / FOOTPRINT_TEXTURE_RESOLUTION;
                    if (validStep) {
                        lastPosX = posX;
                        lastPosZ = posZ;
                        lastTargetPos = targetPos;
                    } else {
                        decalRadius = 0f;
                    }
                    decalMat.SetFloat(ShaderParams.DrawDist, decalRadius);
                    doBlit = true;
                    needFootprintBlit = false;
                }
            }
            float dTime = Time.time - lastFootprintRemovalTime;
            if (dTime > _footprintsDuration / 200f) {
                lastFootprintRemovalTime = Time.time;
                decalMat.SetFloat(ShaderParams.EraseSpeed, Mathf.Max(dTime / _footprintsDuration, 1 / 200f));
                if (!doBlit) {
                    decalMat.SetFloat(ShaderParams.DrawDist, 0);
                    doBlit = true;
                }
            } else {
                decalMat.SetFloat(ShaderParams.EraseSpeed, 0);
            }
            if (doBlit || !Application.isPlaying) {
                Graphics.Blit(footprintTexture, footprintTexture2, decalMat, 0);
                // Flip decal buffer
                RenderTexture decalAux = footprintTexture;
                footprintTexture = footprintTexture2;
                footprintTexture2 = decalAux;
            }
            requestedFootprint = false;
        }


        void RenderTerrainMarks() {
            int textureSize = (int)terrainMarksTextureSize;
            bool usesHighPrecisionTexture = !Application.isMobilePlatform || QualitySettings.activeColorSpace == ColorSpace.Linear;
            if (decalTexture == null || decalTexture2 == null) {
                RenderTextureFormat rtFormat = usesHighPrecisionTexture ? RenderTextureFormat.ARGBFloat : RenderTextureFormat.ARGB32;
                decalTexture = new RenderTexture(textureSize, textureSize, 0, rtFormat);
                decalTexture.wrapMode = TextureWrapMode.Repeat;
                decalTexture.hideFlags = HideFlags.DontSave;
                decalTexture2 = new RenderTexture(textureSize, textureSize, 0, rtFormat);
                decalTexture2.wrapMode = TextureWrapMode.Repeat;
                decalTexture2.hideFlags = HideFlags.DontSave;
                // Clean textures
                Graphics.Blit(decalTexture, decalTexture2, decalMat, 2);
                RenderTexture decalAux = decalTexture;
                decalTexture = decalTexture2;
                decalTexture2 = decalAux;
                Graphics.Blit(decalTexture, decalTexture2, decalMat, 2);
                decalAux = decalTexture;
                decalTexture = decalTexture2;
                decalTexture2 = decalAux;
            }

            bool needCleanBlit = false;
            float dTime = Time.time - lastMarkRemovalTime;
            float eraseSpeed = 0;
            if (dTime > _terrainMarksDuration / 200f) {
                lastMarkRemovalTime = Time.time;
                eraseSpeed = Mathf.Max(dTime / _terrainMarksDuration, 1 / 200f);
                needCleanBlit = true;
            }
            decalMat.SetFloat(ShaderParams.EraseSpeed, eraseSpeed);

            const int MAX_DECAL_BLITS_PER_FRAME = 4;
            const float scale = 8f;
            const int VECTOR_ARRAY_LENGTH = 64;

            if (targetUVArray == null || targetUVArray.Length == 0) {
                targetUVArray = new Vector4[VECTOR_ARRAY_LENGTH];
                worldPosArray = new Vector4[VECTOR_ARRAY_LENGTH];
            }
            int drCount = decalRequests.Count;
            for (int blit = 0; blit < MAX_DECAL_BLITS_PER_FRAME; blit++) {
                int stamps = 0;
                for (int k = 0; k < VECTOR_ARRAY_LENGTH; k++) {
                    if (drCount == 0) {
                        targetUVArray[k] = Vector4.zero;
                        worldPosArray[k] = Vector4.zero;
                        continue;
                    }
                    drCount--;
                    Vector4 vpos = decalRequests[drCount];
                    decalRequests.RemoveAt(drCount);

                    if (_terrainMarksRoofMinDistance > 0) {
                        Vector3 wpos = vpos;
                        wpos.y += _terrainMarksRoofMinDistance;
                        Ray ray = new Ray(wpos, Vector3.up);
                        if (Physics.Raycast(ray)) continue;
                    }

                    float drawDist = vpos.w * scale / textureSize;
                    targetUVArray[k] = new Vector4((((vpos.x + textureSize) * scale) % textureSize) / textureSize,
                                                   (((vpos.z + textureSize) * scale) % textureSize) / textureSize, 0, drawDist * drawDist);

                    if (zenithCam == null) {
                        worldPosArray[stamps] = new Vector4(vpos.x, 0, vpos.z, 0);
                    } else {
                        worldPosArray[stamps] = new Vector4(vpos.x, (zenithCam.transform.position.y - vpos.y) / zenithCam.farClipPlane, vpos.z, 0);
                    }
                    stamps++;
                }
                if (stamps == 0) break;

                decalMat.SetInt(ShaderParams.TargetCount, stamps);
                decalMat.SetVectorArray(ShaderParams.TargetUVArray, targetUVArray);
                decalMat.SetVectorArray(ShaderParams.WorldPosArray, worldPosArray);

                Graphics.Blit(decalTexture, decalTexture2, decalMat, 1);

                // Flip decal buffer
                RenderTexture decalAux = decalTexture;
                decalTexture = decalTexture2;
                decalTexture2 = decalAux;
                needCleanBlit = false;
                decalMat.SetFloat(ShaderParams.EraseSpeed, 0); // don't erase anymore for this frame
            }

            if (needCleanBlit) {
                Graphics.Blit(decalTexture, decalTexture2, decalMat, 3);
                // Flip decal buffer
                RenderTexture decalAux = decalTexture;
                decalTexture = decalTexture2;
                decalTexture2 = decalAux;
            }
            Shader.SetGlobalTexture(ShaderParams.GlobalDecalTex, decalTexture);
        }


        void SnowRender_DeferredPath() {

            // Render footprints
            if (_footprints && decalMat != null)
                RenderFootprints();

            // Render decals
            if (_terrainMarks && decalMat != null)
                RenderTerrainMarks();


        }

        // Do the magic
        void LateUpdate() {

            if (!enabled || !gameObject.activeInHierarchy || cameraEffect == null)
                return;

            if (needsUpdateProperties) {
                needsUpdateProperties = false;
                UpdateMaterialPropertiesNow();
            }

            if (cameraEffect.cameraType == CameraType.SceneView && !showSnowInSceneView) return;

            CheckSunOcclusion();

            // Apply exclusion list
            bool updateCoverage = false;
            if (_coverageUpdateMethod != SnowCoverageUpdateMethod.Disabled) {
                if (depthTexture == null || !depthTexture.IsCreated()) {
                    CleanUpTextureDepth();
                    needUpdateSnowCoverage = true;
                }

                updateCoverage = !Application.isPlaying || needUpdateSnowCoverage || _coverageUpdateMethod == SnowCoverageUpdateMethod.EveryFrame || IsUnSafeTime();
                if (!updateCoverage && _coverageUpdateMethod == SnowCoverageUpdateMethod.Discrete) {
                    updateCoverage = (lastCameraEffectPosition - cameraEffect.transform.position).sqrMagnitude > 2500;
                }
            }

            if (updateCoverage) {
                if (OnBeforeUpdateCoverage != null) OnBeforeUpdateCoverage();
                ToggleIgnoreZenithalCameraGOsLayer(exclude: true);
                lastCameraEffectPosition = cameraEffect.transform.position;
                UpdateSnowCoverageNow();
            }

            // Render snow scene
            float brightness;
            if (_sun != null) {
                Vector3 forward = _sun.transform.forward;
                Shader.SetGlobalVector(ShaderParams.GlobalSunDir, new Vector4(forward.x, forward.y, forward.z, _terrainMarksViewDistance * _terrainMarksViewDistance));
                brightness = Mathf.Clamp01(1.5f * _maxExposure / (_maxExposure + Mathf.Max(0.001f, -forward.y)));
            } else {
                Shader.SetGlobalVector(ShaderParams.GlobalSunDir, new Vector4(0, -0.3f, 0.7f, _terrainMarksViewDistance * _terrainMarksViewDistance));
                brightness = 1f;
            }
            Shader.SetGlobalVector(ShaderParams.GlobalSnowData1, new Vector4(_reliefAmount, _occlusionIntensity, _glitterStrength, brightness));

            SnowRender_DeferredPath();
            CheckCommandBuffer();

            // Restore exclusion list
            if (updateCoverage) {
                ToggleIgnoreZenithalCameraGOsLayer(exclude: false);
                if (OnPostUpdateCoverage != null) OnPostUpdateCoverage();
            }

            if (needMaskUpdate) {
                SubmitCoverageMaskTextureChanges();
            }

        }


        /// <summary>
        /// Toggles exclusion for zenithal camera 
        /// </summary>
        private void ToggleIgnoreZenithalCameraGOsLayer(bool exclude) {
            int ignoredGOsCount = ignoredGOs.Count;
            bool validFrameCount = !IsUnSafeTime();
            for (int k = 0; k < ignoredGOsCount; k++) {
                GlobalSnowIgnoreCoverage igo = ignoredGOs[k];
                if (igo != null && igo.isActiveAndEnabled && !igo.blockSnow) {
                    int renderersCount = igo.renderers.Length;
                    for (int j = 0; j < renderersCount; j++) {
                        Renderer r = igo.renderers[j];
                        if (r == null || !r.enabled || (!r.isVisible && validFrameCount)) continue;
                        if (exclude) {
                            igo.renderersLayers[j] = r.gameObject.layer;
                            r.gameObject.layer = _defaultExclusionLayer;
                        } else {
                            r.gameObject.layer = igo.renderersLayers[j];
                        }
                    }
                }
            }
        }

        void Update() {

            if (cameraEffect != null) {
                if (cameraEffect.cameraType == CameraType.SceneView && !showSnowInSceneView) return;

                if (snowfallSystem != null) {
                    snowfallSystem.transform.position = cameraEffect.transform.position + new Vector3(0, snowfallSystemAltitude, _snowfallWind * -50f);
                }

                if (snowdustSystem != null) {
                    snowdustSystem.transform.position = cameraEffect.transform.position + new Vector3(0, -2 + _snowdustVerticalOffset, 0);
                }
            }

#if UNITY_EDITOR
    if (!Application.isPlaying) return;
#endif

            if (_terrainMarks && _terrainMarksAutoFPS && (transform.position - lastCameraMarkPosition).sqrMagnitude > 0.5f) {
                Vector3 dir = transform.position - lastCameraMarkPosition;
                Vector3 dirxz = new Vector3(dir.z, 0, -dir.x).normalized * UnityEngine.Random.Range(0.11f, 0.35f);
                Vector3 medPos = (lastCameraMarkPosition + transform.position) * 0.5f;
                lastCameraMarkPosition = transform.position;
                MarkSnowAt(medPos + dirxz, 0.185f);
                MarkSnowAt(transform.position - dirxz, 0.185f);
            }

        }

        private void CheckCommandBuffer() {
            if (!needRebuildCommandBuffer) {
                if (excludedRenderers.Length < excludedRenderersCount) {
                    excludedRenderersCount = excludedRenderers.Length;
                }
                bool dirtyVisible = IsUnSafeTime();
                for (int k = 0; k < excludedRenderersCount; k++) {
                    Renderer r = excludedRenderers[k].renderer;
                    if (r == null) {
                        for (int j = k; j < excludedRenderersCount - 1; j++) {
                            excludedRenderers[j] = excludedRenderers[j + 1];
                        }
                        excludedRenderersCount--;
                        k--;
                    } else {
                        bool isVisible = dirtyVisible || r.isVisible;
                        if (isVisible != excludedRenderers[k].wasVisible) {
                            excludedRenderers[k].wasVisible = isVisible;
                            needRebuildCommandBuffer = true;
                        }
                    }
                }
            }
            if (needRebuildCommandBuffer) {
                RebuildCommandBuffer();
            }
        }

        #endregion

        #region Internal stuff

        void CheckSunOcclusion() {
            if (cameraEffect == null || _sun == null)
                return;
            if (_sun.transform.rotation != lastSunRotation) {
                lastSunRotation = _sun.transform.rotation;
                sunOccluded = _sun.transform.forward.y > 0;
                UpdateSnowData3();
            }
        }

        void UpdateSnowTintColor() {
            Shader.SetGlobalColor(ShaderParams.GlobalSnowTint, _snowTint);
        }


        void UpdateSnowData2(float minimumAltitude) {
            if (_coverageExtension > 1 && _coverageResolution == 1)
                _coverageResolution = 2;
            float coverageExtensionValue = 1f / Mathf.Pow(2, 8f + _coverageExtension);
            Shader.SetGlobalVector(ShaderParams.GlobalSnowData2, new Vector4(minimumAltitude + _minimumAltitudeVegetationOffset, 10f * _altitudeScatter, coverageExtensionValue, minimumAltitude));
        }

        void UpdateSnowData3() {
            float y = _sun != null ? _sun.transform.forward.y : -0.3f;
            Shader.SetGlobalVector(ShaderParams.GlobalSnowData3, new Vector4(sunOccluded ? 0f : 1f, Mathf.Clamp01(y * -100f), _groundCoverage + 0.0012f, (2f * (_grassCoverage * Mathf.Min(_snowAmount, 1f)) - 1f)));
        }

        void UpdateSnowData4() {
            Shader.SetGlobalVector(ShaderParams.GlobalSnowData4, new Vector4(1f / _footprintsScale, _footprintsObscurance, _snowNormalsStrength, 1.0f - (_billboardCoverage * Mathf.Min(_snowAmount, 1f))));
        }

        void UpdateSnowData5() {
            _noiseTexScale = Mathf.Max(0.0001f, _noiseTexScale);
            Shader.SetGlobalVector(ShaderParams.GlobalSnowData5, new Vector4(_slopeThreshold - 0.2f, 5f + _slopeSharpness * 10f, _slopeNoise * 5f, _noiseTexScale));
        }

        void UpdateSnowData6() {
            Shader.SetGlobalVector(ShaderParams.GlobalSnowData6, new Vector4(1f - Mathf.Min(_snowAmount, 1f), _smoothness, _altitudeBlending, Mathf.Max(_snowAmount - 1f, 0)));
        }

        /// <summary>
        /// In case Sun is not set, locate any directional light on the scene.
        /// </summary>
        void SetupSun() {
            if (_sun == null) {
                Light[] lights = FindObjectsOfType<Light>();
                Light directionalLight = lights.FirstOrDefault(l => l != null && l.type == LightType.Directional);
                if (directionalLight != null) {
                    _sun = directionalLight.gameObject;
                }
            }
        }

        void CleanUpTextureDepth() {
            if (depthTexture != null) {
                depthTexture.Release();
                depthTexture = null;
            }
            if (depthTextureBlurred != null) {
                depthTextureBlurred.Release();
                depthTextureBlurred = null;
            }
        }

        void ReleaseZenithCam() {
            if (zenithCam != null) {
                DestroyImmediate(zenithCam.gameObject);
                zenithCam = null;
                CleanUpTextureDepth();
            }
        }

        float GetAngle(Vector2 v1, Vector2 v2) {
            float sign = Mathf.Sign(v1.x * v2.y - v1.y * v2.x);
            return Vector2.Angle(v1, v2) * sign;
        }

        #endregion

        #region Property handling


        void OnDidApplyAnimationProperties() {   // support for animating property based fields
            needsUpdateProperties = true;
        }

        public void UpdateMaterialProperties() {
            if (Application.isPlaying) {
                needsUpdateProperties = true;
            } else {
                UpdateMaterialPropertiesNow();
            }
        }

        public void UpdateMaterialPropertiesNow() {

            // Setup materials & shaders
            LoadResources();

            CheckTerrainsCollisionDetectors();

            if (OnUpdateProperties != null)
                OnUpdateProperties();

            needUpdateSnowCoverage = true;

            UpdateSnowfallProperties();
            UpdateSnowdustProperties();

            UpdateSnowTintColor();
            UpdateSnowData2(_minimumAltitude);
            UpdateSnowData3();
            UpdateSnowData4();
            UpdateSnowData5();
            UpdateSnowData6();

            Shader.SetGlobalFloat(ShaderParams.GlobalMinimumGIAmbient, _minimumGIAmbient);
            Shader.SetGlobalFloat(ShaderParams.GlobalSnowExclusionBias, _exclusionBias);
            Shader.SetGlobalInt(ShaderParams.EraseCullMode, _exclusionDoubleSided ? (int)CullMode.Off : (int)CullMode.Back);

            if (_footprintsTexture == null) {
                _footprintsTexture = Resources.Load<Texture2D>("GlobalSnow/Textures/Footprint");
            }
            Shader.SetGlobalTexture(ShaderParams.GlobalDetailTex, _footprintsTexture);

            if (_snowQuality == SnowQuality.FlatShading) {
                Shader.DisableKeyword(ShaderParams.SKW_RELIEF);
                Shader.DisableKeyword(ShaderParams.SKW_OCLUSSION);
                Shader.EnableKeyword(ShaderParams.SKW_FLAT_SHADING);
            } else {
                Shader.DisableKeyword(ShaderParams.SKW_FLAT_SHADING);
                if (_snowQuality == SnowQuality.ReliefMapping) {
                    if (_occlusion) {
                        Shader.DisableKeyword(ShaderParams.SKW_RELIEF);
                        Shader.EnableKeyword(ShaderParams.SKW_OCLUSSION);
                    } else {
                        Shader.DisableKeyword(ShaderParams.SKW_OCLUSSION);
                        Shader.EnableKeyword(ShaderParams.SKW_RELIEF);
                    }
                } else {
                    Shader.DisableKeyword(ShaderParams.SKW_RELIEF);
                    Shader.DisableKeyword(ShaderParams.SKW_OCLUSSION);
                }
            }
            if (_footprints && Application.isPlaying) {
                if (characterController == null) {
                    characterController = FindObjectOfType<CharacterController>();
                }
                Shader.EnableKeyword(ShaderParams.SKW_FOOTPRINTS);
            } else {
                Shader.DisableKeyword(ShaderParams.SKW_FOOTPRINTS);
            }
            if (_terrainMarks && Application.isPlaying) {
                Shader.EnableKeyword(ShaderParams.SKW_TERRAIN_MARKS);
            } else {
                Shader.DisableKeyword(ShaderParams.SKW_TERRAIN_MARKS);
            }
            if (_preserveGI) {
                Shader.EnableKeyword(ShaderParams.SKW_PRESERVE_GI);
            } else {
                Shader.DisableKeyword(ShaderParams.SKW_PRESERVE_GI);
            }
            if (_useZenithalCoverage) {
                Shader.EnableKeyword(ShaderParams.SKW_ZENITHAL_COVERAGE);
            } else {
                Shader.DisableKeyword(ShaderParams.SKW_ZENITHAL_COVERAGE);
            }
            if (_coverageMask && _coverageMaskTexture != null) {
                Shader.EnableKeyword(ShaderParams.SKW_COVERAGE_MASK);
                Shader.SetGlobalTexture(ShaderParams.GlobalDepthMaskTexture, _coverageMaskTexture);
                Shader.SetGlobalVector(ShaderParams.GlobalDepthMaskWorldSize, new Vector4(_coverageMaskWorldSize.x, _coverageMaskWorldCenter.x, _coverageMaskWorldSize.z, _coverageMaskWorldCenter.z));
            } else {
                Shader.DisableKeyword(ShaderParams.SKW_COVERAGE_MASK);
            }
        }

        public void UpdateSnowfallProperties() {
            if (snowfallSystem == null) {
                GameObject go = GameObject.Find(SNOW_PARTICLE_SYSTEM);
                if (go != null) {
                    snowfallSystem = go.GetComponent<ParticleSystem>();
                    if (snowfallSystem == null)
                        DestroyImmediate(go);
                }
            }

            if (_snowfall) {
                if (snowfallSystem == null) {
                    GameObject go = Resources.Load<GameObject>("GlobalSnow/Prefabs/SnowParticleSystem");
                    if (go != null) {
                        go = Instantiate(go);

                        go.name = SNOW_PARTICLE_SYSTEM;
                        go.hideFlags = HideFlags.DontSave;
                        if (go == null) {
                            Debug.LogError("SnowParticleSystem not found.");
                            _snowfall = false;
                        } else {
                            snowfallSystem = go.GetComponent<ParticleSystem>();
                        }
                    }
                }
                if (snowfallSystem != null) {
                    snowfallSystem.gameObject.layer = SNOW_PARTICLES_LAYER;

                    float snowfallIntensity = _snowfallIntensity * _snowAmount;

                    var emission = snowfallSystem.emission;
                    emission.rateOverTime = 1000 * snowfallIntensity * _snowAmount;
                    ParticleSystem.ShapeModule shape = snowfallSystem.shape;
                    shape.scale = new Vector3(_snowfallDistance, _snowfallDistance, 20f);
                    if (_snowfallDistance != lastSnowfallDistance && lastSnowfallDistance > 0) {
                        lastSnowfallIntensity -= 0.001f;
                    }
                    lastSnowfallDistance = _snowfallDistance;
                    ParticleSystem.MainModule main = snowfallSystem.main;
                    main.simulationSpeed = _snowfallSpeed;
                    ParticleSystem.ForceOverLifetimeModule force = snowfallSystem.forceOverLifetime;
                    force.z = new ParticleSystem.MinMaxCurve(-0.2f + _snowfallWind, 0.2f + _snowfallWind);
                    ParticleSystemRenderer r = snowfallSystem.GetComponent<ParticleSystemRenderer>();
                    r.shadowCastingMode = ShadowCastingMode.Off;
                    if (_snowfallUseIllumination) {
                        r.sharedMaterial = snowParticleIllumMat;
                        r.receiveShadows = false;
                    } else {
                        r.sharedMaterial = snowParticleMat;
                        r.receiveShadows = false;
                    }
                    if (snowfallIntensity != lastSnowfallIntensity) {
                        if (m_Particles == null || m_Particles.Length < snowfallSystem.main.maxParticles) {
                            m_Particles = new ParticleSystem.Particle[snowfallSystem.main.maxParticles];
                        }
                        if (snowfallSystem.particleCount < 200) {
                            snowfallSystem.Emit(200 - snowfallSystem.particleCount);
                        }
                        int actualCount = snowfallSystem.GetParticles(m_Particles);
                        float t = snowfallIntensity / lastSnowfallIntensity;
                        if (snowfallIntensity < lastSnowfallIntensity) {
                            for (int k = 0; k < actualCount; k++) {
                                if (UnityEngine.Random.value > t) {
                                    m_Particles[k].remainingLifetime = -1;
                                }
                            }
                        } else if (snowfallIntensity > lastSnowfallIntensity) {
                            int maxCount = (int)(actualCount * t);
                            if (maxCount > m_Particles.Length) maxCount = m_Particles.Length;
                            Vector3 camPos = cameraEffect.transform.position;
                            float distance = _snowfallDistance * 0.5f;
                            snowfallSystemAltitude = Mathf.Min(50f, distance);
                            for (int k = actualCount; k < maxCount; k++) {
                                m_Particles[k] = m_Particles[k - actualCount];
                                Vector3 randomPos = UnityEngine.Random.insideUnitSphere;
                                m_Particles[k].position = new Vector3(camPos.x + randomPos.x * distance, camPos.y + randomPos.y * snowfallSystemAltitude, camPos.z + randomPos.z * distance);
                                m_Particles[k].remainingLifetime = 10f + k % 10;
                            }
                            actualCount = maxCount;
                        }
                        snowfallSystem.SetParticles(m_Particles, actualCount);
                        lastSnowfallIntensity = snowfallIntensity;
                        snowfallSystem.trigger.SetCollider(0, blockCelling);
                        if (Application.isPlaying) {
                            snowfallSystem.Play();
                        }
                    }
                }
            } else if (snowfallSystem != null) {
                DestroyImmediate(snowfallSystem.gameObject);
                snowfallSystem = null;
            }
        }

        public void UpdateSnowdustProperties() {

            if (snowdustSystem == null) {
                GameObject go = GameObject.Find(SNOW_DUST_SYSTEM);
                if (go != null) {
                    snowdustSystem = go.GetComponent<ParticleSystem>();
                    if (snowdustSystem == null)
                        DestroyImmediate(go);
                }
            }

            if (_snowdustIntensity > 0) {
                if (snowdustSystem == null) {
                    GameObject go = Instantiate(Resources.Load<GameObject>("GlobalSnow/Prefabs/SnowDustSystem")) as GameObject;
                    go.name = SNOW_DUST_SYSTEM;
                    go.hideFlags = HideFlags.DontSave;
                    if (go == null) {
                        Debug.LogError("SnowDustSystem not found.");
                        _snowdustIntensity = 0;
                    } else {
                        snowdustSystem = go.GetComponent<ParticleSystem>();
                    }
                }
                if (snowdustSystem != null) {
                    ParticleSystem.MainModule main = snowdustSystem.main;
                    main.simulationSpeed = _snowfallSpeed + _snowdustIntensity;
                    snowdustSystem.gameObject.layer = SNOW_PARTICLES_LAYER;
                    ParticleSystemRenderer r = snowdustSystem.GetComponent<ParticleSystemRenderer>();
                    r.shadowCastingMode = ShadowCastingMode.Off;
                    if (_snowfallUseIllumination) {
                        r.sharedMaterial = snowDustIllumMat;
                    } else {
                        r.sharedMaterial = snowDustMat;
                    }
                    Color dustColor = r.sharedMaterial.color;
                    dustColor.a = _snowdustIntensity;
                    r.sharedMaterial.color = dustColor;
                    if (Application.isPlaying) {
                        snowdustSystem.Play();
                    }
                }
            } else if (snowdustSystem != null) {
                DestroyImmediate(snowdustSystem.gameObject);
                snowdustSystem = null;
            }
        }


        void CheckTerrainsCollisionDetectors() {
            Terrain[] activeTerrains = Terrain.activeTerrains;
            if (activeTerrains != null) {
                for (int k = 0; k < activeTerrains.Length; k++) {
                    Terrain activeTerrain = activeTerrains[k];
                    CheckTerrainCollisionDetector(activeTerrain);
                }
            }
        }

        void CheckTerrainCollisionDetector(Terrain terrain) {
            GlobalSnowCollisionDetector cd = terrain.GetComponent<GlobalSnowCollisionDetector>();
            if (_terrainMarks) {
                if (cd == null)
                    terrain.gameObject.AddComponent<GlobalSnowCollisionDetector>();
            } else {
                DestroyImmediate(cd);
            }
        }

        #endregion

        #region Gizmos

        void OnDrawGizmosSelected() {
            if ((_showCoverageGizmo || _maskEditorEnabled) && zenithCam != null) {
                Vector3 pos = zenithCam.transform.position;
                pos.y = -10;
                float viewSize = zenithCam.orthographicSize * 2f;
                Vector3 size = new Vector3(viewSize, 0.1f, viewSize);
                Gizmos.color = Color.blue;
                for (int k = 0; k < 5; k++) {
                    Gizmos.DrawWireCube(pos, size);
                    pos.y += 0.5f;
                    Gizmos.DrawWireCube(pos, size);
                    pos.y += 0.5f;
                }
            }
            if (_coverageMask && (_showCoverageGizmo || _maskEditorEnabled)) {
                Vector3 pos = _coverageMaskWorldCenter;
                pos.y = -10;
                Vector3 size = new Vector3(_coverageMaskWorldSize.x, 0.1f, _coverageMaskWorldSize.z);
                Gizmos.color = Color.red;
                for (int k = 0; k < 5; k++) {
                    Gizmos.DrawWireCube(pos, size);
                    pos.y += 0.5f;
                    Gizmos.DrawWireCube(pos, size);
                    pos.y += 0.5f;
                }
            }
        }

        #endregion


        #region Misc tools

        /// <summary>
        /// Makes Global Snow ignore a gameobject. Used internally.
        /// </summary>
        public static void IgnoreGameObject(GlobalSnowIgnoreCoverage o) {
            if (o == null) return;
            if (ignoredGOs.Contains(o)) return;
            ignoredGOs.Add(o);
            needUpdateSnowCoverage = true;
            needRebuildCommandBuffer = true;
        }

        /// <summary>
        /// Makes Global Snow use a gameobject for snow coverage. Used internally.
        /// </summary>
        /// <param name="o">O.</param>
        public static void UseGameObject(GlobalSnowIgnoreCoverage o) {
            if (o == null) return;
            if (!ignoredGOs.Contains(o)) return;
            ignoredGOs.Remove(o);
            needUpdateSnowCoverage = true;
            needRebuildCommandBuffer = true;
        }

        /// <summary>
        /// Leaves a mark on the snow at a given world space position.
        /// </summary>
        public void MarkSnowAt(Vector3 position) {
            MarkSnowAt(position, _terrainMarksDefaultSize);
        }

        /// <summary>
        /// Leaves a mark on the snow at a given world space position and radius.
        /// </summary>
        public void MarkSnowAt(Vector3 position, float radius) {
            if (radius <= 0) {
                radius = _terrainMarksDefaultSize;
            }
            decalRequests.Add(new Vector4(position.x, position.y, position.z, radius));
        }

        /// <summary>
        /// Refresh snow coverage
        /// </summary>
        public void UpdateSnowCoverage() {
            needUpdateSnowCoverage = true;
        }


        /// <summary>
        /// Forces command buffer to be rebuilt
        /// </summary>
        /// <param name="performFullSceneScan">If true, the full scene will be parsed to find excluded objects also according to the layer mask setting. This can be time consuming so it usually is only done once at the start of the scene.</param>
        public void RefreshExcludedObjects(bool performFullSceneScan = false) {
            if (performFullSceneScan) {
                this.performFullSceneScan = true;
            }
            needRebuildCommandBuffer = true;
        }


        readonly static Dictionary<GameObject, SnowColliderInfo> collisionCache = new Dictionary<GameObject, SnowColliderInfo>();

        /// <summary>
        /// Reports a continuous collision by an object with ground. This method takes care of spreading terrain marks depending on distance travelled and mark size.
        /// </summary>
        /// <param name="collision">Collision data in case there's rigidbody involved</param>
        /// <param name="contactPoint">Direct collision point in world space if no rigidbody is involved</param>
        public void CollisionStay(GameObject go, Collision collision, Vector3 contactPoint) {
            if (go == null) return;

            // Get snow mark size
            float collisionDistanceThreshold = 0.1f;
            float maxStep = terrainMarksStepMaxDistance;
            float rotationThreshold = terrainMarksRotationThreshold;
            SnowColliderInfo newColliderInfo;
            GameObject collidingGO;
            if (collision != null && collision.collider != null) {
                collidingGO = collision.collider.gameObject;
            } else {
                collidingGO = go;
            }
            GlobalSnowColliderExtraInfo ci = collidingGO.GetComponentInParent<GlobalSnowColliderExtraInfo>();
            bool isFootprint = false;
            if (ci != null) {
                if (ci.ignoreThisCollider) return;
                newColliderInfo.markSize = ci.markSize;
                collisionDistanceThreshold = ci.collisionDistanceThreshold;
                rotationThreshold = ci.rotationThreshold;
                maxStep = ci.stepMaxDistance;
                isFootprint = ci.isFootprint;
            } else {
                newColliderInfo.markSize = terrainMarksDefaultSize;
            }

            // Check gameobject position change
            Vector3 currentPos = collidingGO.transform.position;
            Vector3 currentForward = collidingGO.transform.forward;
            SnowColliderInfo colliderInfo;
            Vector3 moveDir = Vector3.zero;
            float steps = 1f;
            if (!isFootprint && collisionCache.TryGetValue(collidingGO, out colliderInfo)) {
                Vector3 oldPosition = colliderInfo.position;
                moveDir = currentPos - oldPosition;
                float diffSqr = moveDir.sqrMagnitude;
                float angleDiff = Vector3.Angle(colliderInfo.forward, currentForward);
                if (diffSqr < collisionDistanceThreshold && angleDiff < rotationThreshold)
                    return;

                maxStep *= maxStep;
                if (diffSqr < maxStep) {
                    float diff = Mathf.Sqrt(diffSqr);
                    steps = diff / newColliderInfo.markSize;
                    if (steps > 100) steps = 100;
                    steps = ((int)steps) + 1;
                }
            }
            newColliderInfo.position = currentPos;
            newColliderInfo.forward = currentForward;
            collisionCache[collidingGO] = newColliderInfo;
            if (collision != null) {
                for (int k = 0; k < collision.contactCount && k < 5; k++) {
                    ContactPoint cp = collision.GetContact(k);
                    SendSnowMarkAt(cp.point, moveDir, steps, newColliderInfo.markSize);
                }
            } else {
                SendSnowMarkAt(contactPoint, moveDir, steps, newColliderInfo.markSize);
            }
        }

        void SendSnowMarkAt(Vector3 currMarkPos, Vector3 moveDir, float steps, float markSize) {
            if (steps < 2) {
                MarkSnowAt(currMarkPos, markSize);
            } else {
                Vector3 prevMarkpos = currMarkPos - moveDir;
                for (int s = 1; s <= steps; s++) {
                    Vector3 snowMarkPos = Vector3.Lerp(currMarkPos, prevMarkpos, s / steps);
                    MarkSnowAt(snowMarkPos, markSize);
                }
            }
        }

        /// <summary>
        /// Requests drawing a footprint at specified position and direction
        /// </summary>
        public void FootprintAt(Vector3 position, Vector3 moveDir) {
            requestedFootprint = true;
            requestedFootprintPos = position;
            requestedFootprintDir = moveDir;
        }


        public void CollisionStop(GameObject go) {
            SnowColliderInfo colliderInfo;
            if (collisionCache.TryGetValue(go, out colliderInfo)) {
                Vector3 diff = colliderInfo.position - go.transform.position;
                if (diff.sqrMagnitude >= 0.01f) {
                    collisionCache.Remove(go);
                }
            }
        }

        /// <summary>
        /// Returns an approximation of the amount of snow at a world position (0 = no snow, greater than 0 = snow)
        /// </summary>
        public float GetSnowAmountAt(Vector3 position) {

            // get ground position
            Ray ray = new Ray(position + Vector3.up * 1000f, Vector3.down);
            if (!Physics.Raycast(ray, out RaycastHit hitInfo)) return 0;

            // check if this position is covered
            if (hitInfo.point.y > position.y) return 0;

            // we hit the ground
            position = hitInfo.point;

            // check minimum altitude
            if (position.y < minimumAltitude) return 0;

            // check mask
            if (coverageMask && coverageMaskTexture != null) {
                float mx = (position.x - coverageMaskWorldCenter.x) / coverageMaskWorldSize.x + 0.5f;
                float mz = (position.z - coverageMaskWorldCenter.z) / coverageMaskWorldSize.z + 0.5f;
                if (mz >= 0 && mz < 1 && mx >= 0 && mx < 1) {
                    int pixelIndex = (int)(mz * coverageMaskTexture.height) * coverageMaskTexture.width + (int)(mx * coverageMaskTexture.width);
                    if (maskColors == null || maskColors.Length != coverageMaskTexture.width * coverageMaskTexture.height) {
                        maskColors = coverageMaskTexture.GetPixels32();
                    }
                    return maskColors[pixelIndex].r / 255f;
                }
            }

            return 1;
        }


        void CheckMaskColorsArray() {
            int length = coverageMaskTexture.width * coverageMaskTexture.height;
            if (maskColors == null || maskColors.Length != length) {
                maskColors = coverageMaskTexture.GetPixels32();
            }
        }

        void CheckMaskTexture() {
            if (coverageMaskTexture != null) {
                CheckMaskColorsArray();
                return;
            }

            int res = Mathf.Clamp(maskTextureResolution, 256, 8192);
            coverageMask = true;
            coverageMaskTexture = new Texture2D(res, res, TextureFormat.R8, false, true);
            coverageMaskTexture.wrapMode = TextureWrapMode.Clamp;
            coverageMaskTexture.filterMode = FilterMode.Bilinear;

            MaskClear(255);

            UpdateMaterialProperties();
        }

        /// <summary>
        /// Paints or clears snow on mask
        /// </summary>
        /// <param name="pos">Position in world space</param>
        /// <param name="value">Opacity of the snow being painted</param>
        public void MaskPaint(Vector3 pos, byte value, float brushWidth, float brushOpacity = 1f, float brushFuzziness = 0f, MaskPaintMethod paintMethod = MaskPaintMethod.BackgroundThread) {

            CheckMaskTexture();

            if (paintMethod == MaskPaintMethod.GPU) {
                MaskPaintGPU(pos, value, brushWidth, brushOpacity, brushFuzziness);
                return;
            }

            int th = coverageMaskTexture.height;
            int tw = coverageMaskTexture.width;

            if (paintMethod == MaskPaintMethod.BackgroundThread && Application.isPlaying) {
                MaskPaintBackgroundThread(pos, value, tw, th, brushWidth, brushOpacity, brushFuzziness);
                return;
            }

            MaskPaintThread(pos, value, brushWidth, tw, th, brushOpacity, brushFuzziness);
        }

        void MaskPaintBackgroundThread(Vector3 pos, byte value, int tw, int th, float brushWidth, float brushOpacity = 1f, float brushFuzziness = 0f) {
            System.Threading.Tasks.Task.Run(() => MaskPaintThread(pos, value, brushWidth, tw, th, brushOpacity, brushFuzziness));
        }

        void MaskPaintGPU(Vector3 pos, byte value, float brushWidth, float brushOpacity = 1f, float brushFuzziness = 0f) {
            if (coverageMaskRT == null || coverageMaskRT.width != _coverageMaskTexture.width || coverageMaskRT.height != _coverageMaskTexture.height) {
                CleanUpCoverageMaskRT();
                coverageMaskRT = new RenderTexture(_coverageMaskTexture.width, _coverageMaskTexture.height, 0, RenderTextureFormat.R8, RenderTextureReadWrite.Linear);
                coverageMaskRT.filterMode = FilterMode.Bilinear;
                Graphics.Blit(_coverageMaskTexture, coverageMaskRT);
            }
            float x = (pos.x - coverageMaskWorldCenter.x) / coverageMaskWorldSize.x + 0.5f;
            float z = (pos.z - coverageMaskWorldCenter.z) / coverageMaskWorldSize.z + 0.5f;
            int brushSize = Mathf.FloorToInt(coverageMaskRT.width * brushWidth / coverageMaskWorldSize.x);
            float radius = (float)brushSize / coverageMaskRT.width;
            matMaskPaint.SetVector(ShaderParams.MaskPaintData, new Vector4(x, z, radius * radius, brushOpacity));
            matMaskPaint.SetVector(ShaderParams.MaskPaintData2, new Vector4(value / 255f, brushFuzziness, Time.frameCount, 0));
            Graphics.Blit(null, coverageMaskRT, matMaskPaint);
            Shader.SetGlobalTexture(ShaderParams.GlobalDepthMaskTexture, coverageMaskRT); // switch to render texture
        }

        void MaskPaintThread(Vector3 pos, byte value, float brushWidth, int tw, int th, float brushOpacity = 1f, float brushFuzziness = 0f) {

            // Get texture location
            float x = (pos.x - coverageMaskWorldCenter.x) / coverageMaskWorldSize.x + 0.5f;
            float z = (pos.z - coverageMaskWorldCenter.z) / coverageMaskWorldSize.z + 0.5f;
            int tx = Mathf.Clamp((int)(x * tw), 0, tw - 1);
            int ty = Mathf.Clamp((int)(z * th), 0, th - 1);

            // Prepare brush data
            int brushSize = Mathf.FloorToInt(tw * brushWidth / coverageMaskWorldSize.x);
            float opacity = 1f - brushOpacity;
            double fuzziness = 1.1 - brushFuzziness;
            byte colort = (byte)(value * (1f - opacity));
            float radiusSqr = brushSize * brushSize;
            // Paint!
            int j0 = ty - brushSize;
            j0 = Mathf.Clamp(j0, 1, th);
            int j1 = ty + brushSize;
            j1 = Mathf.Clamp(j1, 1, th);
            int k0 = tx - brushSize;
            k0 = Mathf.Clamp(k0, 1, tw);
            int k1 = tx + brushSize;
            k1 = Mathf.Clamp(k1, 1, tw);
            System.Random rnd = new System.Random();
            if (brushFuzziness > 0) {
                for (int j = j0; j < j1; j++) {
                    int jj = j * tw;
                    int dj = (j - ty) * (j - ty);
                    for (int k = k0; k < k1; k++) {
                        int distSqr = dj + (k - tx) * (k - tx);
                        float op = distSqr / radiusSqr;
                        if (op <= 1f) {
                            double threshold = rnd.NextDouble();
                            if (threshold * op < fuzziness) {
                                maskColors[jj + k].r = (byte)(colort + maskColors[jj + k].r * opacity);
                            }
                        }
                    }
                }
            } else if (brushOpacity < 1f) {
                for (int j = j0; j < j1; j++) {
                    int jj = j * tw;
                    int dj = (j - ty) * (j - ty);
                    for (int k = k0; k < k1; k++) {
                        int distSqr = dj + (k - tx) * (k - tx);
                        if (distSqr <= radiusSqr) {
                            maskColors[jj + k].r = (byte)(colort + maskColors[jj + k].r * opacity);
                        }
                    }
                }
            } else {
                for (int j = j0; j < j1; j++) {
                    int jj = j * tw;
                    int dj = (j - ty) * (j - ty);
                    for (int k = k0; k < k1; k++) {
                        int distSqr = dj + (k - tx) * (k - tx);
                        if (distSqr <= radiusSqr) {
                            maskColors[jj + k].r = colort;
                        }
                    }
                }
            }
            needMaskUpdate = true;
        }


        void UpdateSnowGlobalCoverageMaskTexture() {
            if (coverageMaskRT == null) {
                if (_coverageMask && _coverageMaskTexture != null) {
                    Shader.SetGlobalTexture(ShaderParams.GlobalDepthMaskTexture, _coverageMaskTexture);
                } else {
                    Shader.SetGlobalTexture(ShaderParams.GlobalDepthMaskTexture, Texture2D.whiteTexture);
                }
            }
            Shader.SetGlobalVector(ShaderParams.GlobalDepthMaskWorldSize, new Vector4(_coverageMaskWorldSize.x, _coverageMaskWorldCenter.x, _coverageMaskWorldSize.z, _coverageMaskWorldCenter.z));
            Shader.SetGlobalFloat(ShaderParams.GlobalFillOutSideOfMask, !_coverageMask || _coverageMaskFillOutside ? 1f : 0);
        }

        /// <summary>
        /// Fills the mask texture with a constant value
        /// </summary>
        public void MaskClear(byte value = 255) {

            CleanUpCoverageMaskRT();
            CheckMaskTexture();

            Color32 opaque = new Color32(value, value, value, value);
            int length = maskColors.Length;
            for (int k = 0; k < length; k++) {
                maskColors[k] = opaque;
            }

            UpdateSnowGlobalCoverageMaskTexture();
            needMaskUpdate = true;
        }

        /// <summary>
        /// Fills an area with snow equal to the object shape
        /// </summary>
        public void MaskFillArea(GameObject go, byte value, float opacity = 1f, float border = 0f) {
            MeshRenderer meshRenderer = go.GetComponent<MeshRenderer>();
            if (meshRenderer == null) return;
            MaskFillArea(meshRenderer, value, opacity, border);
        }

        readonly List<ushort> indices = new List<ushort>();
        readonly List<Vector3> vertices = new List<Vector3>();
        Mesh lastMesh;

        /// <summary>
        /// Fills an area with snow equal to the object shape
        /// </summary>
        /// <param name="meshRenderer">Mesh renderer of the object</param>
        public void MaskFillArea(MeshRenderer meshRenderer, byte value, float opacity = 1f, float border = 0f, bool useBackgroundThread = true) {

            CheckMaskTexture();

            if (meshRenderer == null) return;

            Bounds bounds = meshRenderer.bounds;

            Vector3 worldSize = coverageMaskWorldSize;
            Vector3 worldCenter = coverageMaskWorldCenter;
            Vector3 worldPosition = bounds.center;

            float tx = (worldPosition.x - worldCenter.x) / worldSize.x + 0.5f;
            if (tx < 0 || tx > 1f)
                return;
            float tz = (worldPosition.z - worldCenter.z) / worldSize.z + 0.5f;
            if (tz < 0 || tz > 1f)
                return;

            // Get triangle info
            MeshFilter mf = meshRenderer.GetComponent<MeshFilter>();
            if (mf == null) {
                Debug.LogError("No MeshFilter found on this object.");
                return;
            }

            Mesh mesh = mf.sharedMesh;
            if (mesh == null) {
                Debug.LogError("No Mesh found on this object.");
                return;
            }
            if (mesh.GetTopology(0) != MeshTopology.Triangles) {
                Debug.LogError("Only triangle topology is supported by this tool.");
                return;
            }
            if (lastMesh != mesh) {
                mesh.GetTriangles(indices, 0);
                mesh.GetVertices(vertices);
                lastMesh = mesh;
            }
            int indicesLength = indices.Count;
            Vector2[] triangles = new Vector2[indicesLength];
            Transform t = meshRenderer.transform;

            Vector2 v0, v1, v2;
            if (border > 0) {
                for (int k = 0; k < indicesLength; k += 3) {
                    Vector3 w0 = t.TransformPoint(vertices[indices[k]]);
                    Vector3 w1 = t.TransformPoint(vertices[indices[k + 1]]);
                    Vector3 w2 = t.TransformPoint(vertices[indices[k + 2]]);
                    v0.x = w0.x; v0.y = w0.z;
                    v1.x = w1.x; v1.y = w1.z;
                    v2.x = w2.x; v2.y = w2.z;
                    Vector2 c = (v0 + v1 + v2) / 3f;
                    v0 += (v0 - c).normalized * border;
                    v1 += (v1 - c).normalized * border;
                    v2 += (v2 - c).normalized * border;
                    triangles[k] = v0;
                    triangles[k + 1] = v1;
                    triangles[k + 2] = v2;
                }

            } else {
                for (int k = 0; k < indicesLength; k += 3) {
                    Vector3 w0 = t.TransformPoint(vertices[indices[k]]);
                    Vector3 w1 = t.TransformPoint(vertices[indices[k + 1]]);
                    Vector3 w2 = t.TransformPoint(vertices[indices[k + 2]]);
                    v0.x = w0.x; v0.y = w0.z;
                    v1.x = w1.x; v1.y = w1.z;
                    v2.x = w2.x; v2.y = w2.z;
                    triangles[k] = v0;
                    triangles[k + 1] = v1;
                    triangles[k + 2] = v2;
                }
            }

            if (useBackgroundThread && Application.isPlaying) {
                System.Threading.Tasks.Task.Run(() => MaskFillAreaThread(value, opacity, triangles, indicesLength, bounds, tx, tz, worldCenter, worldSize));
            } else {
                MaskFillAreaThread(value, opacity, triangles, indicesLength, bounds, tx, tz, worldCenter, worldSize);
            }
        }

        void MaskFillAreaThread(byte value, float opacity, Vector2[] triangles, int indicesLength, Bounds bounds, float tx, float tz, Vector3 worldCenter, Vector3 worldSize) {

            int res = maskTextureResolution;
            int tw = res;
            int th = res;
            int px = (int)(tx * tw);
            int pz = (int)(tz * th);
            float trz = bounds.extents.z / worldSize.z;
            float trx = bounds.extents.x / worldSize.x;
            int deltaz = (int)(th * trz);
            int deltax = (int)(tw * trx);

            int r0 = pz - deltaz;
            if (r0 < 1) r0 = 1; else if (r0 >= th) r0 = th - 1;
            int r1 = pz + deltaz;
            if (r1 < 1) r1 = 1; else if (r1 >= th) r1 = th - 1;
            int c0 = px - deltax;
            if (c0 < 1) c0 = 1; else if (c0 >= tw) c0 = tw - 1;
            int c1 = px + deltax;
            if (c1 < 1) c1 = 1; else if (c1 >= tw) c1 = tw - 1;

            Vector2 wpos;

            int index = 0;
            Vector2 v0 = triangles[index];
            Vector2 v1 = triangles[index + 1];
            Vector2 v2 = triangles[index + 2];

            for (int z = r0; z <= r1; z++) {
                int zz = z * res;
                wpos.y = (((z + 0.5f) / res) - 0.5f) * worldSize.z + worldCenter.z;
                for (int x = c0; x <= c1; x++) {
                    wpos.x = (((x + 0.5f) / res) - 0.5f) * worldSize.x + worldCenter.x;

                    // Check if any triangle contains this position
                    if (opacity >= 1f) {
                        for (int i = 0; i < indicesLength; i += 3) {
                            if (PointInTriangle(wpos, v0, v1, v2)) {
                                maskColors[zz + x].r = value;
                                break;
                            } else {
                                index += 3;
                                index %= indicesLength;
                                v0 = triangles[index];
                                v1 = triangles[index + 1];
                                v2 = triangles[index + 2];
                            }
                        }
                    } else {
                        for (int i = 0; i < indicesLength; i += 3) {
                            if (PointInTriangle(wpos, v0, v1, v2)) {
                                byte v = (byte)(value * opacity + maskColors[zz + x].r * (1f - opacity));
                                maskColors[zz + x].r = v;
                                break;
                            } else {
                                index += 3;
                                index %= indicesLength;
                                v0 = triangles[index];
                                v1 = triangles[index + 1];
                                v2 = triangles[index + 2];
                            }
                        }
                    }
                }
            }

            needMaskUpdate = true;
        }


        float Sign(Vector2 p1, Vector2 p2, Vector2 p3) {
            return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
        }

        bool PointInTriangle(Vector2 pt, Vector2 v1, Vector2 v2, Vector2 v3) {
            float d1, d2, d3;
            bool has_neg, has_pos;

            d1 = Sign(pt, v1, v2);
            d2 = Sign(pt, v2, v3);
            d3 = Sign(pt, v3, v1);

            has_neg = (d1 < 0) || (d2 < 0) || (d3 < 0);
            has_pos = (d1 > 0) || (d2 > 0) || (d3 > 0);

            return !(has_neg && has_pos);
        }


        /// <summary>
        /// Fills an area with snow equal to the object shape
        /// </summary>
        /// <param name="value">Snow value (0=no snow, 255=full snow)</param>
        /// <param name="opacity">Blending strength (0-1)</param>
        /// <param name="fallOff">Creates a smooth border (0-1)</param>
        public void MaskFillArea(Bounds bounds, byte value, float opacity = 1f, float fallOff = 0) {

            CheckMaskTexture();

            Vector3 worldSize = coverageMaskWorldSize;
            Vector3 worldCenter = coverageMaskWorldCenter;
            Vector3 worldPosition = bounds.center;

            float tx = (worldPosition.x - worldCenter.x) / worldSize.x + 0.5f;
            if (tx < 0 || tx > 1f)
                return;
            float tz = (worldPosition.z - worldCenter.z) / worldSize.z + 0.5f;
            if (tz < 0 || tz > 1f)
                return;

            int res = maskTextureResolution;
            int tw = res;
            int th = res;
            int px = (int)(tx * tw);
            int pz = (int)(tz * th);
            float trz = bounds.extents.z / worldSize.z;
            float trx = bounds.extents.x / worldSize.x;
            int deltaz = (int)(th * trz);
            int deltax = (int)(tw * trx);

            int r0 = pz - deltaz;
            if (r0 < 1) r0 = 1; else if (r0 >= th) r0 = th - 1;
            int r1 = pz + deltaz;
            if (r1 < 1) r1 = 1; else if (r1 >= th) r1 = th - 1;
            int c0 = px - deltax;
            if (c0 < 1) c0 = 1; else if (c0 >= tw) c0 = tw - 1;
            int c1 = px + deltax;
            if (c1 < 1) c1 = 1; else if (c1 >= tw) c1 = tw - 1;

            float midz = (r1 + r0) / 2f;
            float midx = (c1 + c0) / 2f;
            float hz = midz - r0;
            float hx = midx - c0;

            if (opacity >= 1f) {
                for (int z = r0; z <= r1; z++) {
                    int zz = z * res;
                    float gradZ = 1f - Mathf.Abs(z - midz) / hz;
                    gradZ = gradZ / (fallOff + 0.001f);
                    if (gradZ > 1f) gradZ = 1f;
                    for (int x = c0; x <= c1; x++) {
                        float gradX = 1f - Mathf.Abs(x - midx) / hx;
                        gradX = gradX / (fallOff + 0.001f);
                        if (gradX > 1f) gradX = 1f;
                        if (gradZ < gradX) gradX = gradZ;
                        maskColors[zz + x].r = (byte)(value * gradX);
                    }
                }
            } else {
                for (int z = r0; z <= r1; z++) {
                    int zz = z * res;
                    float smz = 1f - (float)Mathf.Abs(z - midz) / hz;
                    float gradZ = Mathf.Lerp(1, 0, smz * fallOff);
                    for (int x = c0; x <= c1; x++) {
                        float smx = 1f - (float)Mathf.Abs(x - midx) / hx;
                        float gradX = Mathf.Lerp(1, 0, smx * fallOff);
                        if (gradZ < gradX) gradX = gradZ;
                        byte v = (byte)(value * gradX * opacity + maskColors[zz + x].r * (1f - opacity));
                        maskColors[zz + x].r = v;
                    }
                }
            }

            needMaskUpdate = true;
        }


        /// <summary>
        /// Sends any pending coverage mask texture change to GPU
        /// </summary>
        public void SubmitCoverageMaskTextureChanges() {
            needMaskUpdate = false;
            if (coverageMaskTexture != null && maskColors != null) {
                int length = coverageMaskTexture.width * coverageMaskTexture.height;
                if (length == maskColors.Length) {
                    coverageMaskTexture.SetPixels32(maskColors);
                    coverageMaskTexture.Apply(false);
                }
            }
        }

        #endregion

    }

}