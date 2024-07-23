public class Item : InteractableObject
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
        GameManager.Instance.player.interactableText.text = "È¹µæ";

    }

    public override void Interact()
    {
        InitNPCData(npcSO);

        DialogueManager.Instance.dialogue.StartDialogue();
    }
}