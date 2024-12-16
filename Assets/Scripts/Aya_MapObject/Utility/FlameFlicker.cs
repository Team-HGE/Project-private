using System;
using UnityEngine;


[Serializable]
public struct MinMax
{
    public float min;
    public float max;

    public bool Flipped => max < min;

    public bool HasValue => min != 0 || max != 0;

    public float RealMin => Flipped ? max : min;
    public float RealMax => Flipped ? min : max;
    public Vector2 RealVector => this;
    public Vector2 Vector => new Vector2(min, max);

    public MinMax(float min, float max)
    {
        this.min = min;
        this.max = max;
    }

    public static implicit operator Vector2(MinMax minMax)
    {
        return new Vector2(minMax.RealMin, minMax.RealMax);
    }

    public static implicit operator MinMax(Vector2 vector)
    {
        MinMax result = default;
        result.min = vector.x;
        result.max = vector.y;
        return result;
    }

    public MinMax Flip() => new MinMax(max, min);

    public override string ToString() => $"({RealMin}, {RealMax})";
}
public class FlameFlicker : MonoBehaviour
{
    public Light FlameLight;
    public MinMax FlameFlickerLimits;
    public float FlameFlickerSpeed = 1f;

    [Header("Position Flicker")]
    public bool PositionFlicker;
    public float PositionFlickerSpeed = 1f;
    public Vector3 PositionFlickerMagnitude = new(0.1f, 0.1f, 0.1f);

    [Header("Optimization")]
    public bool Optimize = true;
    public float FlickerDistance = 10f;

    public Transform player;
    private Vector3 originalPosition;

    private void Awake()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        if (player == null)
        {
            player = GameManager.Instance.player.transform;
        }
        if (!FlameLight.enabled)
            return;

        // Optimization
        if (Optimize && Vector3.Distance(transform.position, player.position) > FlickerDistance)
            return;

        // Intensity Flicker
        float flicker = Mathf.PerlinNoise1D(Time.time * FlameFlickerSpeed);
        FlameLight.intensity = Mathf.Lerp(FlameFlickerLimits.RealMin, FlameFlickerLimits.RealMax, flicker);

        // Position Flicker
        if (PositionFlicker)
        {
            float xOffset = Perlin(Time.time * PositionFlickerSpeed, 1f);
            float yOffset = Perlin(Time.time * PositionFlickerSpeed, 2f);
            float zOffset = Perlin(Time.time * PositionFlickerSpeed, 3f);

            Vector3 flickerPosition = new(xOffset, yOffset, zOffset);
            transform.position = originalPosition + Vector3.Scale(flickerPosition, PositionFlickerMagnitude);
        }
    }

    private float Perlin(float x, float y)
    {
        float value = Mathf.PerlinNoise(x, y);
        return (value - 0.5f) * 2;
    }
}