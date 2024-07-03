using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    // 임시 싱글톤 
    private static DialogueManager Instance;
    private Dialogue dialogue;

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
        dialogue.Init();
        dialogue.StartDialogue();
    }

    // TODO: 다이얼로그 초기화, 다이얼로그 활성화/비활성화
}
