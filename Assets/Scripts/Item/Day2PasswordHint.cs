using UnityEngine;

public class Day2PasswordHint : InteractableObject
{
    [Header("SecondDayEvent")]
    public ScriptSO scriptSO;

    public override void ActivateInteraction()
    {
        if (isInteractable || !EventManager.Instance.GetSwitch(GameSwitch.NowDay2)) return;

        GameManager.Instance.player.playerInteraction.SetActive(true);
        GameManager.Instance.player.interactableText.text = "줍기";
    }

    public override void Interact()
    {
        if (isInteractable) return;
        isInteractable = true;

        HotelFloorScene_DataManager.Instance.controller.ComputerPassward += 1;
        DialogueManager.Instance.itemScript.Init(scriptSO);
        DialogueManager.Instance.itemScript.Print();

        Destroy(gameObject);
    }
}
