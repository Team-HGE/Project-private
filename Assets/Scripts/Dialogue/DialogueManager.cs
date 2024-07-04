using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    // 임시 싱글톤 
    public static DialogueManager Instance;
    public Dialogue dialogue;
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
        systemMsg = GetComponent<SystemMsg>();
        quest = GetComponent<Quest>();

        dialogue.Init();
        systemMsg.Init();

        quest.UpdateQuest();
        systemMsg.UpdateMessage();

        Debug.Log("E 키를 눌러서 대화를 시작하세요");
    }

    private void Update()
    {
        //if (Input.GetKey(KeyCode.E))
        {
          //  dialogue.StartDialogue();
        }
    }
}
