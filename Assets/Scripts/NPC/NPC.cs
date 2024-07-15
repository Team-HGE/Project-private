
using JetBrains.Annotations;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class NPC : InteractableObject
{
    public NPC_SO npcSO;

    //public string npcName { get; private set; }
    //public npcState npcState { get; private set; }
    //public Emotion emotion { get; private set; }

    //public NPC(string npcName, npcState npcState, Emotion emotion)
    //{
    //    this.npcName = npcName;
    //    this.npcState = npcState;
    //    this.emotion = emotion;
    //}

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
        ChangeState(npcState.Speaking);

        InitNPCData(npcSO);

        Debug.Log($"{npcSO.npcName} �� ��ȭ ��");

        DialogueManager.Instance.dialogue.StartDialogue();
    }

    // NPC ǥ�� �ٲٱ�
    public Sprite ChangeEmotion(Emotion emotion)
    {
        switch(emotion)
        {
            case Emotion.Default:
                return npcSO.illusts[0];
            case Emotion.Embarrassed:
                return npcSO.illusts[1];
            case Emotion.Rage:
                return npcSO.illusts[2];
            default:
                return npcSO.illusts[0];
        }
    }

    // NPC ���� ����
    // ��ȭ��, ��ȭ��, ����, ���

    public string ChangeState(npcState stateType)
    {
        switch (stateType)
        {
            case npcState.Idle:
                npcSO.state = npcState.Idle;
                return "�����";
            case npcState.Speaking:
                npcSO.state = npcState.Speaking;
                return "��ȭ��";
            case npcState.Calling:
                npcSO.state = npcState.Calling;
                return "��ȭ��";
            default:
                npcSO.state = npcState.Idle;
                return "�����";
        }
    }
}