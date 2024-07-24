public class Bed : InteractableObject
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
        GameManager.Instance.player.interactableText.text = "¿·µÈ±‚";

    }

    public override void Interact()
    {
        InitNPCData(npcSO);

        DialogueManager.Instance.dialogue.StartDialogue();
    }
}