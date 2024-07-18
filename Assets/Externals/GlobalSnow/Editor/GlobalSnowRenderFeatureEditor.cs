using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering.Universal;

namespace GlobalSnowEffect {
    [CustomEditor(typeof(GlobalSnowRenderFeature))]
    public class GlobalSnowRenderFeatureEditor : Editor {

        public override void OnInspectorGUI() {

            if (GlobalSnowRenderFeature.renderingMode != RenderingMode.Deferred) {
                EditorGUILayout.HelpBox("Global Snow requires deferred rendering path.", MessageType.Error);
            }
            DrawDefaultInspector();
        }
    }

}

