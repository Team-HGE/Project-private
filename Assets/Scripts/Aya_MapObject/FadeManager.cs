using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeManager : MonoBehaviour
{
    [SerializeField] FadeEffect fadeEffect;
    
    public void FadeStart()
    {
        StartCoroutine(Fade());
    }
    private IEnumerator Fade()
    {
        yield return fadeEffect.UseFadeEffect(FadeState.FadeOut);
        yield return fadeEffect.UseFadeEffect(FadeState.FadeIn);
        fadeEffect.OffFadeObject();
    }
}
