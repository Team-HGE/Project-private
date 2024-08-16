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
        //AudioManager.Instance.StopSound(BackGroundSound.MainMenuSound);
        GameDataSaveLoadManager.Instance.CreatedNewData();
        SceneManager.LoadScene((int)SceneEnum.Hotel_Day1);
    }
    public void LoadGame()
    {
        GameDataSaveLoadManager.Instance.LoadGameData(0);
        SceneManager.LoadScene((int)GameDataSaveLoadManager.Instance.ReturnSceneEnum());
    }
}
