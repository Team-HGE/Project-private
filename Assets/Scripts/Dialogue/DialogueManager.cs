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
        set = GetComponent<DialogueSetting>();
        storyScript = GetComponent<StoryScript>();
        npcScript = GetComponent<NPCScript>();
        itemScript = GetComponent<ItemScript>();
        answer = GetComponent<Answer>();

        systemMsg = GetComponent<SystemMsg>();
        quest = GetComponent<Quest>();

        set.InitUI();
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
}