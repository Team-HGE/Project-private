using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonManager<AudioManager>
{
    [Title("AudioSources")]
    public List<AudioSource> audioSources;
    public int maxAudioSources;

    [Title("BackGroundClips")]
    public BackGroundSoundClipMapping[] backGroundAudioClips;

    [Title("SEClips")]
    public SoundEffectClipMapping[] soundEffectClips;

    [System.Serializable]
    public struct SoundEffectClipMapping
    {
        public SoundEffect soundEffect;
        public AudioClip audioClip;
    }

    [System.Serializable]
    public struct BackGroundSoundClipMapping
    {
        public BackGroundSound backGroundSound;
        public AudioClip audioClip;
    }

    public PlayAudio playAudio;
    public PlayAudioSE playAudioSE;

    [ShowInInspector]
    private Dictionary<BackGroundSound, AudioClip> _audioBackGroundClipDictionary = new Dictionary<BackGroundSound, AudioClip>();
    public Dictionary<BackGroundSound, AudioClip> AudioBackGroundClipDictionary
    {
        get { return _audioBackGroundClipDictionary; }
        set { _audioBackGroundClipDictionary = value; }
    }
    [ShowInInspector]
    private Dictionary<SoundEffect, AudioClip> _audioSoundEffectClipDictionary = new Dictionary<SoundEffect, AudioClip>();
    public Dictionary<SoundEffect, AudioClip> AudioSoundEffectClipDictionary
    {
        get { return _audioSoundEffectClipDictionary; }
        set { _audioSoundEffectClipDictionary = value; }
    }

    protected override void Awake()
    {
        base.Awake();

        playAudio = GetComponent<PlayAudio>();
        playAudioSE = GetComponent<PlayAudioSE>();
        DontDestroyOnLoad(gameObject);
        foreach (var mapping in backGroundAudioClips)
        {
            _audioBackGroundClipDictionary[mapping.backGroundSound] = mapping.audioClip;
        }

        foreach (var mapping in soundEffectClips)
        {
            _audioSoundEffectClipDictionary[mapping.soundEffect] = mapping.audioClip;
        }

        for (int i = 0; i < maxAudioSources; i++)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSources.Add(audioSource);
        }
    }
    public void PlayBackGroundSound(BackGroundSound backGroundSound, bool isFade, float volume)
    {
        if (_audioBackGroundClipDictionary.TryGetValue(backGroundSound, out AudioClip clip))
        {
            playAudio.PlayAudioClip(clip, isFade, volume);
        }
    }
    public void StopBackGroundSound(BackGroundSound backGroundSound, bool isFade)
    {
        if (_audioBackGroundClipDictionary.TryGetValue(backGroundSound, out AudioClip clip))
        {
            playAudio.PlayStopClip(clip, isFade);
        }
    }

    public void PlaySoundEffect(SoundEffect soundEffect)
    {
        if (_audioSoundEffectClipDictionary.TryGetValue(soundEffect, out AudioClip clip))
        {
            playAudioSE.PlayAudioCIip(clip);
        }
    }

    public void PlayDialSE(AudioClip clip) //Ãß°¡
    {
        if (clip == null) return;
        playAudioSE.PlayDialSE(clip);
    }

    public void StopSoundEffect(SoundEffect soundEffect)
    {
        if (_audioSoundEffectClipDictionary.TryGetValue(soundEffect, out AudioClip clip))
        {
            playAudioSE.StopAudioCIip(clip);
        }
    }

    public void StopDialSE(AudioClip clip)
    {
        if (clip != null)
        {
            playAudioSE.StopDialSE(clip);
        }
    }

    public void StopAllClips()
    {
        foreach (var audioSource in audioSources)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
        public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }
}