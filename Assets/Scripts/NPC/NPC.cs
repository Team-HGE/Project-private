
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

    // NPC 정보 불러오기
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

        Debug.Log($"{npcSO.npcName} 와 대화 중");

        DialogueManager.Instance.dialogue.StartDialogue();
    }

    // NPC 표정 바꾸기
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

    // NPC 상태 제어
    // 대화중, 통화중, 변이, 사망

    public string ChangeState(npcState stateType)
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