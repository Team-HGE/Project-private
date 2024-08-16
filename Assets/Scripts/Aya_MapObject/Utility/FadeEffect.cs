using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum FadeState
{
    FadeIn, // 점점 밝아짐
    FadeOut, // 점점 어두워짐
    FadeInOut, // 밝 -> 어
    FadeOutIn, // 어 -> 밝
    FadeLoop,  // 여러번 반복
}
public class FadeEffect : MonoBehaviour
{
    [SerializeField]
    [Range(0.01f, 10f)]
    private float fadeTime;

    [SerializeField]
    private AnimationCurve fadeCurve;
    private Image image;
    private FadeState fadeState;
    void Awake()
    {
        image = GetComponent<Image>();
        this.gameObject.SetActive(false);
    }
    public void FadeImmediately()
    {
        Color color = image.color;
        color.a = 1;
        image.color = color;
        image.gameObject.SetActive(true);
    }
    public Coroutine UseFadeEffect(FadeState fade)
    {
        this.gameObject.SetActive(true);
        return OnFade(fade);
    }

    private Coroutine OnFade(FadeState state)
    {
        fadeState = state;

        switch (fadeState)
        {
            case FadeState.FadeIn:
                return StartCoroutine(Fade(1, 0));
            case FadeState.FadeOut:
                return StartCoroutine(Fade(0, 1));
            case FadeState.FadeInOut:
                return StartCoroutine(FadeInOut());
            case FadeState.FadeOutIn:
                return StartCoroutine(FadeOutIn());
            case FadeState.FadeLoop:
                return StartCoroutine(FadeInOut());
        }
        return null;
    }
    private IEnumerator FadeInOut()
    {
        while (true)
        {
            // 코루틴 내부에서 코루틴 함수를 호출하면 해당 코루틴 함수가 종료되어야 다음 문장이 실행
            yield return StartCoroutine(Fade(1, 0)); // Fade In

            yield return StartCoroutine(Fade(0, 1)); // Fade Out

            if (fadeState == FadeState.FadeInOut)
            {
                this.gameObject.SetActive(false);
                break; // 1회만 재생하는 상태
            }
        }
    }
    private IEnumerator FadeOutIn()
    {
        while (true)
        {
            // 코루틴 내부에서 코루틴 함수를 호출하면 해당 코루틴 함수가 종료되어야 다음 문장이 실행
            yield return StartCoroutine(Fade(0, 1)); // Fade Out

            yield return StartCoroutine(Fade(1, 0)); // Fade In

            if (fadeState == FadeState.FadeOutIn)
            {
                this.gameObject.SetActive(false);
                break; // 1회만 재생하는 상태
            }

        }
    }


    private IEnumerator Fade(float start, float end)
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / fadeTime;

            Color color = image.color;
            color.a = Mathf.Lerp(start, end, fadeCurve.Evaluate(percent));
            image.color = color;
            yield return null;
        }
    }


    public void OffFadeObject()
    {
        if (this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(false);
        }
    }
}