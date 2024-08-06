using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlaySEAudio : MonoBehaviour
{
    public void PlayAudioCIip(AudioClip clip)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();
        Destroy(audioSource, clip.length);

    }
}
