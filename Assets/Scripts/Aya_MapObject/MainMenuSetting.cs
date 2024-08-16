using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSetting : MonoBehaviour
{
    private void Start()
    {
        
    }
    public void NewGame()
    {
        GameDataSaveLoadManager.Instance.CreatedNewData();

        GameManager.Instance.fadeManager.MoveScene(SceneEnum.Hotel_Day1);
    }
    public void LoadGame()
    {
        GameDataSaveLoadManager.Instance.LoadGameData(0);

        GameManager.Instance.fadeManager.MoveScene(GameDataSaveLoadManager.Instance.ReturnSceneEnum());
    }
}
