using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    private bool isMoved = false;

    [SerializeField]
    private bool isInteracted = false;

    [SerializeField]
    private bool isRan = false;

    [SerializeField]
    private bool isCrouched = false;

    private bool nowChecking = false;


    public void Start()
    {
        Init();
    }

    public void Update()
    {
        CheckTutorial();

        if(isMoved && isInteracted && isRan && isCrouched)
        {
            Debug.Log("Ʃ�丮���� ��� �Ϸ��߽��ϴ�.");
            Init();
            Destroy(gameObject);
        }
    }

    public void Init()
    {
        isMoved = false;
        isInteracted = false;
        isRan = false;
        isCrouched = false;
    }

    public void CheckTutorial()
    {
        if (nowChecking) return;
        nowChecking = true;

        if (!isMoved && Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("Ʃ�丮��: WASD�� �̵��ϱ� �Ϸ�");
            isMoved = true;
            DialogueManager.Instance.quest.UpdateQuest();
        }
        else if (!isInteracted && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Ʃ�丮��: E�� ��ȣ�ۿ��ϱ� �Ϸ�");
            isInteracted = true;
            DialogueManager.Instance.quest.UpdateQuest();
        }
        else if (!isRan && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            Debug.Log("Ʃ�丮��: Shift�� �޸��� �Ϸ�");
            isRan = true;
            DialogueManager.Instance.quest.UpdateQuest();
        }
        else if (!isCrouched && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            Debug.Log("Ʃ�丮��: Ctrl�� ��ũ���� �Ϸ�");
            isCrouched = true;
            DialogueManager.Instance.quest.UpdateQuest();
        }

        nowChecking = false;
    }
}
