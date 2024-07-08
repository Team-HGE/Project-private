
using UnityEngine;

public interface INoise
{ 
    public float NoiseTransitionTime { get; set; }
    public float NoiseMin { get; set;}
    public float NoiseMax { get; set; }
    public float NoiseAmount { get; set; }
    public float DecreaseSpeed { get; set; }

    public SoundSource PlayNoise(AudioClip[] audioClips, string tag);


    //public void MakeNoise(float transitionTime, float min, float max, float speed);

    //public void PlayNoise();
}
