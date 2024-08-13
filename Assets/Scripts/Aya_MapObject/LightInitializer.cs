using UnityEngine;

public class LightInitializer : MonoBehaviour
{
    private static LightInitializer _instance;
    public static LightInitializer Instance
    {
        get
        {
            _instance ??= FindObjectOfType<LightInitializer>();
            return _instance;
        }
    }

    [SerializeField] private Transform[] floorTopTransforms;
    [SerializeField] private Transform[] floorBottomTransforms;

    public Floor ReturnFloorOfLight(Vector3 lightPos)
    {
        int floor = 0;
        for (int i = 0; i < floorTopTransforms.Length; i++)
        {
            if (lightPos.y >= floorBottomTransforms[i].position.y && lightPos.y <= floorTopTransforms[i].position.y)
            {
                floor = i;
                break;
            }
        }
        if (lightPos.x > 300)
        {
            // Bµ¿
            return (Floor)((int)Floor.BFloor1F << floor); 
        }
        else
        {
            // Aµ¿
            return (Floor)((int)Floor.AFloor1F << floor);
        }
    }
}
