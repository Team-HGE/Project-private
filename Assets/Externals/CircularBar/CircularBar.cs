using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace DiceNook.View
{
    public class CircularBar : VisualElement
    {
        private readonly Material mat;
        private readonly RenderTexture rt;
        private readonly Texture2D tex;
        public float fill { get; set; }


        public CircularBar()
        {
            var visualTree = Resources.Load<VisualTreeAsset>("UI/CircularBar");
            if (visualTree == null)
            {
                Debug.LogError("Failed to load UI/CircularBar VisualTreeAsset.");
                return;
            }
            visualTree.CloneTree(this);

            var style = Resources.Load<StyleSheet>("UI/CircularBar");
            if (style == null)
            {
                Debug.LogError("Failed to load UI/CircularBar StyleSheet.");
                return;
            }
            styleSheets.Add(style);

            var shader = Shader.Find("Shader Graphs/CircularBar");
            if (shader == null)
            {
                Debug.LogError("Failed to find Shader Graphs/CircularBar.");
                return;
            }
            mat = new Material(shader);

            rt = new RenderTexture(128, 128, 0, RenderTextureFormat.ARGBFloat);
            tex = new Texture2D(128, 128);
            RenderTexture.active = rt;

            var circularBar = this.Q<VisualElement>("CircularBar__bar");
            circularBar.style.backgroundImage = new StyleBackground(tex);
            this.style.width = 128;
            this.style.height = 128;
        }

        /// <summary>
        /// Updates the progress of the CircularBar by the fill parameter
        /// </summary>
        /// <param name="fill">From 0 to 1</param>
        public void UpdateBar(float fill)
        {
            this.fill = fill;
            mat.SetFloat("_fill", fill);
            mat.SetColor("_ProgressColor", CalculateColor(fill)); // 쉐이더의 색상 설정
            var circularBar = this.Q<VisualElement>("CircularBar__bar");
            circularBar.style.unityBackgroundImageTintColor = CalculateColor(fill);
            var tempRT = RenderTexture.GetTemporary(rt.width, rt.height, rt.depth, rt.format);
            Graphics.Blit(rt, tempRT, mat);
            RenderTexture.ReleaseTemporary(tempRT);
            TextureApply();
        }

        private Color CalculateColor(float fill)
        {
            if (fill >= 0.6f)
            {
                // 80% 이상일 때 빨간
                return Color.red;
            }
            else if (fill >= 0.3f)
            {
                // 40%에서 80% 사이일 때 노란색으로 보간
                return Color.Lerp(Color.yellow, Color.red, (fill - 0.6f) / 0.3f);
            }
            else
            {
                // 0%에서 40% 사이일 때 빨간색으로 보간
                return Color.Lerp(Color.green, Color.yellow, fill / 0.3f);
            }
        }

        private void TextureApply()
        {
            tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            tex.Apply();
        }

        public new class UxmlFactory : UxmlFactory<CircularBar, UxmlTraits>
        {
        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private readonly UxmlFloatAttributeDescription m_Fill = new() { name = "fill", defaultValue = 0.1f };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var circularBar = ve as CircularBar;
                circularBar.UpdateBar(m_Fill.GetValueFromBag(bag, cc));
            }
        }
    }
}


