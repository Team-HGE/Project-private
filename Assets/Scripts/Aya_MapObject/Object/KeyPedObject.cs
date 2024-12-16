using Cinemachine;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class KeyPadObject : InteractableObject
{
    [Title("Cs")]
    [SerializeField] LockDoorObject lockDoorObject;
    [SerializeField] KeyPadGimmick keyPadGimmick;

    [Title("TriggerObject")]
    [SerializeField] GameObject KeyPadDecal;
    [SerializeField] GameObject keyPadGimmickCanvas;

    [Title("Gimmick")]
    [SerializeField] int[] passwords;
    [SerializeField] bool unLock;

    [Title("VCAM")]
    [SerializeField] CinemachineVirtualCamera keyPadCam;

    [Title("SecondDayEvent")]
    public ScriptSO scriptSO;
    public bool isScondDayEvent = false;

    [Title("Glare")]
    public GameObject glare;

    public override void ActivateInteraction()
    {
        if (isInteractable) return;
        if (unLock) return;
        GameManager.Instance.player.playerInteraction.SetActive(true);
        GameManager.Instance.player.interactableText.text = "해제하기";
    }

    private void Start()
    {
        if (keyPadGimmickCanvas == null) keyPadGimmickCanvas = GameManager.Instance.keyPadGimmickCanvas;
        if (keyPadGimmick == null) keyPadGimmick = GameManager.Instance.keyPadGimmick;  
    }
    public override void Interact()
    {
        if (isInteractable) return;
        if (unLock) return;
        //isInteractable = true;
        StartCoroutine(Init());
        keyPadGimmick.puzzleSetting(passwords, this);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameManager.Instance.PlayerStateMachine.Player.PlayerControllOff();
    }

    IEnumerator Init()
    {
        yield return StartCoroutine(GameManager.Instance.cinemachineManager.LookTarget(keyPadCam));
        
        keyPadGimmickCanvas.SetActive(true);
    }
    public void GimmickSuccess()
    {
        glare.SetActive(false);
        isInteractable = true;
        unLock = true;
        lockDoorObject.onInteract = true;
        KeyPadDecal.SetActive(false);
        keyPadGimmickCanvas.SetActive(false);
        StartCoroutine(ReturnToMainCamera());
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameManager.Instance.PlayerStateMachine.Player.PlayerControllOn();
    }
    IEnumerator ReturnToMainCamera()
    {
        yield return StartCoroutine(GameManager.Instance.cinemachineManager.ReturnToMainCamera());
    }

    public void CloseKeyPad()
    {
        if (isScondDayEvent && !EventManager.Instance.GetSwitch(GameSwitch.Day_2_A2F_LeverOn) && EventManager.Instance.GetSwitch(GameSwitch.NowDay2))
        {
            GameManager.Instance.cinemachineManager.endEvent += SecondDayEventScript;
        }

        KeyPadDecal.SetActive(true);
        keyPadGimmickCanvas.SetActive(false);
        StartCoroutine(ReturnToMainCamera());
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameManager.Instance.PlayerStateMachine.Player.PlayerControllOn();
    }

    void SecondDayEventScript() 
    {
        isScondDayEvent = false;
        DialogueManager.Instance.itemScript.Init(scriptSO);
        DialogueManager.Instance.itemScript.Print();
    }
}
