using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    // 임시 싱글톤 
    private static DialogueManager Instance;
    private Dialogue dialogue;
    private SystemMsg systemMsg;


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

        dialogue.Init();
        systemMsg.Init();
        systemMsg.UpdateMessage();

        Debug.Log("E 키를 눌러서 대화를 시작하세요");
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            Debug.Log("NPC와 대화 시작");
            dialogue.StartDialogue();
        }
    }
}
