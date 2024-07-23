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
            Debug.Log("튜토리얼을 모두 완료했습니다.");
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
            Debug.Log("튜토리얼: WASD로 이동하기 완료");
            isMoved = true;
            DialogueManager.Instance.quest.UpdateQuest();
        }
        else if (!isInteracted && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("튜토리얼: E로 상호작용하기 완료");
            isInteracted = true;
            DialogueManager.Instance.quest.UpdateQuest();
        }
        else if (!isRan && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            Debug.Log("튜토리얼: Shift로 달리기 완료");
            isRan = true;
            DialogueManager.Instance.quest.UpdateQuest();
        }
        else if (!isCrouched && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            Debug.Log("튜토리얼: Ctrl로 웅크리기 완료");
            isCrouched = true;
            DialogueManager.Instance.quest.UpdateQuest();
        }

        nowChecking = false;
    }
}
