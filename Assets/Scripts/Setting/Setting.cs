using UnityEngine;
public class GameSettings : MonoBehaviour
{
    public float volume = 1f;
    public float mouseSensitivity = 100f;
    public bool isFullScreen = true;

    public void SaveSettings() 
    {
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.SetFloat("MouseSensitivityX", mouseSensitivity);
        PlayerPrefs.SetInt("FullScreen", isFullScreen ? 1 : 0);
        

        PlayerPrefs.Save(); // 저장
    }


    public void LoadSettings()
    {
        // 필요에 따라 설정 불러오기 기능을 추가할 수 있음
    }
}
