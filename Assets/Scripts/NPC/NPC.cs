
public class NPC : InteractableObject
{
    public int ID;
    private NpcData data;

    public override void ActivateInteraction()
    {
        if (isInteractable) return;

        GameManager.Instance.player.playerInteraction.SetActive(true);
        GameManager.Instance.player.interactableText.text = "대화하기";
    }

    public override void Interact()
    {
        // ID로 npc 정보 불러오기
        data = DialogueManager.Instance.npcData;

        DialogueManager.Instance.npcScript.InitNPC(data, ID);
        DialogueManager.Instance.npcScript.Print();
    }
}