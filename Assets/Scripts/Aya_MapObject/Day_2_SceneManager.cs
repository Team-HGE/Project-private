using Cinemachine;
using UnityEngine;

public class Day_2_SceneManager : MonoBehaviour
{
    private static Day_2_SceneManager _instance;
    public static Day_2_SceneManager Instance
    {
        get
        {
            _instance ??= FindObjectOfType<Day_2_SceneManager>();
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
        GameManager.Instance.lightManager.elementsForFloors.Clear();
    }

    private void Start()
    {
        if (GameManager.Instance.lightManager.levers.Count > 0)
        {
            GameManager.Instance.lightManager.levers.Clear();
        }

        GameManager.Instance.cinemachineManager.mainCamera = mainCamera;
        GameManager.Instance.cinemachineManager.playerVC = playerVC;
        GameManager.Instance.jumpScareManager.flashLight = playerFlashLight;
        GameManager.Instance.jumpScareManager.blackBG = blackBG;
    }

    [Header("Cinemachine")]
    [SerializeField] CinemachineVirtualCamera playerVC;
    [SerializeField] CinemachineBrain mainCamera;
    [SerializeField] GameObject blackBG;

    [Header("Light")]
    [SerializeField] Light playerFlashLight;
}
