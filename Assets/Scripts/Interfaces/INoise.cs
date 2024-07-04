
public interface INoise
{ 
    public float NoiseTransitionTime { get; set; }
    public float NoiseMin { get; set;}
    public float NoiseMax { get; set; }
    public float NoiseAmount { get; set; }
    public float DecreaseSpeed { get; set; }

    public void MakeNoise(float transitionTime, float min, float max, float speed);

    public void PlayNoise();
}
