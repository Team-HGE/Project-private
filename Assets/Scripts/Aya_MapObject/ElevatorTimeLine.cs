using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class ElevatorTimeLine : MonoBehaviour
{
    public PlayableDirector timelineDirector;
    public GameObject timeLineObject;
    public GameObject UI;
    public Light playerLight;

    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.fadeManager.fadeComplete += ElevatorMovie;
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.fadeManager.FadeStart(FadeState.FadeIn);
            
        }
    }

    private void ElevatorMovie()
    {
        timeLineObject.SetActive(true);
        playerLight.enabled = false;
        UI.SetActive(false);
        timelineDirector.Play();
    }
}
