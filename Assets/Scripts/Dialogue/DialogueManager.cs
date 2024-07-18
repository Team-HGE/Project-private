﻿using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    // 임시 싱글톤 
    public static DialogueManager Instance;

    public Dialogue dialogue;
    public Script script;

    private SystemMsg systemMsg;
    private Quest quest;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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
