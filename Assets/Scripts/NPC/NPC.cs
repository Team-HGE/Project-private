
using UnityEngine;

public class NPC : InteractableObject
{
    public NPC_SO npcSO;

    private void InitNPCData(NPC_SO _npc)
    {
        npcSO = _npc;
    }

    public override void ActivateInteraction()
    {
        if (isInteractable) return;

        GameManager.Instance.player.playerInteraction.SetActive(true);
        GameManager.Instance.player.interactableText.text = "��ȭ�ϱ�";

    }

    public override void Interact()
    {
        //ChangeState(npcState.Speaking);

        InitNPCData(npcSO);

        DialogueManager.Instance.dialogue.StartDialogue();
    }

    // NPC ǥ�� �ٲٱ�
    public void SwitchEmotion(Emotion emotion)
    {
        switch(emotion)
        {
            case Emotion.Default:
                npcSO.emotion = Emotion.Default;
                return;
            case Emotion.Embarrassed:
                npcSO.emotion = Emotion.Embarrassed;
                return;
            case Emotion.Rage:
                npcSO.emotion = Emotion.Rage;
                return;
            default:
                npcSO.emotion = Emotion.Default;
                return;
        }
    }

    public Sprite SwitchPortrait(Emotion emotion)
    {
        switch (emotion)
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

    public string ChangeNpcState(NpcState stateType)
    {
        switch (stateType)
        {
            case NpcState.Idle:
                npcSO.state = NpcState.Idle;
                return "�����";
            case NpcState.Speaking:
                npcSO.state = NpcState.Speaking;
                return "��ȭ��";
            case NpcState.Calling:
                npcSO.state = NpcState.Calling;
                return "��ȭ��";
            default:
                npcSO.state = NpcState.Idle;
                return "�����";
        }
    }
}