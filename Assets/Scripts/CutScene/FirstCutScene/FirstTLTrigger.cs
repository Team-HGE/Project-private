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
    public GameObject SM1;

    private bool _isEnd = false;
    private bool _onTrigger = false;


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
        if (_onTrigger) return;

        //Debug.Log("타임라인 시작" );
        _onTrigger = true;
        GameManager.Instance.nowPlayCutScene = true;
        PlayTimeline();
    }

    private void PlayTimeline()
    {
        if (firstCutScene != null)
        {
            GameManager.Instance.PlayerStateMachine.Player.VCOnOff();
            GameManager.Instance.PlayerStateMachine.Player.PlayerControllOnOff();
            VCs.SetActive(true);
            GameManager.Instance.jumpScareManager.playerCanvas.SetActive(false);
            DialogueManager.Instance.quest.questCanvas.SetActive(false);
            firstCutScene.Play();
        }
    }

    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        VCs.SetActive(false);
        GameManager.Instance.nowPlayCutScene = false;
        GameManager.Instance.PlayerStateMachine.Player.VCOnOff();
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

        SM1.SetActive(true);
        IsEnd = true;
        //Debug.Log("타임라인이 종료");
    }

    private void OnDestroy()
    {
        firstCutScene.stopped -= OnPlayableDirectorStopped;
    }
}
