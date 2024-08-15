using DG.Tweening;
using UnityEngine;

public class IntroTimeLine : MonoBehaviour
{
    [SerializeField] DOTweenAnimation[] openAni;
    [SerializeField] DOTweenAnimation[] alramAni;
    [SerializeField] Light[] alisdfasd;

    public void OpenAnimation()
    {
        foreach (var animation in openAni)
        {
            animation.duration = 10;
            animation.CreateTween(true);
        }

        foreach (var animation in alramAni)
        {
            animation.CreateTween(true);
        }

        foreach (var animation in alisdfasd)
        {
            animation.enabled = true;
        }
    }
}
