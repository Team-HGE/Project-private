
using UnityEngine;

public interface INoise
{ 
    public float NoiseTransitionTime { get; set; }
    //public float NoiseMin { get; set;}
    //public float NoiseMax { get; set; }

    // 발생중인 소음
    public float CurNoiseAmount { get; set; }

    public float SumNoiseAmount { get; set; }
    public float DecreaseSpeed { get; set; }

    public SoundSource PlayNoise(AudioClip[] audioClips, string tag, float amount, float addVolume, float transitionTime, float pitch);


    //public void MakeNoise(float transitionTime, float min, float max, float speed);

    //public void PlayNoise();
}
