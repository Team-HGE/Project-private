using UnityEngine;

public enum BackGroundSound
{
    MainMenuSound,
    ASceneSound,
    BSceneSound
}
public class AudioManager : SingletonManager<AudioManager>
{
    public AudioSource soundSource;
    public AudioClip[] backGroundAudioClips;

    // 단발성 효과음 재생 메서드
    protected override void Awake()
    {
        base.Awake();
        soundSource = GetComponent<AudioSource>();
        PlaySound(backGroundAudioClips[0]);
    }
    public void PlaySound(AudioClip clip)
    {
        soundSource.clip = clip;
        soundSource.Play();
    }
    public void StopSound()
    {
        soundSource.Stop();
    }
}