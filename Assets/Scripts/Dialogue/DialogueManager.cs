using UnityEngine;

public class DialogueManager : SingletonManager<DialogueManager>
{
    public Dialogue dialogue;

    public Script script;

    private SystemMsg systemMsg;
    private Quest quest;

    public Answer answer;

    public bool isSceneChanged;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {

        dialogue = GetComponent<Dialogue>();
        script = GetComponent<Script>();

        systemMsg = GetComponent<SystemMsg>();
        quest = GetComponent<Quest>();

        answer = GetComponent<Answer>();

        dialogue.Init();
        systemMsg.Init();
        answer.InitAnswer();

        quest.UpdateQuest();
        systemMsg.UpdateMessage();

        //script.StartScript();
    }


    public void ChangeSO()
    {
        if (isSceneChanged)
        {
            //TODO: 씬이 바뀌면 SO를 바꿔주는 함수
        }
    }
}
