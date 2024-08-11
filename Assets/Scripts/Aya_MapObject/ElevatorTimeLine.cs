using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class ElevatorTimeLine : MonoBehaviour
{
    public bool triggerOn; 
    public PlayableDirector timelineDirector;
    public GameObject timeLineObject;
    public GameObject UI;
    public Light playerLight;

    public TextMeshPro roomTxt101;
    public TextMeshPro roomTxt102;
    public TextMeshPro roomTxt101_111;
    public TextMeshPro roomTxt112_121;

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

    public void OffLight()
    {
        GameManager.Instance.lightManager.OffLaversAllLight();
    }

    public void ChangeTxt()
    {
        roomTxt101.text = "2";
        roomTxt102.text = "2";
        roomTxt101_111.text = "201 - 211";
        roomTxt112_121.text = "212 - 221";
    }
}
