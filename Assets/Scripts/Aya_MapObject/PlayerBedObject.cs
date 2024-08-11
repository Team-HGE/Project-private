using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBedObject : InteractableObject
{
    public ScriptSO scriptSO;

    public override void ActivateInteraction()
    {
        if (isInteractable) return;
        GameManager.Instance.player.playerInteraction.SetActive(true);
        GameManager.Instance.player.interactableText.text = "잠들기";
    }
    public override void Interact()
    {
        isInteractable = true;

        DialogueManager.Instance.itemScript.Init(scriptSO);
        DialogueManager.Instance.itemScript.Print();

        StartCoroutine(Sleep());
    }
    IEnumerator Sleep()
    {
        // 스크립트 종료되면 잠들기
        yield return new WaitUntil(() => !DialogueSetting.isTalking);

        GameManager.Instance.fadeManager.FadeStart(FadeState.FadeOut);
        

        yield return null;
    }
}
