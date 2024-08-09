using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FirstTLTrigger : MonoBehaviour
{
    public PlayableDirector firstCutScene;
    public GameObject[] doors;
    public GameObject VCs;
    public GameObject monsters;

    public bool isEnd = false;

    private void Start()
    {
        if (firstCutScene != null)
        {
            firstCutScene.stopped += OnPlayableDirectorStopped;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("타임라인 시작" );
        VCs.SetActive(true);
        PlayTimeline();
    }

    private void PlayTimeline()
    {
        if (firstCutScene != null)
        {
            GameManager.Instance.PlayerStateMachine.Player.PlayerControllOnOff();
            firstCutScene.Play();
        }
    }

    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        VCs.SetActive(false);

        GameManager.Instance.PlayerStateMachine.Player.PlayerControllOnOff();

        // 코루틴 대기 후 진행

        if (doors != null)
        {
            doors[0].SetActive(false);
            doors[1].SetActive(true);
        }

        monsters.SetActive(true);
        isEnd = true;
        //Debug.Log("타임라인이 종료");
    }
}
