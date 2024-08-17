using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
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
    
    public Quest quest; // 권용 수정
    public SystemMsg systemMsg;

    public void Start()
    {
        Init();
    }


    public void Init()
    {
        isMoved = false;
        isInteracted = false;
        isRan = false;
        isCrouched = false;
    }

    public void Update()
    {
        CheckTutorial();
    }

    public void CheckTutorial()
    {
        
        if (!isMoved && GameManager.Instance.PlayerStateMachine.IsWalking) // null 에러 남
        {
            Debug.Log("튜토리얼: WASD로 이동해보기");
            systemMsg.UpdateMessage(4);
            quest.NextQuest(1);

            isMoved = true;
        }
        else if (!isInteracted && GameManager.Instance.player.tutorialSuccess && isMoved)
        {
            Debug.Log("튜토리얼: E로 상호작용해보기 ");
            quest.NextQuest(2);
            systemMsg.UpdateMessage(5);
            isInteracted = true;
        }
        else if (EventManager.Instance.GetSwitch(GameSwitch.BarrierIsOpen) && GameManager.Instance.PlayerStateMachine.IsCrouch && isInteracted )
        {
            Debug.Log("튜토리얼: Ctrl로 웅크려보기 완료");
            quest.NextQuest(4);
            isCrouched = true;
            this.enabled = false;
        }


        
    }
}
