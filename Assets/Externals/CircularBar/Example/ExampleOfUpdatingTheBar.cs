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

        private PlayerStamina playerStamina;

        private void Start()
        {
            circularBars = UIDocument.rootVisualElement.Query<CircularBar>().ToList();
            labels = UIDocument.rootVisualElement.Query<Label>().ToList();
            playerStamina = FindObjectOfType<PlayerStamina>(); // PlayerStamina 컴포넌트를 찾습니다.
        }

        void Update()
        {
            if (playerStamina == null) return;

            float currentStamina = playerStamina.CurrentStamina / playerStamina.MaxStamina;

            UpdateBars(currentStamina);
            UpdateLabels(currentStamina);
        }

        private void UpdateBars(float staminaPercentage)
        {
            foreach (var bar in circularBars)
            {
                bar.UpdateBar(staminaPercentage);
            }
        }

        private void UpdateLabels(float staminaPercentage)
        {
            int percentage = Mathf.RoundToInt(staminaPercentage * 100);
            foreach (var label in labels)
            {
                label.text = $"{percentage}% stamina";
            }
        }
    }
}