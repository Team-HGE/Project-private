using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    public void PlayAudioClip(AudioClip clip, bool isFade, float volume)
    {
        AudioSource playingSource = AudioManager.Instance.audioSources.Find(source => source.isPlaying && source.clip == clip);

        if (playingSource != null)
        {
            playingSource.volume = volume;
            playingSource.Play();
            if (isFade)
            {
                StartCoroutine(FadeInAudio(playingSource, volume));
            }
            return;
        }

        AudioSource availableSource = AudioManager.Instance.audioSources.Find(source => !source.isPlaying);

        if (availableSource != null)
        {
            availableSource.clip = clip;
            availableSource.volume = volume;
            availableSource.Play();
            if (isFade)
            {
                StartCoroutine(FadeInAudio(availableSource, volume));
            } 
        }
        else
        {
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            newSource.clip = clip;
            newSource.volume = volume;
            newSource.Play();
            AudioManager.Instance.audioSources.Add(newSource);
            if (isFade)
            {
                StartCoroutine(FadeInAudio(newSource, volume));
            }
        }
    }

    public void PlayStopClip(AudioClip clip, bool isFade)
    {
        AudioSource playingSource = AudioManager.Instance.audioSources.Find(source => source.isPlaying && source.clip == clip);

        if (playingSource != null)
        {
            if (isFade)
            {
                StartCoroutine (FadeOutAudio(playingSource));
            }
            else
            {
                playingSource.Stop();
                playingSource.clip = null;
            }
        }
    }

    private IEnumerator FadeInAudio(AudioSource audioSource, float volume)
    {
        audioSource.volume = 0f;

        float currentTime = 0f;

        while (currentTime < 2)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, volume, currentTime / 2);
            yield return null;
        }

        audioSource.volume = volume;
    }

    private IEnumerator FadeOutAudio(AudioSource audioSource)
    {
        float startVolume = audioSource.volume;
        float currentTime = 0f;

        while (currentTime < 2)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, currentTime / 2);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();
        audioSource.clip = null;
    }
}
