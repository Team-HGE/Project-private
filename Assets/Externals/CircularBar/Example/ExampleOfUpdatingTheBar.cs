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
            player = FindObjectOfType<Player>();
        }

        void Update()
        {
            if (player == null) Debug.Log("뭐야 아무것도없는데?");

            float currentNoise = player.CurNoiseAmount / 30 ;
             
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