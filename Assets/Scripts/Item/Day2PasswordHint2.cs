using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day2PasswordHint2 : InteractableObject
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
        EventManager.Instance.SetSwitch(GameSwitch.Day2GetPasswordHint2, true);

        DialogueManager.Instance.itemScript.Init(scriptSO);
        DialogueManager.Instance.itemScript.Print();

        Destroy(gameObject);
    }
}
