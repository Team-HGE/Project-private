
using JetBrains.Annotations;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;


public class NPCInteract : InteractableObject
{
    public bool readyToTalk = false;

    public override void ActivateInteraction()
    {
        if (isInteractable) return;

        Debug.Log("������Ʈ ����");

        GameManager.Instance.player.playerInteraction.SetActive(true);
        GameManager.Instance.player.interactableText.text = "[E] Talk";

    }

    public override void Interact()
    {

        readyToTalk = true;
        // readyToTalk = false;

        DialogueManager.Instance.dialogue.StartDialogue();

        //Debug.Log($"readyToTalk: {readyToTalk}");
    }

    // NPC ���� ����
    // NPC ǥ�� ����
}