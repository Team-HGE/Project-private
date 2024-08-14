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
        // ���� �������� �����̴��� ��ۿ� ����
        volumeSlider.value = GameSettings.Instance.volume;
        sensitivitySlider.value = GameSettings.Instance.mouseSensitivity;
        fullscreenToggleButton.onClick.AddListener(ToggleFullScreen);
        

        // �̺�Ʈ ������ ����
        volumeSlider.onValueChanged.AddListener(SetVolume);
        sensitivitySlider.onValueChanged.AddListener(SetMouseSensitivity);
        
    }

    public void SetVolume(float volume)
    {
        GameSettings.Instance.volume = volume;
        AudioListener.volume = volume; // ���� ���� ������ �ݿ�
        GameSettings.Instance.SaveSettings(); // ���� ������ ����
    }

    public void SetMouseSensitivity(float sensitivity)
    {
        GameSettings.Instance.mouseSensitivity = sensitivity;

        // ���콺 ������ �÷��̾� ��Ʈ�ѷ��� �ݿ� (����)
        //playerController.mouseSensitivity = sensitivity;
        GameSettings.Instance.SaveSettings();
    }


    void ToggleFullScreen()
    {
        GameSettings.Instance.isFullScreen = !GameSettings.Instance.isFullScreen;

        if (GameSettings.Instance.isFullScreen)
        {
            // ��üȭ�� ���� ��ȯ
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
            GameSettings.Instance.SaveSettings();
        }
        else
        {
            // â ���� ��ȯ
            Screen.SetResolution(1920, 1080, false);
            GameSettings.Instance.SaveSettings();
        }
    }

    void UpdateFullScreenButtonText()
    {
        // ���� ��üȭ�� ���¿� ���� ��ư �ؽ�Ʈ ����
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