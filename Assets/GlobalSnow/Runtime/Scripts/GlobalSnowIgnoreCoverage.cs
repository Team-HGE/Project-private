using System;
using System.Collections;
using UnityEngine;

namespace GlobalSnowEffect {

    [ExecuteInEditMode]
    public class GlobalSnowIgnoreCoverage : MonoBehaviour {

        [Tooltip("If this gameobject or any of its children can receive snow.")]
        public bool receiveSnow;

        [Tooltip("If this gameobject or any of its children block snow down.")]
        public bool blockSnow;

        [Tooltip("If enabled, Global Snow will use a fast mask shader to exclude snow from this object. If disabled, Global Snow will use the object material and shader, which can be a bit slower but more accurate. If shader uses displacement or vertex animation, disable this option.")]
        public bool useFastMaskShader = true;

        [Tooltip("Exclusion alpha cut-off")]
        [Range(0, 1)]
        public float exclusionCutOff;

        [NonSerialized]
        public int layer;

        [NonSerialized]
        public Renderer[] renderers;

        [NonSerialized, HideInInspector]
        public int[] renderersLayers;

        void OnEnable() {
            renderers = GetComponentsInChildren<Renderer>(true);
            renderersLayers = new int[renderers.Length];
            GlobalSnow.IgnoreGameObject(this);
        }

        void OnDisable() {
            GlobalSnow.UseGameObject(this);
        }



    }
}