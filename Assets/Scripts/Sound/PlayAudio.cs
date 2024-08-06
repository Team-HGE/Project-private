using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlaySEAudio : MonoBehaviour
{
    public float volume = 0.6f;
    public void PlayAudioCIip(AudioClip clip)
    {  
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
        Destroy(audioSource, clip.length);

    }
}
