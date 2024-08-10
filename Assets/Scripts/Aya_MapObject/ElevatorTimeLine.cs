using UnityEngine;
using UnityEngine.Playables;

public class ElevatorTimeLine : MonoBehaviour
{
    public bool triggerOn; 
    public PlayableDirector timelineDirector;
    public GameObject timeLineObject;
    public GameObject UI;
    public Light playerLight;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggerOn)
        {
            triggerOn = true;
            GameManager.Instance.fadeManager.fadeComplete += ElevatorMovie;
            GameManager.Instance.fadeManager.FadeStart(FadeState.FadeOut);
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
