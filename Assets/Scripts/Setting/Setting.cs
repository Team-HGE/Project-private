using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public float volume = 1f;
    public float mouseSensitivity = 100f;
    public bool isFullScreen = true;
    

    void Start()
    {

    }

    public void SaveSettings() 
    {
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.SetFloat("MouseSensitivityX", mouseSensitivity);
        PlayerPrefs.SetInt("FullScreen", isFullScreen ? 1 : 0);
        

        PlayerPrefs.Save(); // ����
    }


    public void LoadSettings()
    {
        // �ʿ信 ���� ���� �ҷ����� ����� �߰��� �� ����
    }
}
