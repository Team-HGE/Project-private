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

        Debug.Log("���� ����");

        audioSource.clip = clip;
        audioSource.volume = soundEffectVolume;

        // ��ġ ����
        audioSource.pitch = 1f + Random.Range(-soundEffectPitchVariance, soundEffectPitchVariance);

        // ����� ���
        audioSource.Play();

        // Disable �޼��带 Ŭ�� ���� + �߰� ���� �ð� �Ŀ� ȣ��
        // disableDelay ���ڸ� �߰��Ͽ� ������Ʈ�� ��Ȱ��ȭ�ϱ� ���� ����� �ð��� �����ϰ� ����
        Invoke("Disable", clip.length + disableDelay);
    }

    public void Disable()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
        Debug.Log("���� ����");


        gameObject.SetActive(false);
    }
}
