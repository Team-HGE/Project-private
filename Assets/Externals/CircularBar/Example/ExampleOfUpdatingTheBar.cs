using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace DiceNook.View
{
    public class ExampleOfUpdatingTheBar : MonoBehaviour
    {
        public UIDocument UIDocument;
        [SerializeField] private List<CircularBar> circularBars;
        [SerializeField] private List<Label> labels;

        public Player player;

        private void OnEnable()
        {
            circularBars = UIDocument.rootVisualElement.Query<CircularBar>().ToList();
            labels = UIDocument.rootVisualElement.Query<Label>().ToList();
            UpdateUI();
        }
        void Update()
        {
            if (player == null) return;
            UpdateUI();
        }

        private void UpdateUI()
        {
            float currentNoise = player.CurNoiseAmount / player.MaxNoiseAmount;

            UpdateBars(currentNoise);
            UpdateLabels(currentNoise);
        }

        private void UpdateBars(float noisePercentage)
        {
            foreach (var bar in circularBars)
            {
                bar.UpdateBar(noisePercentage);
            }
        }

        private void UpdateLabels(float noisePercentage)
        {
            int percentage = Mathf.RoundToInt(noisePercentage * 100);
            foreach (var label in labels)
            {
                label.text = $"{percentage}% noise";
            }
        }
    }
}