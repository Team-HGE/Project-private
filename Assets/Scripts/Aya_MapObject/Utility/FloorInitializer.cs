using Sirenix.OdinInspector;
using UnityEngine;

public class FloorInitializer : MonoBehaviour
{
    private static FloorInitializer _instance;
    public static FloorInitializer Instance
    {
        get
        {
            _instance ??= FindObjectOfType<FloorInitializer>();
            return _instance;
        }
    }
    /// <summary>
    /// 씬 이동시 호출
    /// </summary>
    public void SetInitializerNull()
    {
        _instance = null;
    }

    [Title("Floor")]
    [SerializeField] private Transform[] floorTopTransforms;
    [SerializeField] private Transform[] floorBottomTransforms;

    [Title("Lobby")]
    [SerializeField] private Transform lobbyTopTransforms;
    [SerializeField] private Transform lobbyBottomTransforms;

    public Floor ReturnFloorPosition(Vector3 targetPos)
    {
        int floor = 0;
        for (int i = 0; i < floorTopTransforms.Length; i++)
        {
            if (targetPos.y >= floorBottomTransforms[i].position.y && targetPos.y <= floorTopTransforms[i].position.y)
            {
                floor = i;
                break;
            }
        }
        if (targetPos.x > 300)
        {
            // B동
            return (Floor)((int)Floor.BFloor1F << floor); 
        }
        else
        {
            // A동
            return (Floor)((int)Floor.AFloor1F << floor);
        }
    }

    public Floor? ReturnLobbyPosition(Vector3 targetPos)
    {
        if (targetPos.y >= lobbyBottomTransforms.position.y && targetPos.y <= lobbyTopTransforms.position.y)
        {
            return Floor.Lobby;
        }
        return null;
    }
}
