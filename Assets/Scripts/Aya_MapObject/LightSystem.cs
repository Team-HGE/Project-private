using UnityEngine;

public class LightSystem : MonoBehaviour
{
    public Light thisLight;
    public MeshRenderer[] parentRenderer;
    [SerializeField] private Floor floor;
    private void Start()
    {
        if (thisLight == null)
        {
            thisLight = GetComponent<Light>();
        }
        floor = LightInitializer.Instance.ReturnFloorOfLight(transform.position);
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
