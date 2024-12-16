using UnityEngine;

public class LightSystem : MonoBehaviour
{
    public Light thisLight;
    public MeshRenderer[] parentRenderer;
    [SerializeField] private Floor nowFloor;
    private void Start()
    {
        if (thisLight == null)
        {
            thisLight = GetComponent<Light>();
        }

        Floor? floor = FloorInitializer.Instance.ReturnLobbyPosition(transform.position);
        if (floor == null)
        {
            floor = FloorInitializer.Instance.ReturnFloorPosition(transform.position);
            nowFloor = floor.Value;
        }
        else
        {
            if (floor.HasValue)
            {
                nowFloor = floor.Value;
            }
            else
            {
                floor = FloorInitializer.Instance.ReturnFloorPosition(transform.position);
                nowFloor = floor.Value;
            }
        }
            
        
        GameManager.Instance.lightManager.AddLightToFloor(nowFloor, thisLight);

        if (parentRenderer != null)
        {
            foreach (var renderer in parentRenderer)
            {
                GameManager.Instance.lightManager.AddRendererToFloor(nowFloor, renderer);
            }
        }
    }
}
