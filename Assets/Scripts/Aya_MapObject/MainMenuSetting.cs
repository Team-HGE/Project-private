using UnityEngine;

public class MainMenuSetting : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.PlayBackGroundSound(BackGroundSound.MainMenuSound, false, 1f);
    }
    public void NewGame()
    {
        GameDataSaveLoadManager.Instance.CreatedNewData();
        AudioManager.Instance.StopBackGroundSound(BackGroundSound.MainMenuSound, true);
        GameManager.Instance.fadeManager.fadeComplete += SetSwitch;
        GameManager.Instance.fadeManager.MoveScene(SceneEnum.Hotel_Day1);
    }
    public void LoadGame()
    {
        GameDataSaveLoadManager.Instance.LoadGameData(0);
        AudioManager.Instance.StopBackGroundSound(BackGroundSound.MainMenuSound, true);
        GameManager.Instance.fadeManager.fadeComplete += SetSwitch;
        GameManager.Instance.fadeManager.MoveScene(GameDataSaveLoadManager.Instance.ReturnSceneEnum());
    }

    void SetSwitch()
    {
        EventManager.Instance.SetSwitch(GameSwitch.IsPlayingGame, false);
    }
}
