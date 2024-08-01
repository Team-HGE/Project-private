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
    public Answer answer;

    private SystemMsg systemMsg;
    private Quest quest;

    public bool isSceneChanged;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        //카르마 초기화 추후 게임매니저나 다른 곳으로 옮길 것
        GameManager.Instance.PlayerStateMachine.Player.Karma = 0;

        set = GetComponent<DialogueSetting>();
        set.InitUI();
        set.InitDialogueSetting();

        storyScript = GetComponent<StoryScript>();
        npcScript = GetComponent<NPCScript>();
        itemScript = GetComponent<ItemScript>();
        answer = GetComponent<Answer>();

        systemMsg = GetComponent<SystemMsg>();
        quest = GetComponent<Quest>();

        systemMsg.Init();
        answer.Init();

        quest.UpdateQuest();
        systemMsg.UpdateMessage();

        storyScript.Print();
    }

    public void ChangeSO()
    {
        if (isSceneChanged)
        {
            //TODO: 씬이 바뀌면 SO를 바꿔주는 함수
        }
    }

    public void FinishStory()
    {
        set.InitDialogueSetting();
    }
}