using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource soundSource;

    // �ܹ߼� ȿ���� ��� �޼���
    public void PlaySound(AudioClip clip)
    {
        soundSource.clip = clip;
        soundSource.Play();
    }
}