using UnityEngine;

public class DialogueManager : SingletonManager<DialogueManager>
{
    public Dialogue dialogue;

    public Script script;

    private SystemMsg systemMsg;
    private Quest quest;

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

        dialogue.Init();
        systemMsg.Init();

        quest.UpdateQuest();
        systemMsg.UpdateMessage();

        script.StartScript();
    }
}
