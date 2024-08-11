using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum SceneEnum
{
    Hotel_Day1,
    Hotel_Day2
}
public class FadeManager : MonoBehaviour
{
    [Header("Fade")]
    [SerializeField] FadeEffect fadeEffect;

    [Header("SceneLoading")]
    [SerializeField] GameObject[] sceneLoadings;
    [SerializeField] GameObject loadingBar;

    public event Action fadeComplete;
    public void FadeStart(FadeState fadeState)
    {
        StartCoroutine(Fade(fadeState));
    }

    public void FadeImmediately() 
    {
        fadeEffect.FadeImmediately();
    }
    public void MoveScene(SceneEnum sceneEnum)
    {
        StartCoroutine(MoveSceneFade(sceneEnum));
    }
    private IEnumerator Fade(FadeState fadeState)
    {
        switch (fadeState)
        {
            case FadeState.FadeOut:
                yield return fadeEffect.UseFadeEffect(FadeState.FadeOut);
                break;
            case FadeState.FadeIn:
                yield return fadeEffect.UseFadeEffect(FadeState.FadeIn);
                break;
            case FadeState.FadeOutIn:
                yield return fadeEffect.UseFadeEffect(FadeState.FadeOut);
                yield return fadeEffect.UseFadeEffect(FadeState.FadeIn);
                break;
            case FadeState.FadeInOut:
                yield return fadeEffect.UseFadeEffect(FadeState.FadeIn);
                yield return fadeEffect.UseFadeEffect(FadeState.FadeOut);
                break;
        }
        fadeComplete?.Invoke();
        fadeEffect.OffFadeObject();
    }
    public bool loadComplete;
    public int sceneIndex;
    private IEnumerator MoveSceneFade(SceneEnum sceneEnum)
    {
        BackGroundSound backGroundSound = BackGroundSound.MainMenuSound;
        switch (sceneEnum)
        {
            case SceneEnum.Hotel_Day1:
                //sceneIndex = (int)SceneEnum.BScene;
                backGroundSound = BackGroundSound.ASceneSound;
                break;
            case SceneEnum.Hotel_Day2:
                //sceneIndex = (int)SceneEnum.BScene;
                backGroundSound = BackGroundSound.BSceneSound;
                break;
        }
        GameManager.Instance.PlayerStateMachine.Player.PlayerControllOff();
        yield return fadeEffect.UseFadeEffect(FadeState.FadeOut);
        //AudioManager.Instance.StopAllClips();
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync((int)sceneEnum);

        int rand = UnityEngine.Random.Range(0,sceneLoadings.Length);
        sceneLoadings[rand].SetActive(true);
        loadingBar.SetActive(true);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }
        GameManager.Instance.PlayerStateMachine.Player.PlayerControllOff();
        yield return new WaitForSeconds(3);
        sceneLoadings[rand].SetActive(false);
        loadingBar.SetActive(false);
        yield return fadeEffect.UseFadeEffect(FadeState.FadeIn);
        //AudioManager.Instance.PlaySound(backGroundSound);
        GameManager.Instance.PlayerStateMachine.Player.PlayerControllOn();

        fadeComplete?.Invoke();
        fadeEffect.OffFadeObject();
    }
}
