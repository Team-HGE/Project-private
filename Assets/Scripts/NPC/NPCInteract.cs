
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

        Debug.Log($"{nowInteractNPC} �� ��ȭ ��");

        DialogueManager.Instance.dialogue.StartDialogue();
    }

    // NPC ���� ����
    // NPC ǥ�� ����
}