using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Day2F3EarTypeCutScene : MonoBehaviour
{
    private static Day2F3EarTypeCutScene _instance;

    public static Day2F3EarTypeCutScene Instance
    {
        get
        {
            _instance ??= FindObjectOfType<Day2F3EarTypeCutScene>();
            return _instance;
        }
    }

    public GameObject Trigger;
    public PlayableDirector firstCutScene;


    private void Awake()
    {
        _instance = this;
        
    }

    private void PlayTimeline()
    {
        if (firstCutScene != null)
        {
            //GameManager.Instance.PlayerStateMachine.Player.VCOnOff();
            //GameManager.Instance.PlayerStateMachine.Player.PlayerControllOnOff();
            //VCs.SetActive(true);
            //GameManager.Instance.jumpScareManager.playerCanvas.SetActive(false);
            //DialogueManager.Instance.quest.questCanvas.SetActive(false);
            //firstCutScene.Play();
        }
    }



}
