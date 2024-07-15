using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace DiceNook.View
{
    public class ExampleOfUpdatingTheBar : MonoBehaviour
    {
        public UIDocument UIDocument;
        private List<CircularBar> circularBars;
        private List<Label> labels;

        private void Start()
        {
            circularBars = UIDocument.rootVisualElement.Query<CircularBar>().ToList();
            labels = UIDocument.rootVisualElement.Query<Label>().ToList();
        }

        void Update()
        {
            var time = Time.time / 10;

            UpdateBars(time);
            UpdateLabels(time);
        }

        private void UpdateBars(float time)
        {
            foreach (var bar in circularBars)
            {
                bar.UpdateBar(time);
            }
        }

        private void UpdateLabels(float time)
        {
            int percentage = Mathf.RoundToInt(time * 100);
            foreach (var label in labels)
            {
                label.text = $"{percentage}% filled";
            }
        }
    }
}