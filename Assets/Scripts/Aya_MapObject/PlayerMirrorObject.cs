using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMirrorObject : InteractableObject
{
    public ScriptSO scriptSO;

    public override void ActivateInteraction()
    {
        if (isInteractable) return;
        GameManager.Instance.player.playerInteraction.SetActive(true);
        GameManager.Instance.player.interactableText.text = "상태 확인";
    }
    public override void Interact()
    {
        isInteractable = true;

        DialogueManager.Instance.itemScript.Init(scriptSO);
        DialogueManager.Instance.itemScript.Print();

        //StartCoroutine(Sleep());
    }
    IEnumerator Sleep()
    {
        GameManager.Instance.fadeManager.FadeStart(FadeState.FadeOut);
        GameManager.Instance.dayNightUI.TimeUpdate();

        yield return null;
    }
}
