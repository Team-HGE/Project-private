using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine;

public class StaminaEffects : MonoBehaviour
{
    private RunEffect runEffect;
    public Volume volume;
    private Bloom bloom;
    private MotionBlur motionBlur;

    private void Start()
    {
        runEffect = GetComponent<RunEffect>(); // RunEffect 클래스의 인스턴스를 가져옴

        // Volume 프로파일에서 Bloom과 MotionBlur 컴포넌트 가져오기
        if (volume.profile.TryGet<Bloom>(out Bloom bloomComponent))
        {
            bloom = bloomComponent;
        }
        if (volume.profile.TryGet<MotionBlur>(out MotionBlur motionBlurComponent))
        {
            motionBlur = motionBlurComponent;
        }
    }

    void Update()
    {
        if (runEffect != null)
        {
            float currentStamina = runEffect.CurrentStamina;

            if (currentStamina < 50)
            {
                // 스태미나에 비례하여 Bloom과 MotionBlur 값을 조절
                float staminaRatio = currentStamina / 10f;
                bloom.intensity.value = Mathf.Lerp(10f, 0f, staminaRatio); // Bloom 효과
                motionBlur.intensity.value = Mathf.Lerp(1f, 0f, staminaRatio); // Motion Blur 효과
            }
            else
            {
                // 스태미나가 50 이상일 때 효과를 기본값으로 설정
                bloom.intensity.value = 0f;
                motionBlur.intensity.value = 0f;
            }
        }
        else
        {
            Debug.LogError("RunEffect 컴포넌트를 찾을 수 없습니다!");
        }
    }
}