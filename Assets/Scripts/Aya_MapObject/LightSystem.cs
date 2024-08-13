using UnityEngine;

public class LightSystem : MonoBehaviour
{
    public Light thisLight;
    public MeshRenderer[] parentRenderer;
    public Floor floor;
    private void Start()
    {
        thisLight ??= GetComponent<Light>();

        GameManager.Instance.lightManager.AddLightToFloor(floor, thisLight);
        if (parentRenderer != null)
        {
            foreach (var renderer in parentRenderer)
            {
                GameManager.Instance.lightManager.AddRendererToFloor(floor, renderer);
            }
        }
    }
}
