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

    public Quest quest; // 권용 수정

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
        if (!isMoved && GameManager.Instance.PlayerStateMachine.IsWalking) // null 에러 남
        {
            Debug.Log("튜토리얼: WASD로 이동하기 완료");
            quest.NextQuest(0);
            isMoved = true;
        }
        else if (!isInteracted && GameManager.Instance.player.tutorialSuccess)
        {
            Debug.Log("튜토리얼: E로 상호작용하기 완료");
            quest.NextQuest(1);
            isInteracted = true;
        }
        else if (!isRan && GameManager.Instance.PlayerStateMachine.IsRunning)
        {
            Debug.Log("튜토리얼: Shift로 달리기 완료");
            quest.NextQuest(2);
            isRan = true;
        }
        else if (!isCrouched && GameManager.Instance.PlayerStateMachine.IsCrouch)
        {
            Debug.Log("튜토리얼: Ctrl로 웅크리기 완료");
            quest.NextQuest(3);
            isCrouched = true;
        }


        // 플레이어 정지 확인용(지워도 됨)** 
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("플레이어 OnOff");
            GameManager.Instance.PlayerStateMachine.Player.PlayerControllOnOff();
        }
    }
}
