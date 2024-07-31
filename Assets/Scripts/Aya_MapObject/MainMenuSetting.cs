using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSetting : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.PlaySound(BackGroundSound.MainMenuSound);
    }
    public void LoadAScene()
    {
        AudioManager.Instance.StopSound(BackGroundSound.MainMenuSound);
        SceneManager.LoadScene((int)SceneEnum.AScene);
    }
}
