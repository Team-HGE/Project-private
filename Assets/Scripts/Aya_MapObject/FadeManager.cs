using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum SceneEnum
{
    AScene,
    BScene
}
public class FadeManager : MonoBehaviour
{
    [SerializeField] FadeEffect fadeEffect;
    [SerializeField] GameObject sceneLoading;
    [SerializeField] CharacterController playerController;
    public void FadeStart()
    {
        StartCoroutine(Fade());
    }
    public void MoveScene(SceneEnum sceneEnum)
    {
        StartCoroutine(MoveSceneFade(sceneEnum));
    }
    private IEnumerator Fade()
    {
        yield return fadeEffect.UseFadeEffect(FadeState.FadeOut);
        yield return fadeEffect.UseFadeEffect(FadeState.FadeIn);
        fadeEffect.OffFadeObject();
    }
    public bool loadComplete;
    private IEnumerator MoveSceneFade(SceneEnum sceneEnum)
    {
        playerController = GameManager.Instance.exampleBar.player.GetComponent<CharacterController>();
        yield return playerController.enabled = false;
        yield return fadeEffect.UseFadeEffect(FadeState.FadeOut);
        SceneManager.LoadScene((int)sceneEnum);
        loadComplete = false;
        sceneLoading.SetActive(true);
        yield return new WaitForSeconds(3);
        sceneLoading .SetActive(false);
        loadComplete = true;
        yield return new WaitUntil(()=> loadComplete);
        yield return fadeEffect.UseFadeEffect(FadeState.FadeIn);

        playerController = GameManager.Instance.exampleBar.player.GetComponent<CharacterController>();
        yield return playerController.enabled = true;
        fadeEffect.OffFadeObject();
    }
}
