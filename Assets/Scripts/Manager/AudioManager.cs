using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource soundSource;

    // 단발성 효과음 재생 메서드
    public void PlaySound(AudioClip clip)
    {
        soundSource.clip = clip;
        soundSource.Play();
    }
}