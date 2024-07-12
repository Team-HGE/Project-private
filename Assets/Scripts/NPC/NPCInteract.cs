
using JetBrains.Annotations;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;


public class NPCInteract : InteractableObject
{
    public bool readyToTalk = false;

    private GameObject nowInteractNPC;

    public override void ActivateInteraction()
    {
        if (isInteractable) return;

        nowInteractNPC = GameManager.Instance.player.curInteractableGameObject;

        GameManager.Instance.player.playerInteraction.SetActive(true);
        GameManager.Instance.player.interactableText.text = "[E] Talk";

    }

    public override void Interact()
    {

        readyToTalk = true;
        // readyToTalk = false;

        Debug.Log($"{nowInteractNPC} 와 대화 중");

        DialogueManager.Instance.dialogue.StartDialogue();
    }

    // NPC 상태 제어
    // NPC 표정 제어
}