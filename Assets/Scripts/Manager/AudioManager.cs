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

    // �ܹ߼� ȿ���� ��� �޼���
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