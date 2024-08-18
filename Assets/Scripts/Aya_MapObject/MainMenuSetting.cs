using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSetting : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.PlayBackGroundSound(BackGroundSound.MainMenuSound);
    }
    public void NewGame()
    {
        GameDataSaveLoadManager.Instance.CreatedNewData();
        AudioManager.Instance.StopBackGroundSound(BackGroundSound.MainMenuSound);
        GameManager.Instance.fadeManager.MoveScene(SceneEnum.Hotel_Day1);
    }
    public void LoadGame()
    {
        GameDataSaveLoadManager.Instance.LoadGameData(0);
        AudioManager.Instance.StopBackGroundSound(BackGroundSound.MainMenuSound);
        GameManager.Instance.fadeManager.MoveScene(GameDataSaveLoadManager.Instance.ReturnSceneEnum());
    }
}
