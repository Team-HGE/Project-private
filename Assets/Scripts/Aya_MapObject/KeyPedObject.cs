using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KeyPadObject : InteractableObject
{
    [Header("Cs")]
    [SerializeField] LockDoorObject lockDoorObject;
    [SerializeField] KeyPadGimmick keyPadGimmick;

    [Header("TriggerObject")]
    [SerializeField] GameObject KeyPadDecal;
    [SerializeField] GameObject keyPadGimmickCanvas;

    [Header("Gimmick")]
    [SerializeField] int[] passwords;
    [SerializeField] bool unLock;

    [Header("VCAM")]
    [SerializeField] CinemachineVirtualCamera keyPadCam;
    public override void ActivateInteraction()
    {
        if (isInteractable) return;
        if (unLock) return;
        GameManager.Instance.player.playerInteraction.SetActive(true);
        GameManager.Instance.player.interactableText.text = "�����ϱ�";
    }
    public override void Interact()
    {
        if (isInteractable) return;
        if (unLock) return;
        isInteractable = true;
        StartCoroutine(Init());
        keyPadGimmick.puzzleSetting(passwords, this);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    IEnumerator Init()
    {
        yield return StartCoroutine(GameManager.Instance.cinemachineManager.LookTarget(keyPadCam));
        
        keyPadGimmickCanvas.SetActive(true);
    }
    public void GimmickSuccess()
    {
        unLock = true;
        lockDoorObject.onInteract = true;
        KeyPadDecal.SetActive(false);
        keyPadGimmickCanvas.SetActive(false);
        StartCoroutine(Success());
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    IEnumerator Success()
    {
        yield return StartCoroutine(GameManager.Instance.cinemachineManager.ReturnToMainCamera());
    }
}
