
using JetBrains.Annotations;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class NPC : InteractableObject
{
    public NPC_SO npcSO;

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
        ChangeState(NPCStateType.Speaking);

        InitNPCData(npcSO);

        Debug.Log($"{npcSO.npcName} 와 대화 중");

        DialogueManager.Instance.dialogue.StartDialogue();
    }

    // NPC 표정 바꾸기
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

    // NPC 상태 제어
    // 대화중, 통화중, 변이, 사망

    public string ChangeState(NPCStateType stateType)
    {
        switch (stateType)
        {
            case NPCStateType.Idle:
                npcSO.stateType = NPCStateType.Idle;
                return "대기중";
            case NPCStateType.Speaking:
                npcSO.stateType = NPCStateType.Speaking;
                return "대화중";
            case NPCStateType.Calling:
                npcSO.stateType = NPCStateType.Calling;
                return "통화중";
            default:
                npcSO.stateType = NPCStateType.Idle;
                return "대기중";
        }
    }
}