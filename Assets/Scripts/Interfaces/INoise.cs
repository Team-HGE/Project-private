using UnityEngine;

public interface INoise
{ 
    public float NoiseTransitionTime { get; set; }

    // 발생중인 소음
    public float CurNoiseAmount { get; set; }

    public float SumNoiseAmount { get; set; }
    public float DecreaseSpeed { get; set; }

    public SoundSource PlayNoise(AudioClip[] audioClips, string tag, float amount, float addVolume, float transitionTime, float pitch);
}
