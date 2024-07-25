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
    protected override void Awake()
    {
        base.Awake();

        playAudio = GetComponent<PlayAudio>();

        foreach (var mapping in backGroundAudioClips)
        {
            _audioBackGroundClipDictionary[mapping.backGroundSound] = mapping.audioClip;
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
            playAudio.PlayAudioClip(clip);
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
}