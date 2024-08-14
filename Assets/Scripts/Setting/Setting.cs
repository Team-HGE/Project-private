using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : SingletonManager<GameSettings>
{
    public float volume = 1f;
    public float mouseSensitivity = 100f;
    public bool isFullScreen = true;
    

    void Start()
    {
    }

    // Update is called once per frame

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("Volume", GameSettings.Instance.volume);
        PlayerPrefs.SetFloat("MouseSensitivityX", GameSettings.Instance.mouseSensitivity);
        PlayerPrefs.SetInt("FullScreen", GameSettings.Instance.isFullScreen ? 1 : 0);
        

        PlayerPrefs.Save(); // ����
    }


    public void LoadSettings()
    {
        // �ʿ信 ���� ���� �ҷ����� ����� �߰��� �� ����
    }
}
