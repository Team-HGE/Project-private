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

    public void Play(AudioClip clip, float soundEffectVolume, float soundEffectPitchVariance, float addVolume, float transitionTime, float pitch)
    {
        if (audioSource == null) return;

        CancelInvoke();
        audioSource.clip = clip;
        audioSource.volume = soundEffectVolume + addVolume;
        audioSource.pitch = soundEffectPitchVariance + Random.Range(-0.2f, 0.2f) + pitch;
        audioSource.Play();

        // Disable 메서드를 클립 길이 + 추가 지연 시간 후에 호출
        // disableDelay 인자를 추가하여 오브젝트를 비활성화하기 전에 대기할 시간을 유연하게 설정
        Invoke("Disable", clip.length + 0.1f + transitionTime);
    }

    public void Disable()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }

        gameObject.SetActive(false);
    }
}
