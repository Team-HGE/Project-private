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

    public Quest quest; // �ǿ� ����

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
        if (!isMoved && GameManager.Instance.PlayerStateMachine.IsWalking) // null ���� ��
        {
            Debug.Log("Ʃ�丮��: WASD�� �̵��ϱ� �Ϸ�");
            quest.NextQuest(0);
            isMoved = true;
        }
        else if (!isInteracted && GameManager.Instance.player.tutorialSuccess)
        {
            Debug.Log("Ʃ�丮��: E�� ��ȣ�ۿ��ϱ� �Ϸ�");
            quest.NextQuest(1);
            isInteracted = true;
        }
        else if (!isRan && GameManager.Instance.PlayerStateMachine.IsRunning)
        {
            Debug.Log("Ʃ�丮��: Shift�� �޸��� �Ϸ�");
            quest.NextQuest(2);
            isRan = true;
        }
        else if (!isCrouched && GameManager.Instance.PlayerStateMachine.IsCrouch)
        {
            Debug.Log("Ʃ�丮��: Ctrl�� ��ũ���� �Ϸ�");
            quest.NextQuest(3);
            isCrouched = true;
        }


        // �÷��̾� ���� Ȯ�ο�(������ ��)** 
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("�÷��̾� OnOff");
            GameManager.Instance.PlayerStateMachine.Player.PlayerControllOnOff();
        }
    }
}
