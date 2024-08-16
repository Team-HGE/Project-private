using Cinemachine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HotelFloorScene_DataManager : MonoBehaviour
{
    private static HotelFloorScene_DataManager _instance;
    public static HotelFloorScene_DataManager Instance
    {
        get
        {
            _instance ??= FindObjectOfType<HotelFloorScene_DataManager>();
            return _instance;
        }
    }
    private void Awake()
    {
        _instance = this;
        elevatorManager ??= GetComponent<ElevatorManager>();
        controller ??= GetComponent<HotelFloorScene_Controller>();

        LoadDataByInitialize();
    }
    private void Start()
    {
        GameManager.Instance.fadeManager.FadeStart(FadeState.FadeIn);
    }
    

    [Title("Manager")]
    public ElevatorManager elevatorManager;
    public HotelFloorScene_Controller controller;

    [Title("PLAYER")]
    public Transform player;
    [SerializeField] CinemachineVirtualCamera playerVC;
    public CinemachineVirtualCamera GetPlayerVC { get { return playerVC; } }

    [Title("NPC")]
    [SerializeField] public Transform[] npc_Transforms;
    public Transform[] GetNPC_Transform()
    {
        return npc_Transforms;
    }
    [Title("MONSTER")]
    [SerializeField] private Transform[] monsters_Transfroms;
    public Transform[] GetMonster_Transform()
    {
        return monsters_Transfroms;
    }

    [ShowInInspector]
    public IInitializeByLoadedDataWraper[] initializeByLoadedDatas;

    public void LoadDataByInitialize()
    {
        GameDataSaveLoadManager.Instance.playerGameDataManager.playerTransform = player;

        GameDataSaveLoadManager.Instance.ApplayGameData();

        foreach (var data in initializeByLoadedDatas)
        {
            data.InitializeByData();
        }
    }
    #region 에디터 버튼
    public void SetInitializeLoadData()
    {
        initializeByLoadedDatas = FIND(true);
    }

    private IInitializeByLoadedDataWraper[] FIND(bool includeInactive)
    {
        List<IInitializeByLoadedData> results = new List<IInitializeByLoadedData>();

        foreach (GameObject go in FindObjectsOfType<GameObject>(true))
        {
            if (includeInactive || go.activeInHierarchy)
            {
                // MonoBehaviour로부터 모든 컴포넌트를 가져오고, 인터페이스를 구현한 것만 필터링
                var components = go.GetComponents<MonoBehaviour>().OfType<IInitializeByLoadedData>().ToArray();

                if (components.Length > 0)
                {
                    results.AddRange(components);
                }
            }
        }

        // 중복 제거
        results = results.Distinct().ToList();

        IInitializeByLoadedDataWraper[] ret = new IInitializeByLoadedDataWraper[results.Count];
        for (int i = 0; i < ret.Length; i++)
        {
            ret[i] = new IInitializeByLoadedDataWraper(results[i]);
        }

        return ret;
    }
    #endregion
}
