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
        if (!isMoved && Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("Ʃ�丮��: WASD�� �̵��ϱ� �Ϸ�");
            isMoved = true;
        }
        else if (!isInteracted && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Ʃ�丮��: E�� ��ȣ�ۿ��ϱ� �Ϸ�");
            isInteracted = true;
        }
        else if (!isRan && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            Debug.Log("Ʃ�丮��: Shift�� �޸��� �Ϸ�");
            isRan = true;
        }
        else if (!isCrouched && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            Debug.Log("Ʃ�丮��: Ctrl�� ��ũ���� �Ϸ�");
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
