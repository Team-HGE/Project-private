using UnityEngine;

public class DialogueManager : SingletonManager<DialogueManager>
{
    public Dialogue dialogue;

    public Script script;

    private SystemMsg systemMsg;
    private Quest quest;

    private Answer answer;


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

        script.StartScript();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            answer.StartAnswer();
        }
    }
}
