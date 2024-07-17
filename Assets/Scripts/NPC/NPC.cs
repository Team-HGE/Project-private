
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
        GameManager.Instance.player.interactableText.text = "대화하기";

    }

    public override void Interact()
    {
        //ChangeState(npcState.Speaking);

        InitNPCData(npcSO);

        DialogueManager.Instance.dialogue.StartDialogue();
    }

    // NPC 표정 바꾸기
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

    // NPC 상태 제어
    // 대화중, 통화중, 변이, 사망

    public string ChangeNpcState(npcState stateType)
    {
        switch (stateType)
        {
            case npcState.Idle:
                npcSO.state = npcState.Idle;
                return "대기중";
            case npcState.Speaking:
                npcSO.state = npcState.Speaking;
                return "대화중";
            case npcState.Calling:
                npcSO.state = npcState.Calling;
                return "통화중";
            default:
                npcSO.state = npcState.Idle;
                return "대기중";
        }
    }
}