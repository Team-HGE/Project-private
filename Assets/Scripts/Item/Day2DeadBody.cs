using UnityEngine;

public class Day2DeadBody : InteractableObject
{
    [Header("SecondDayEvent")]
    public ScriptSO scriptSO;

    public override void ActivateInteraction()
    {
        if (isInteractable || EventManager.Instance.GetSwitch(GameSwitch.Day2GetCardKey)) return;        

        GameManager.Instance.player.playerInteraction.SetActive(true);
        GameManager.Instance.player.interactableText.text = "시체 뒤지기";
    }

    public override void Interact()
    {
        if (isInteractable) return;
        isInteractable = true;
        EventManager.Instance.SetSwitch(GameSwitch.Day2GetCardKey, true);

        
        DialogueManager.Instance.itemScript.Init(scriptSO);
        DialogueManager.Instance.itemScript.Print();
    }
}
