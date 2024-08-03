using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonManager<AudioManager>
{
    [Header("AudioSources")]
    public List<AudioSource> audioSources;
    public int maxAudioSources;

    [Header("Clips")]
    public BackGroundSoundClipMapping[] backGroundAudioClips;

    [Header("SE Clips")]
    public List<AudioClip> seAudioClips;
    private Dictionary<string, AudioClip> _seAudioClipDictionary = new Dictionary<string, AudioClip>();

    [System.Serializable]
    public struct BackGroundSoundClipMapping
    {
        public BackGroundSound backGroundSound;
        public AudioClip audioClip;
    }
    private Dictionary<BackGroundSound, AudioClip> _audioBackGroundClipDictionary = new Dictionary<BackGroundSound, AudioClip>();
    public Dictionary<BackGroundSound, AudioClip> AudioBackGroundClipDictionary
    {
        get { return _audioBackGroundClipDictionary; }
        set { _audioBackGroundClipDictionary = value; }
    }
    public PlayAudio playAudio;
    public PlaySEAudio playSEAudio;
    protected override void Awake()
    {
        base.Awake();

        playAudio = GetComponent<PlayAudio>();

        foreach (var mapping in backGroundAudioClips)
        {
            _audioBackGroundClipDictionary[mapping.backGroundSound] = mapping.audioClip;
        }

        foreach (var clip in seAudioClips)
        {
            _seAudioClipDictionary[clip.name] = clip;
        }

        for (int i = 0; i < maxAudioSources; i++)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSources.Add(audioSource);
        }
    }
    public void PlaySound(BackGroundSound backGroundSound)
    {
        if (_audioBackGroundClipDictionary.TryGetValue(backGroundSound, out AudioClip clip))
        {
            playSEAudio.PlaySEAudioClip(clip);
        }
    }
    public void StopSound(BackGroundSound backGroundSound)
    {
        if (_audioBackGroundClipDictionary.TryGetValue(backGroundSound, out AudioClip clip))
        {
            playAudio.PlayStopClip(clip);
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

    public void PlaySE(string seName)
    {
        if (_seAudioClipDictionary.TryGetValue(seName, out AudioClip clip))
        {
            playAudio.PlayAudioClip(clip);
        }
    }

}