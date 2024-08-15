using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO; //파일을 생성, 수정, 삭제
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


[Serializable]
public class GameBaseInfo
{
    public string sceneName;  // 저장 당시의 씬 이름
    public string dateTime;   // 저장된 날짜와 시간
}

[Serializable]
public class GameDataCore
{
    public SceneIndividualData[] sceneIndividualDatas;
    public GameBaseInfo gameBaseInfo;
    public PlayerGameData playerGameData;

}

[Serializable]
public class SceneIndividualData
{
    // 씬 데이터 구현
    public string sceneName;
    public SceneGameData sceneGameData;
}

public class GameDataSaveLoadManager : SingletonManager<GameDataSaveLoadManager>
{

    [TabGroup("Data", "DataManager", SdfIconType.GearFill, TextColor = "orange")]

    [TabGroup("Data", "DataManager")][SerializeField] private PlayerGameDataManager playerGameDataManager;
    [TabGroup("Data", "DataManager")][SerializeField] private NPCGameDataManager npcGameDataManager;
    [TabGroup("Data", "DataManager")][SerializeField] private MonsterGameDataManager monsterGameDataManager;
    [TabGroup("Data", "DataManager")][SerializeField] private SceneGameDataManager sceneGameDataManager;
    [TabGroup("Data", "DataManager")][SerializeField] private QuestGameDataManager questGameDataManager;


    protected override void Awake()
    {
        base.Awake();

        if (FindObjectsOfType<GameDataSaveLoadManager>().Length != 1)
        {
            Destroy(gameObject);
            return;
        }
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



    private static readonly string mFileName = "GameData.data"; // 파일 이름 [완]
    public static string _FILE_PATH
    {
        get
        {
            return $"{Application.persistentDataPath}/{mFileName}";
        }
    }
    /// <summary>
    /// 인덱스 번호에 해당하는 세이브 파일이 있는지 [완]
    /// </summary>
    public bool FileExists(int index)
    {
        return File.Exists(_FILE_PATH + index);
    }


    public void LoadGameData(int slot_Id, string? forceSceneName = null)
    {
        if (FileExists(slot_Id) == false) return;

        // 현재 슬롯 ID 등록 [완]
        _currentSlotId = slot_Id;

        // 파일 읽기
        string fromJson = File.ReadAllText(_FILE_PATH + slot_Id);
        GameDataCore gameDataCore = JsonUtility.FromJson<GameDataCore>(fromJson);

        // Universal Data
        // 게임 씬에 상관없이 저장되는 데이터 [필]
        {
            playerGameDataManager.ApplyGameData(gameDataCore.playerGameData);


            // 추가 구현 필요
        }
        // Scene Individual Data
        // 게임 씬에 따라 별도로 저장이 필요한 데이터 [필]
        {
            foreach (SceneIndividualData indidualData in gameDataCore.sceneIndividualDatas)
            {
                if (indidualData.sceneName == (forceSceneName is null? gameDataCore.gameBaseInfo.sceneName : forceSceneName))
                {
                    sceneGameDataManager.ApplyGameData(indidualData.sceneGameData);




                    // 추가 구현 필요
                }
            }
        }
    }
    public void SaveGameData(int slot_Id)
    {
        GameDataCore gameDataCore = new GameDataCore();

        // UniverSal Data
        // 게임 내 씬에 관계없이 저장될 데이터 [필]
        {
            gameDataCore.gameBaseInfo.dateTime = System.DateTime.Now.ToString();
            gameDataCore.gameBaseInfo.sceneName = SceneManager.GetActiveScene().name;
            gameDataCore.playerGameData = playerGameDataManager.GetData();

            // 쿼스트 게임 데이터 구현 필요

        }

        // Scene Individual Data
        // 게임 씬에 따라 별도로 저장이 필요한 데이터
        {
            List<SceneIndividualData> sceneIndividualDatas = new List<SceneIndividualData>();

            if (FileExists(slot_Id) == true) // 기존 씬 파일 데이터가 있다면 파일 읽기 [완]
            {
                string fromJson = File.ReadAllText(_FILE_PATH + slot_Id);
                sceneIndividualDatas = JsonUtility.FromJson<GameDataCore>(fromJson).sceneIndividualDatas.ToList();
            }

            // 현재 씬의 데이터를 가져오기 [필]
            SceneIndividualData sceneIndividualData = new SceneIndividualData();
            sceneIndividualData.sceneName = SceneManager.GetActiveScene().name;
            sceneIndividualData.sceneGameData = sceneGameDataManager.GetData();
            // 필요한거 더 구현 필요



            // 이미 저장된 파일에서 현재 씬을 덮어쓰기가 가능하면 덮어쓰기 [완]
            for (int i = 0; i < sceneIndividualDatas.Count; i++)
            {
                if (sceneIndividualDatas[i].sceneName == sceneIndividualData.sceneName)
                {
                    sceneIndividualDatas[i] = sceneIndividualData;
                    goto Overrided;
                }
            }

            // 덮어쓰기를 하지 못했다면 (씬을 처음 저장하는 형태) [완]
            sceneIndividualDatas.Add(sceneIndividualData);

            Overrided:;
            // 배열로 변환하여 저장 [완]
            gameDataCore.sceneIndividualDatas = sceneIndividualDatas.ToArray();
        }

        //파일 저장 [완]
        string toJsonData = JsonUtility.ToJson(gameDataCore);
        File.WriteAllText(_FILE_PATH + slot_Id, toJsonData);
    }
    public void RemoveGameData(int slotId) //[완]
    {
        if (FileExists(slotId) == false)
            return;

        File.Delete(_FILE_PATH + slotId);
    }

    public void TryLockSave(bool isEnable) //[완]
{
        isSaveLocked = isEnable;
    }
}

