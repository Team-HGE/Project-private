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
        GameManager.Instance.player.interactableText.text = "마주하기";
    }
    public override void Interact()
    {
        DialogueManager.Instance.karmaScript.Init(scriptSO);
        DialogueManager.Instance.karmaScript.Print();
    }
}
