using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO; //파일을 생성, 수정, 삭제
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;


[Serializable]
public class GameBaseInfo
{
    public SceneEnum sceneEnum;  // 저장 당시의 씬 이름
    public string dateTime;   // 저장된 날짜와 시간

    public GameBaseInfo()
    {
        sceneEnum = SceneEnum.Hotel_Day1;
    }
}

[Serializable]
public class GameDataCore
{
    public GameDataCore()
    {
        sceneIndividualData = new SceneIndividualData();
        gameBaseInfo = new GameBaseInfo();
        playerGameData = new PlayerGameData();
        questGameData = new QuestGameData();
    }
    public SceneIndividualData sceneIndividualData;
    public GameBaseInfo gameBaseInfo;
    public PlayerGameData playerGameData;
    public QuestGameData questGameData;
}

[Serializable]
public class SceneIndividualData
{
    // 씬 데이터 구현
    public string sceneName;
    public SceneGameData sceneGameData;
    public List<NPCGameData> npcGameData;
    public List<MonsterGameData> monsterGameData;

    public SceneIndividualData()
    {
        sceneGameData = new SceneGameData();
        npcGameData = new List<NPCGameData>();
        monsterGameData = new List<MonsterGameData>();
    }
}

public class GameDataSaveLoadManager : SingletonManager<GameDataSaveLoadManager>
{

    [TabGroup("Data", "DataManager", SdfIconType.GearFill, TextColor = "orange")]

    [TabGroup("Data", "DataManager")][HideInInspector] public PlayerGameDataManager playerGameDataManager;
    [TabGroup("Data", "DataManager")][HideInInspector] public NPCGameDataManager npcGameDataManager;
    [TabGroup("Data", "DataManager")][HideInInspector] public MonsterGameDataManager monsterGameDataManager;
    [TabGroup("Data", "DataManager")][HideInInspector] public SceneGameDataManager sceneGameDataManager;
    [TabGroup("Data", "DataManager")][HideInInspector] public QuestGameDataManager questGameDataManager;
    [TabGroup("Data", "DataManager")][HideInInspector] public GameSettings gameSettings;

    [Title("SavingCanvas")]
    public GameObject savingCanvas;

    protected override void Awake()
    {
        base.Awake();

        if (FindObjectsOfType<GameDataSaveLoadManager>().Length != 1)
        {
            Destroy(gameObject);
            return;
        }

        playerGameDataManager = GetComponent<PlayerGameDataManager>();
        npcGameDataManager = GetComponent<NPCGameDataManager>();
        monsterGameDataManager = GetComponent<MonsterGameDataManager>();
        sceneGameDataManager = GetComponent<SceneGameDataManager>();
        questGameDataManager = GetComponent<QuestGameDataManager>();

        gameSettings = GetComponent<GameSettings>();
    }

    private static bool isSaveLocked = false;
    /// <summary>
    /// 특정 구역에서 세이브 막기 [완]
    /// </summary>
    public static bool IsSaveLocked
    {
        get
        {
            return isSaveLocked;
        }
    }

    private static int _currentSlotId = -1;
    /// <summary>
    /// 현재 선택한 슬롯의 ID [완]
    /// </summary>
    public static int CurrentSlotId
    {
        get
        {
            return _currentSlotId;
        }
    }

    public static string returnFilePath(int index)
    {
        return $"{Application.persistentDataPath}/GameData{index}.json";
    }
    

    /// <summary>
    /// 인덱스 번호에 해당하는 세이브 파일이 있는지 [완]
    /// </summary>
    public bool FileExists(int index)
    {
        return File.Exists(returnFilePath(index));
    }

    private GameDataCore gameDataCore;
    public void CreatedNewData()
    {
        gameDataCore = new GameDataCore();
    }

    public SceneEnum ReturnSceneEnum()
    {
        return gameDataCore.gameBaseInfo.sceneEnum;
    }
    public void LoadGameData(int slot_Id)
    {
        if (FileExists(slot_Id) == false)
        {
            CreatedNewData();
            return;
        } 

        // 현재 슬롯 ID 등록 [완]
        _currentSlotId = slot_Id;

        // 파일 읽기
        string fromJson = File.ReadAllText(returnFilePath(slot_Id));
        gameDataCore = JsonConvert.DeserializeObject<GameDataCore>(fromJson);
    }
    public void SaveGameData(int slot_Id)
    {
        StartCoroutine(SavingCanvas());
        // UniverSal Data
        // 게임 내 씬에 관계없이 저장될 데이터 [필]
        {
            gameDataCore.gameBaseInfo.dateTime = DateTime.Now.ToString();
            gameDataCore.gameBaseInfo.sceneEnum = (SceneEnum)SceneManager.GetActiveScene().buildIndex;

            gameDataCore.playerGameData = playerGameDataManager.GetData();
            // 쿼스트 게임 데이터 구현 필요
            gameDataCore.questGameData = questGameDataManager.GetData();
        }

        // Scene Individual Data
        // 게임 씬에 따라 별도로 저장이 필요한 데이터
        {
            // 현재 씬의 데이터를 가져오기 [필]

            gameDataCore.sceneIndividualData.sceneName = SceneManager.GetActiveScene().name;
            gameDataCore.sceneIndividualData.sceneGameData = sceneGameDataManager.GetData();
            gameDataCore.sceneIndividualData.npcGameData = npcGameDataManager.GetData();
            gameDataCore.sceneIndividualData.monsterGameData = monsterGameDataManager.GetData();
        }

        //파일 저장 [완]
        string toJsonData = JsonConvert.SerializeObject(gameDataCore);
        File.WriteAllText(returnFilePath(slot_Id), toJsonData);
    }
    public void RemoveGameData(int slotId) //[완]
    {
        if (FileExists(slotId) == false)
            return;

        File.Delete(returnFilePath(slotId));
    }

    public void TryLockSave(bool isEnable) //[완]
{
        isSaveLocked = isEnable;
    }

    public void ApplayGameData()
    {
        sceneGameDataManager.ApplyGameData(gameDataCore.sceneIndividualData.sceneGameData);

        questGameDataManager.ApplyGameData(gameDataCore.questGameData);
        npcGameDataManager.ApplyGameData(gameDataCore.sceneIndividualData.npcGameData);
        monsterGameDataManager.ApplyGameData(gameDataCore.sceneIndividualData.monsterGameData);

        playerGameDataManager.ApplyGameData(gameDataCore.playerGameData);
    }

    IEnumerator SavingCanvas()
    {
        savingCanvas.SetActive(true);
        yield return new WaitForSeconds(3);
        savingCanvas.SetActive(false);
    }
}

