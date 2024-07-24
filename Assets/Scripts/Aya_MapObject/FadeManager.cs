using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum SceneEnum
{
    MainMenuScene,
    Hotel_A_Loading,
    AScene,
    BScene
}
public class FadeManager : MonoBehaviour
{
    [SerializeField] FadeEffect fadeEffect;
    [SerializeField] GameObject sceneLoading;
    public void FadeStart()
    {
        StartCoroutine(Fade());
    }
    public void FadeImmediately() 
    {
        fadeEffect.FadeImmediately();
    }
    public void MoveScene(SceneEnum sceneEnum)
    {
        StartCoroutine(MoveSceneFade(sceneEnum));
    }
    private IEnumerator Fade()
    {
        GameManager.Instance.PlayerStateMachine.Player.PlayerControllOnOff();
        yield return fadeEffect.UseFadeEffect(FadeState.FadeOut);
        yield return fadeEffect.UseFadeEffect(FadeState.FadeIn);
        GameManager.Instance.PlayerStateMachine.Player.PlayerControllOnOff();
        fadeEffect.OffFadeObject();
    }
    public bool loadComplete;
    private IEnumerator MoveSceneFade(SceneEnum sceneEnum)
    {
        GameManager.Instance.PlayerStateMachine.Player.PlayerControllOnOff();
        yield return fadeEffect.UseFadeEffect(FadeState.FadeOut);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync((int)sceneEnum);
        //loadComplete = false;
        sceneLoading.SetActive(true);
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
        GameManager.Instance.PlayerStateMachine.Player.PlayerControllOnOff();
        yield return new WaitForSeconds(3);
        sceneLoading .SetActive(false);
        //loadComplete = true;
        //yield return new WaitUntil(()=> loadComplete);
        yield return fadeEffect.UseFadeEffect(FadeState.FadeIn);
        GameManager.Instance.PlayerStateMachine.Player.PlayerControllOnOff();

        fadeEffect.OffFadeObject();
    }
}