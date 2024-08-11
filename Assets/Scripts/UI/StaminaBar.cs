using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider staminaSlider;
    public RunEffect runEffect; // RunEffect 스크립트 참조
    public Image fillImage; // Fill 이미지 참조

    private float timeAtMaxStamina;
    private bool isAtMaxStamina;
    private RectTransform rectTransform;

    void Start()
    {
        runEffect = GetComponent<RunEffect>();
        if (runEffect != null)
        {
            staminaSlider.maxValue = runEffect.MaxStamina;
            staminaSlider.value = runEffect.CurrentStamina;
        }
        timeAtMaxStamina = -1f; // 초기화
        isAtMaxStamina = false;
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (runEffect != null)
        {
            float currentStamina = runEffect.CurrentStamina;
            staminaSlider.value = currentStamina;
            float fillPercentage = currentStamina / runEffect.MaxStamina;

           
            if (fillPercentage > 0.5f)
            {
                // 60% 이상일 때 초록색에서 노란색으로 점진적으로 변경
                fillImage.color = Color.Lerp(Color.yellow, Color.green, (fillPercentage - 0.5f) / 0.3f);
            }
            else if (fillPercentage > 0.2f)
            {
                // 30%에서 60% 사이일 때 노란색에서 빨간색으로 점진적으로 변경
                fillImage.color = Color.Lerp(Color.red, Color.yellow, (fillPercentage - 0.2f) / 0.2f);
            }

            // 스테미나가 최대치에 도달했을 때 타이머 시작
            if (currentStamina == runEffect.MaxStamina)
            {
                if (!isAtMaxStamina)
                {
                    isAtMaxStamina = true;
                    timeAtMaxStamina = Time.time;
                }

                // 2초 동안 최대치가 유지되면 스테미나 바 숨기기
                if (isAtMaxStamina && Time.time - timeAtMaxStamina >= 2f)
                {
                    rectTransform.localScale = Vector3.zero; // 스테미나 바 숨기기
                }
            }
            else
            {
                // 스테미나가 최대치가 아니면 타이머 초기화
                isAtMaxStamina = false;
                if (rectTransform.localScale == Vector3.zero)
                {
                    rectTransform.localScale = Vector3.one; // 스테미나 바 다시 보이게 설정
                }
            }
        }
    }
}