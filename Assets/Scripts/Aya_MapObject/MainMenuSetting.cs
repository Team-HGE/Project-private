using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSetting : MonoBehaviour
{
    public void LoadAScene()
    {
        AudioManager.Instance.soundSource.Stop();
        SceneManager.LoadScene((int)SceneEnum.AScene);
    }
}
