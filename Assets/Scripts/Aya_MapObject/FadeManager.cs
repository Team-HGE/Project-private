using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum SceneEnum
{
    MainMenuScene,
    AScene,
    BScene
}
public class FadeManager : MonoBehaviour
{
    [SerializeField] FadeEffect fadeEffect;
    public GameObject[] sceneLoadings;
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
        GameManager.Instance.PlayerStateMachine.Player.PlayerControllOnOff();
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
        GameManager.Instance.PlayerStateMachine.Player.PlayerControllOnOff();
        fadeEffect.OffFadeObject();
    }
    public bool loadComplete;
    public int sceneIndex;
    private IEnumerator MoveSceneFade(SceneEnum sceneEnum)
    {
        switch (sceneEnum)
        {
            case SceneEnum.AScene:
                sceneIndex = 0;
                break;
            case SceneEnum.BScene:
                sceneIndex = 1;
                break;
        }
        GameManager.Instance.PlayerStateMachine.Player.PlayerControllOnOff();
        yield return fadeEffect.UseFadeEffect(FadeState.FadeOut);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync((int)sceneEnum);
        //loadComplete = false;
        sceneLoadings[sceneIndex].SetActive(true);
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
        GameManager.Instance.PlayerStateMachine.Player.PlayerControllOnOff();
        yield return new WaitForSeconds(3);
        sceneLoadings[sceneIndex].SetActive(false);
        //loadComplete = true;
        //yield return new WaitUntil(()=> loadComplete);
        yield return fadeEffect.UseFadeEffect(FadeState.FadeIn);
        GameManager.Instance.PlayerStateMachine.Player.PlayerControllOnOff();

        fadeEffect.OffFadeObject();
    }
}
