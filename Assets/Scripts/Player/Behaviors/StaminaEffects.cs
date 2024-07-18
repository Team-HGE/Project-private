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
        runEffect = GetComponent<RunEffect>(); // RunEffect Ŭ������ �ν��Ͻ��� ������

        // Volume �������Ͽ��� Bloom�� MotionBlur ������Ʈ ��������
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
                // ���¹̳��� ����Ͽ� Bloom�� MotionBlur ���� ����
                float staminaRatio = currentStamina / 10f;
                bloom.intensity.value = Mathf.Lerp(10f, 0f, staminaRatio); // Bloom ȿ��
                motionBlur.intensity.value = Mathf.Lerp(1f, 0f, staminaRatio); // Motion Blur ȿ��
            }
            else
            {
                // ���¹̳��� 50 �̻��� �� ȿ���� �⺻������ ����
                bloom.intensity.value = 0f;
                motionBlur.intensity.value = 0f;
            }
        }
        else
        {
            Debug.LogError("RunEffect ������Ʈ�� ã�� �� �����ϴ�!");
        }
    }
}