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
        volumeSlider.value = GameSettings.Instance.volume;
        sensitivitySlider.value = GameSettings.Instance.mouseSensitivity;
        fullscreenToggleButton.onClick.AddListener(ToggleFullScreen);
        

        // 이벤트 리스너 연결
        volumeSlider.onValueChanged.AddListener(SetVolume);
        sensitivitySlider.onValueChanged.AddListener(SetMouseSensitivity);
        
    }

    public void SetVolume(float volume)
    {
        GameSettings.Instance.volume = volume;
        AudioListener.volume = volume; // 실제 게임 음량에 반영
        GameSettings.Instance.SaveSettings(); // 변경 사항을 저장
    }

    public void SetMouseSensitivity(float sensitivity)
    {
        GameSettings.Instance.mouseSensitivity = sensitivity;

        // 마우스 감도를 플레이어 컨트롤러에 반영 (예시)
        //playerController.mouseSensitivity = sensitivity;
        GameSettings.Instance.SaveSettings();
    }


    void ToggleFullScreen()
    {
        GameSettings.Instance.isFullScreen = !GameSettings.Instance.isFullScreen;

        if (GameSettings.Instance.isFullScreen)
        {
            // 전체화면 모드로 전환
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
            GameSettings.Instance.SaveSettings();
        }
        else
        {
            // 창 모드로 전환
            Screen.SetResolution(1920, 1080, false);
            GameSettings.Instance.SaveSettings();
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