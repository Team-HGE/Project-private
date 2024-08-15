using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO; //������ ����, ����, ����
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


[Serializable]
public class GameBaseInfo
{
    public string sceneName;  // ���� ����� �� �̸�
    public string dateTime;   // ����� ��¥�� �ð�
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
    // �� ������ ����
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



    private static readonly string mFileName = "GameData.data"; // ���� �̸� [��]
    public static string _FILE_PATH
    {
        get
        {
            return $"{Application.persistentDataPath}/{mFileName}";
        }
    }
    /// <summary>
    /// �ε��� ��ȣ�� �ش��ϴ� ���̺� ������ �ִ��� [��]
    /// </summary>
    public bool FileExists(int index)
    {
        return File.Exists(_FILE_PATH + index);
    }


    public void LoadGameData(int slot_Id, string? forceSceneName = null)
    {
        if (FileExists(slot_Id) == false) return;

        // ���� ���� ID ��� [��]
        _currentSlotId = slot_Id;

        // ���� �б�
        string fromJson = File.ReadAllText(_FILE_PATH + slot_Id);
        GameDataCore gameDataCore = JsonUtility.FromJson<GameDataCore>(fromJson);

        // Universal Data
        // ���� ���� ������� ����Ǵ� ������ [��]
        {
            playerGameDataManager.ApplyGameData(gameDataCore.playerGameData);


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
            gameDataCore.gameBaseInfo.dateTime = System.DateTime.Now.ToString();
            gameDataCore.gameBaseInfo.sceneName = SceneManager.GetActiveScene().name;
            gameDataCore.playerGameData = playerGameDataManager.GetData();

            // ����Ʈ ���� ������ ���� �ʿ�

        }

        // Scene Individual Data
        // ���� ���� ���� ������ ������ �ʿ��� ������
        {
            List<SceneIndividualData> sceneIndividualDatas = new List<SceneIndividualData>();

            if (FileExists(slot_Id) == true) // ���� �� ���� �����Ͱ� �ִٸ� ���� �б� [��]
            {
                string fromJson = File.ReadAllText(_FILE_PATH + slot_Id);
                sceneIndividualDatas = JsonUtility.FromJson<GameDataCore>(fromJson).sceneIndividualDatas.ToList();
            }

            // ���� ���� �����͸� �������� [��]
            SceneIndividualData sceneIndividualData = new SceneIndividualData();
            sceneIndividualData.sceneName = SceneManager.GetActiveScene().name;
            sceneIndividualData.sceneGameData = sceneGameDataManager.GetData();
            // �ʿ��Ѱ� �� ���� �ʿ�



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
        string toJsonData = JsonUtility.ToJson(gameDataCore);
        File.WriteAllText(_FILE_PATH + slot_Id, toJsonData);
    }
    public void RemoveGameData(int slotId) //[��]
    {
        if (FileExists(slotId) == false)
            return;

        File.Delete(_FILE_PATH + slotId);
    }

    public void TryLockSave(bool isEnable) //[��]
{
        isSaveLocked = isEnable;
    }
}

