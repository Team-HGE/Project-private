
using UnityEngine;

public class NPC : InteractableObject
{
    public NPC_SO npcSO;
    public ScriptSO scriptSO;

    private void Init(NPC_SO _npc)
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
        // NPC 스트레스 지수 감소

        Init(npcSO);
        DialogueManager.Instance.npcScript.Init(scriptSO);
        DialogueManager.Instance.npcScript.Print();
    }

    // NPC 표정 바꾸기: 추후 일러 추가되면 리팩토링 예정
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
                return npcSO.standings[0];
            case Emotion.Embarrassed:
                return npcSO.standings[1];
            case Emotion.Rage:
                return npcSO.standings[2];
            default:
                return npcSO.standings[0];
        }
    }

    // NPC 상태 제어
    // 대화중, 통화중, 변이, 사망

    public string ChangeNpcState(NpcState stateType)
    {
        switch (stateType)
        {
            case NpcState.Idle:
                npcSO.state = NpcState.Idle;
                return "대기중";
            case NpcState.Speaking:
                npcSO.state = NpcState.Speaking;
                return "대화중";
            case NpcState.Calling:
                npcSO.state = NpcState.Calling;
                return "통화중";
            default:
                npcSO.state = NpcState.Idle;
                return "대기중";
        }
    }
}