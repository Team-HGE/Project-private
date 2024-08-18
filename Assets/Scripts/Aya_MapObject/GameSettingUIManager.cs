using UnityEngine;
using SlimUI.ModernMenu;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class GameSettingUIManager : MonoBehaviour
{
    public enum Theme { custom1, custom2, custom3 };
    [Title("THEME SETTINGS")]
    public Theme theme;
    private int themeIndex;
    public ThemedUIData themeController;

    [Title("PANELS")]
    [Tooltip("The UI Panel that holds the CONTROLS window tab")]
    public GameObject PanelControls;
    [Tooltip("The UI Panel that holds the VIDEO window tab")]
    public GameObject PanelVideo;
    [Tooltip("The UI Panel that holds the GAME window tab")]
    public GameObject PanelGame;

    // highlights in settings screen
    [Title("SETTINGS SCREEN")]
    [Tooltip("Highlight Image for when GAME Tab is selected in Settings")]
    public GameObject lineGame;
    [Tooltip("Highlight Image for when VIDEO Tab is selected in Settings")]
    public GameObject lineVideo;
    [Tooltip("Highlight Image for when CONTROLS Tab is selected in Settings")]
    public GameObject lineControls;
    [Tooltip("Highlight Image for when KEY BINDINGS Tab is selected in Settings")]
    public GameObject lineKeyBindings;

    [Title("SFX")]
    [Tooltip("The GameObject holding the Audio Source component for the HOVER SOUND")]
    public AudioSource hoverSound;
    [Tooltip("The GameObject holding the Audio Source component for the AUDIO SLIDER")]
    public AudioSource sliderSound;
    [Tooltip("The GameObject holding the Audio Source component for the SWOOSH SOUND when switching to the Settings Screen")]
    public AudioSource swooshSound;

    [Title("Canvas_Add")]
    public GameObject settingCanvas;

    [BoxGroup("Box")]
    public InfoMessageType EnumField = InfoMessageType.Info;

    public Dropdown qualityDropDown;
    void Start()
    {
        qualityDropDown.value = QualitySettings.GetQualityLevel();
        SetThemeColors();
    }
    public void SetQuality(int index) // 전반적인 세팅 조절
    {
        QualitySettings.SetQualityLevel(index);
    }

    public void SetAntiAliasing(int index) // 안티 엘리어싱
    {
        QualitySettings.antiAliasing = index;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && EventManager.Instance.GetSwitch(GameSwitch.IsPlayingGame))
        {
            ToggleSettingMenu();
        }
    }

    public void ReturnMainMenu()
    {
        ToggleSettingMenu();
        EventManager.Instance.SetSwitch(GameSwitch.IsPlayingGame, false);
        GameManager.Instance.fadeManager.MoveScene(SceneEnum.MainMenuScene);
        GameManager.Instance.fadeManager.fadeComplete += CursorNone;
    }

    public void CursorNone()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    void ToggleSettingMenu()
    {
        settingCanvas.SetActive(!settingCanvas.activeSelf);
        if (settingCanvas.activeSelf)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1;
            if (!DialogueManager.Instance.storyScript.ui.isPlayingStory)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        
    }
    void SetThemeColors()
    {
        switch (theme)
        {
            case Theme.custom1:
                themeController.currentColor = themeController.custom1.graphic1;
                themeController.textColor = themeController.custom1.text1;
                themeIndex = 0;
                break;
            case Theme.custom2:
                themeController.currentColor = themeController.custom2.graphic2;
                themeController.textColor = themeController.custom2.text2;
                themeIndex = 1;
                break;
            case Theme.custom3:
                themeController.currentColor = themeController.custom3.graphic3;
                themeController.textColor = themeController.custom3.text3;
                themeIndex = 2;
                break;
            default:
                Debug.Log("Invalid theme selected.");
                break;
        }
    }

    void DisablePanels()
    {
        PanelControls.SetActive(false);
        PanelVideo.SetActive(false);
        PanelGame.SetActive(false);

        lineGame.SetActive(false);
        lineControls.SetActive(false);
        lineVideo.SetActive(false);
        lineKeyBindings.SetActive(false);
    }
    #region 판넬
    public void GamePanel()
    {
        DisablePanels();
        PanelGame.SetActive(true);
        lineGame.SetActive(true);
    }

    public void VideoPanel()
    {
        DisablePanels();
        PanelVideo.SetActive(true);
        lineVideo.SetActive(true);
    }

    public void ControlsPanel()
    {
        DisablePanels();
        PanelControls.SetActive(true);
        lineControls.SetActive(true);
    }

    public void KeyBindingsPanel()
    {
        DisablePanels();
        MovementPanel();

        lineKeyBindings.SetActive(true);
    }

    public void MovementPanel()
    {
        DisablePanels();
    }

    public void CombatPanel()
    {
        DisablePanels();
    }

    public void GeneralPanel()
    {
        DisablePanels();
    }
    #endregion

    #region 소리
    public void PlayHover()
    {
        hoverSound.Play();
    }

    public void PlaySFXHover()
    {
        sliderSound.Play();
    }

    public void PlaySwoosh()
    {
        swooshSound.Play();
    }
    #endregion
}