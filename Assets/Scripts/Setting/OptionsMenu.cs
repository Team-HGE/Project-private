using TMPro;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Slider volumeSlider;
    public Slider sensitivitySlider;
    public Button fullscreenToggleButton;
    public TextMeshProUGUI fullscreentext;


    void Start()
    {
        // 기존 설정값을 슬라이더와 토글에 적용
        volumeSlider.value = GameDataSaveLoadManager.Instance.gameSettings.volume;
        sensitivitySlider.value = GameDataSaveLoadManager.Instance.gameSettings.mouseSensitivity;
        fullscreenToggleButton.onClick.AddListener(ToggleFullScreen);
        

        // 이벤트 리스너 연결
        volumeSlider.onValueChanged.AddListener(SetVolume);
        sensitivitySlider.onValueChanged.AddListener(SetMouseSensitivity);
        
    }

    public void SetVolume(float volume)
    {
        GameDataSaveLoadManager.Instance.gameSettings.volume = volume;
        AudioListener.volume = volume; // 실제 게임 음량에 반영
        GameDataSaveLoadManager.Instance.gameSettings.SaveSettings(); // 변경 사항을 저장
    }

    public void SetMouseSensitivity(float sensitivity)
    {
        GameDataSaveLoadManager.Instance.gameSettings.mouseSensitivity = sensitivity;

        // 마우스 감도를 플레이어 컨트롤러에 반영 (예시)
        //playerController.mouseSensitivity = sensitivity;
        GameDataSaveLoadManager.Instance.gameSettings.SaveSettings();
    }


    void ToggleFullScreen()
    {
        GameDataSaveLoadManager.Instance.gameSettings.isFullScreen = !GameDataSaveLoadManager.Instance.gameSettings.isFullScreen;

        if (GameDataSaveLoadManager.Instance.gameSettings.isFullScreen)
        {
            // 전체화면 모드로 전환
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
            GameDataSaveLoadManager.Instance.gameSettings.SaveSettings();
        }
        else
        {
            // 창 모드로 전환
            Screen.SetResolution(1920, 1080, false);
            GameDataSaveLoadManager.Instance.gameSettings.SaveSettings();
        }
    }

    void UpdateFullScreenButtonText()
    {
        // 현재 전체화면 상태에 따라 버튼 텍스트 변경
        if (Screen.fullScreen == true)
        {
            fullscreentext.GetComponent<TMP_Text>().text = "on";
        }
        else if (Screen.fullScreen == false)
        {
            fullscreentext.GetComponent<TMP_Text>().text = "off";
        }
    }

}