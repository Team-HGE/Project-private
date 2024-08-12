using UnityEngine;

public class RandomEvent
{

}
public class RandomEventManager : MonoBehaviour
{
    private static RandomEventManager _instance;
    public static RandomEventManager Instance
    {
        get
        {
            _instance ??= FindObjectOfType<RandomEventManager>();
            return _instance;
        }
    }
}
