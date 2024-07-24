using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBedObject : InteractableObject
{
    public override void ActivateInteraction()
    {
        if (isInteractable) return;
        GameManager.Instance.player.playerInteraction.SetActive(true);
        GameManager.Instance.player.interactableText.text = "침대에서 자기";
    }
    public override void Interact()
    {
        isInteractable = true;
        //InitNPCData(npcSO);

        //DialogueManager.Instance.dialogue.StartDialogue();
        StartCoroutine(Sleep());
        
    }
    IEnumerator Sleep()
    {
        GameManager.Instance.fadeManager.FadeStart(FadeState.FadeOut);
        GameManager.Instance.dayNightUI.TimeUpdate();

        yield return null;
    }
}
