using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day2ControlRoomComputer : InteractableObject
{

    [Header("SecondDayEvent")]
    public ScriptSO[] scriptSOs;

    public override void ActivateInteraction()
    {
        GameManager.Instance.player.playerInteraction.SetActive(true);
        GameManager.Instance.player.interactableText.text = "컴퓨터 사용하기";
    }

    public override void Interact()
    {
        if (EventManager.Instance.GetSwitch(GameSwitch.Day2GetPasswordHint))
        {
            DialogueManager.Instance.itemScript.Init(scriptSOs[1]);
            DialogueManager.Instance.itemScript.Print();
        }
        else
        {
            DialogueManager.Instance.itemScript.Init(scriptSOs[0]);
            DialogueManager.Instance.itemScript.Print();
        }
    }
}
