using UnityEngine;

public class SoundSource : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing from the GameObject.");
        }
    }

    public void Play(AudioClip clip, float soundEffectVolume, float soundEffectPitchVariance, float disableDelay = 0.2f)
    {
        if (audioSource == null) return;

        CancelInvoke();

        Debug.Log("소음 실행");

        audioSource.clip = clip;
        audioSource.volume = soundEffectVolume;

        // 핏치 변경
        audioSource.pitch = 1f + Random.Range(-soundEffectPitchVariance, soundEffectPitchVariance);

        // 오디오 재생
        audioSource.Play();

        // Disable 메서드를 클립 길이 + 추가 지연 시간 후에 호출
        // disableDelay 인자를 추가하여 오브젝트를 비활성화하기 전에 대기할 시간을 유연하게 설정
        Invoke("Disable", clip.length + disableDelay);
    }

    public void Disable()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
        Debug.Log("소음 종료");


        gameObject.SetActive(false);
    }
}
