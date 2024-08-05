using UnityEngine;

public class PlaySEAudio : MonoBehaviour
{
    public void PlaySEAudioClip(AudioClip clip)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();
        Destroy(audioSource, clip.length);
    }
}

