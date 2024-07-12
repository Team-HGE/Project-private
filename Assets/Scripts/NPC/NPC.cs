
using JetBrains.Annotations;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class NPC : InteractableObject
{
    public NPC_SO npcSO;

    // NPC ���� �ҷ�����
    private void InitNPCData(NPC_SO _npc)
    {
        npcSO = _npc;
    }

    public override void ActivateInteraction()
    {
        if (isInteractable) return;

        GameManager.Instance.player.playerInteraction.SetActive(true);
        GameManager.Instance.player.interactableText.text = "[E] Talk";

    }

    public override void Interact()
    {
        ChangeState(NPCStateType.Speaking);

        InitNPCData(npcSO);

        Debug.Log($"{npcSO.npcName} �� ��ȭ ��");

        DialogueManager.Instance.dialogue.StartDialogue();
    }

    // NPC ǥ�� �ٲٱ�
    public Sprite ChangeEmotion(NPCEmotion emotion)
    {
        switch(emotion)
        {
            case NPCEmotion.Default:
                return npcSO.emotions[0];
            case NPCEmotion.Embarrassed:
                return npcSO.emotions[1];
            case NPCEmotion.Rage:
                return npcSO.emotions[2];
            default:
                return npcSO.emotions[0];
        }
    }

    // NPC ���� ����
    // ��ȭ��, ��ȭ��, ����, ���

    public string ChangeState(NPCStateType stateType)
    {
        switch (stateType)
        {
            case NPCStateType.Idle:
                npcSO.stateType = NPCStateType.Idle;
                return "�����";
            case NPCStateType.Speaking:
                npcSO.stateType = NPCStateType.Speaking;
                return "��ȭ��";
            case NPCStateType.Calling:
                npcSO.stateType = NPCStateType.Calling;
                return "��ȭ��";
            default:
                npcSO.stateType = NPCStateType.Idle;
                return "�����";
        }
    }
}