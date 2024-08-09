using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FirstTLTrigger : MonoBehaviour
{
    public event Action OnEnd;

    public PlayableDirector firstCutScene;
    public GameObject[] doors;
    public GameObject VCs;
    public GameObject monsters;

    private bool _isEnd = false;


    public bool IsEnd 
    {
        get { return _isEnd; }
        set 
        {
            if (_isEnd != value)
            {
                _isEnd = value;
                OnEnd?.Invoke();
            }
            else _isEnd = value;
        }
    }

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

        StartCoroutine(WaitTime(1f));        
    }

    private IEnumerator WaitTime(float time)
    {
        yield return new WaitForSeconds(time);

        if (doors != null)
        {
            doors[0].SetActive(false);
            doors[1].SetActive(true);
        }

        monsters.SetActive(true);
        IsEnd = true;
        //Debug.Log("타임라인이 종료");
    }
}
