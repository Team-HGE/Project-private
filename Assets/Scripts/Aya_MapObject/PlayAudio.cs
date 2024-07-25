using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    public void PlayAudioClip(AudioClip clip)
    {
        AudioSource playingSource = AudioManager.Instance.audioSources.Find(source => source.isPlaying && source.clip == clip);

        if (playingSource != null)
        {
            playingSource.Play();
            return;
        }

        AudioSource availableSource = AudioManager.Instance.audioSources.Find(source => !source.isPlaying);

        if (availableSource != null)
        {
            availableSource.clip = clip;
            availableSource.Play();
        }
        else
        {
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            newSource.clip = clip;
            newSource.Play();
            AudioManager.Instance.audioSources.Add(newSource);
        }
    }

    public void PlayStopClip(AudioClip clip)
    {
        AudioSource playingSource = AudioManager.Instance.audioSources.Find(source => source.isPlaying && source.clip == clip);

        if (playingSource != null)
        {
            playingSource.Stop();
        }
    }
}
