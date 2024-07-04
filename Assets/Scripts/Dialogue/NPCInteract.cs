
using JetBrains.Annotations;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class NPCInteract : InteractableObject
{
    public bool readyToTalk = false;

    public override void ActivateInteraction()
    {
        if (isInteractable) return;

        Debug.Log("오브젝트 감지");

        GameManager.Instance.player.playerInteraction.SetActive(true);
        GameManager.Instance.player.interactableText.text = "[E] Talk";

    }

    public override void Interact()
    {
        // TODO: 대화할 동안 카메라 NPC에게 고정

        readyToTalk = true;

        DialogueManager.Instance.dialogue.StartDialogue();

        Debug.Log($"readyToTalk: {readyToTalk}");
    }
}