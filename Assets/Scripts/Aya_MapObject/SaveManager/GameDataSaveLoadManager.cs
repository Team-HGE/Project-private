using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO; //������ ����, ����, ����
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

[Serializable]
public class GameBaseInfo
{
    public string sceneName;  // ���� ����� �� �̸�
    public string dateTime;   // ����� ��¥�� �ð�
}

[Serializable]
public class GameDataCore
{
    public GameDataCore()
    {
        sceneIndividualDatas = new SceneIndividualData[5];
        gameBaseInfo = new GameBaseInfo();
        playerGameData = new PlayerGameData();
        questGameData = new QuestGameData();
    }
    public SceneIndividualData[] sceneIndividualDatas;
    public GameBaseInfo gameBaseInfo;
    public PlayerGameData playerGameData;
    public QuestGameData questGameData;
}

[Serializable]
public class SceneIndividualData
{
    // �� ������ ����
    public string sceneName;
    public SceneGameData sceneGameData;
    public List<NPCGameData> npcGameData;
    public List<MonsterGameData> monsterGameData;
}

public class GameDataSaveLoadManager : SingletonManager<GameDataSaveLoadManager>
{

    [TabGroup("Data", "DataManager", SdfIconType.GearFill, TextColor = "orange")]

    [TabGroup("Data", "DataManager")][HideInInspector] public PlayerGameDataManager playerGameDataManager;
    [TabGroup("Data", "DataManager")][HideInInspector] public NPCGameDataManager npcGameDataManager;
    [TabGroup("Data", "DataManager")][HideInInspector] public MonsterGameDataManager monsterGameDataManager;
    [TabGroup("Data", "DataManager")][HideInInspector] public SceneGameDataManager sceneGameDataManager;
    [TabGroup("Data", "DataManager")][HideInInspector] public QuestGameDataManager questGameDataManager;


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
    }

    private static bool isSaveLocked = false;
    /// <summary>
    /// Ư�� �������� ���̺� ���� [��]
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
    /// ���� ������ ������ ID [��]
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
    /// �ε��� ��ȣ�� �ش��ϴ� ���̺� ������ �ִ��� [��]
    /// </summary>
    public bool FileExists(int index)
    {
        return File.Exists(returnFilePath(index));
    }

    public void LoadGameData(int slot_Id, string? forceSceneName = null)
    {
        if (FileExists(slot_Id) == false) return;

        // ���� ���� ID ��� [��]
        _currentSlotId = slot_Id;

        // ���� �б�
        string fromJson = File.ReadAllText(returnFilePath(slot_Id));
        GameDataCore gameDataCore = JsonConvert.DeserializeObject<GameDataCore>(fromJson);

        // Universal Data
        // ���� ���� ������� ����Ǵ� ������ [��]
        {
            playerGameDataManager.ApplyGameData(gameDataCore.playerGameData);
            questGameDataManager.ApplyGameData(gameDataCore.questGameData);


            // �߰� ���� �ʿ�
        }
        // Scene Individual Data
        // ���� ���� ���� ������ ������ �ʿ��� ������ [��]
        {
            foreach (SceneIndividualData indidualData in gameDataCore.sceneIndividualDatas)
            {
                if (indidualData.sceneName == (forceSceneName is null? gameDataCore.gameBaseInfo.sceneName : forceSceneName))
                {
                    sceneGameDataManager.ApplyGameData(indidualData.sceneGameData);
                    npcGameDataManager.ApplyGameData(indidualData.npcGameData);
                    monsterGameDataManager. ApplyGameData(indidualData.monsterGameData);

                    // �߰� ���� �ʿ�
                }
            }
        }
    }
    public void SaveGameData(int slot_Id)
    {
        GameDataCore gameDataCore = new GameDataCore();

        // UniverSal Data
        // ���� �� ���� ������� ����� ������ [��]
        {
            gameDataCore.gameBaseInfo.dateTime = DateTime.Now.ToString();
            gameDataCore.gameBaseInfo.sceneName = SceneManager.GetActiveScene().name;

            gameDataCore.playerGameData = playerGameDataManager.GetData();
            // ����Ʈ ���� ������ ���� �ʿ�
            gameDataCore.questGameData = questGameDataManager.GetData();
        }

        // Scene Individual Data
        // ���� ���� ���� ������ ������ �ʿ��� ������
        {
            List<SceneIndividualData> sceneIndividualDatas = new List<SceneIndividualData>();

            if (FileExists(slot_Id) == true) // ���� �� ���� �����Ͱ� �ִٸ� ���� �б� [��]
            {
                string fromJson = File.ReadAllText(returnFilePath(slot_Id));
                sceneIndividualDatas = JsonConvert.DeserializeObject<GameDataCore>(fromJson).sceneIndividualDatas.ToList();
            }

            // ���� ���� �����͸� �������� [��]
            SceneIndividualData sceneIndividualData = new SceneIndividualData();
            {
                sceneIndividualData.sceneName = SceneManager.GetActiveScene().name;
                sceneIndividualData.sceneGameData = sceneGameDataManager.GetData();
                sceneIndividualData.npcGameData = npcGameDataManager.GetData();
                sceneIndividualData.monsterGameData = monsterGameDataManager.GetData();
                // �ʿ��Ѱ� �� ���� �ʿ�

            }

            // �̹� ����� ���Ͽ��� ���� ���� ����Ⱑ �����ϸ� ����� [��]
            for (int i = 0; i < sceneIndividualDatas.Count; i++)
            {
                if (sceneIndividualDatas[i].sceneName == sceneIndividualData.sceneName)
                {
                    sceneIndividualDatas[i] = sceneIndividualData;
                    goto Overrided;
                }
            }

            // ����⸦ ���� ���ߴٸ� (���� ó�� �����ϴ� ����) [��]
            sceneIndividualDatas.Add(sceneIndividualData);

            Overrided:;
            // �迭�� ��ȯ�Ͽ� ���� [��]
            gameDataCore.sceneIndividualDatas = sceneIndividualDatas.ToArray();
        }

        //���� ���� [��]
        string toJsonData = JsonConvert.SerializeObject(gameDataCore);
        File.WriteAllText(returnFilePath(slot_Id), toJsonData);
    }
    public void RemoveGameData(int slotId) //[��]
    {
        if (FileExists(slotId) == false)
            return;

        File.Delete(returnFilePath(slotId));
    }

    public void TryLockSave(bool isEnable) //[��]
{
        isSaveLocked = isEnable;
    }
}

