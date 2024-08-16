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

        public Player player;

        private void Start()
        {
            circularBars = UIDocument.rootVisualElement.Query<CircularBar>().ToList();
            labels = UIDocument.rootVisualElement.Query<Label>().ToList();
        }

        private void OnEnable()
        {
            UpdateUI();
        }
        void Update()
        {
            if (player == null) return;
            Debug.Log("On");
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