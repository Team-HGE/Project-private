using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class ElevatorTimeLine : MonoBehaviour
{
    public PlayableDirector timelineDirector;
    public GameObject timeLineObject;
    public GameObject UI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.fadeManager.FadeStart(FadeState.FadeIn);
            timeLineObject.SetActive(true);
            UI.SetActive(false);
            timelineDirector.Play();
        }
    }
}
