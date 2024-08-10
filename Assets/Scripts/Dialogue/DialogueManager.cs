using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : SingletonManager<DialogueManager>
{
    [HideInInspector]
    public DialogueSetting set;
    [HideInInspector]
    public StoryScript storyScript;
    [HideInInspector]
    public NPCScript npcScript;
    [HideInInspector]
    public ItemScript itemScript;
    [HideInInspector]
    public KarmaScript karmaScript;
    [HideInInspector]
    public Answer answer;

    //public bool isSceneChanged;
    public List<ScriptSO> storyList = new List<ScriptSO>();
    public List<AnswerSO> answerList = new List<AnswerSO>();
    //private int scriptIndex;

    public SystemMsg systemMsg;
    private Quest quest;


    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        //카르마 초기화 추후 게임매니저나 다른 곳으로 옮길 것
        //GameManager.Instance.PlayerStateMachine.Player.Karma = 0f;
        Debug.Log("현재 카르마 수치: " + GameManager.Instance.PlayerStateMachine.Player.Karma);
        
        set = GetComponent<DialogueSetting>();
        set.InitUI();
        set.InitDialogueSetting();

        storyScript = GetComponent<StoryScript>();
        npcScript = GetComponent<NPCScript>();
        itemScript = GetComponent<ItemScript>();
        karmaScript = GetComponent<KarmaScript>();

        answer = GetComponent<Answer>();
        systemMsg = GetComponent<SystemMsg>();
        quest = GetComponent<Quest>();

        systemMsg.Init();
        //answer.Init();

        quest.UpdateQuest();

        systemMsg.UpdateMessage(0);
    }

    //씬이 바뀌면 새 스토리를 재생하고 선택지 초기화
    //storyIdx 0번: 인트로
    // 1번: 1일차 낮 시작시
    // 2번: 1일차 밤 시작시
    // 3번: 1일차 밤 통로 진입시
    public void StartStory(int storyIdx)
    {
        storyScript.Init(storyList[storyIdx]);
        if (answerList[storyIdx] != null)
            answer.InitAnswer(answerList[storyIdx]);
        else Debug.Log("이 파트엔 선택지가 없습니다. answerList[storyIdx] null");
        storyScript.Print();
        //scriptIndex++;
    }

    public void FinishStory()
    {
        set.InitDialogueSetting();
        Debug.Log("스토리 스크립트 초기화");
        storyScript.scriptSO = null;
        StopAllCoroutines();
        GameManager.Instance.PlayerStateMachine.Player.PlayerControllOnOff();
    }

    public void NpcStartInteract()
    {
        // NPC 대화 기회 초기화
        //isInteracted = false;
    }
}