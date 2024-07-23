using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class StaminaEffects : MonoBehaviour
{
    private RunEffect runEffect;
    public Image image; // 투명도를 조절할 이미지
    public float value; // 100에서 0 사이의 float 값

    private void Start()
    {
    }

    void Update()
    {
        if (runEffect != null)
        {
            // ValueController의 value 값을 가져옵니다.
            float value = runEffect.CurrentStamina;

            // Alpha 값을 계산합니다. 100일 때 1(불투명), 0일 때 0(완전 투명)
            float alpha = Mathf.Clamp(value / 100.0f, 0.0f, 1.0f);

            // 이미지의 현재 색상을 가져옵니다
            Color color = image.color;

            // Alpha 값을 변경합니다
            color.a = alpha;

            // 변경된 색상을 다시 이미지에 적용합니다
            image.color = color;
        }
    }
}