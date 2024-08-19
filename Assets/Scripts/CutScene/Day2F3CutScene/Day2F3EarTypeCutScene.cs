using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Day2F3EarTypeCutScene : MonoBehaviour
{
    public GameObject cutSceneObject;
    private ICutSceneEvent _trigger;
    public GameObject SM;


    [field: Header("TimeLine")]
    public PlayableDirector cutScene;
    public GameObject TLmonster;
    public GameObject VC;

    private void Start()
    {
        if (cutSceneObject != null)
        {
            _trigger = cutSceneObject.gameObject.GetComponent<ICutSceneEvent>();
            _trigger.OnEvent += PlayCutScene;
        }
        else Debug.LogError("Day2F3EarTypeCutScene - cutSceneObject가 없습니다.");

        if (cutScene != null)
        {
            cutScene.stopped += OnPlayableDirectorStopped;
        }
        else Debug.LogError("Day2F3EarTypeCutScene - 타임라인이 없습니다.");
    }

    private void PlayCutScene()
    {
        Debug.Log("플레이 컷신");

        if (cutScene != null)
        {
            VC.SetActive(true);
            TLmonster.SetActive(true);

            GameManager.Instance.NowPlayCutScene = true;
            GameManager.Instance.PlayerStateMachine.Player.VCOnOff();
            GameManager.Instance.PlayerStateMachine.Player.PlayerControllOnOff();
            // 다이얼로그 가리기
            DialogueManager.Instance.quest.questCanvas.SetActive(false);
            cutScene.Play();
        }
        else Debug.LogError("Day2F3EarTypeCutScene - 타임라인이 없습니다.");
    }

    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        VC.SetActive(false);
        GameManager.Instance.NowPlayCutScene = false;
        GameManager.Instance.PlayerStateMachine.Player.VCOnOff();
        GameManager.Instance.PlayerStateMachine.Player.PlayerControllOnOff();
        TLmonster.SetActive(false);
        SM.SetActive(true);
    }
}
