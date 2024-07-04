
public interface INoise
{ 
    public float NoiseTransitionTime { get; set; }
    public float NoiseMin { get; set;}
    public float NoiseMax { get; set; }
    public float NoiseAmount { get; set; }

    public void MakeNoise(float second);

    public void PlayNoise();
}
