using System.Collections;
using UnityEngine;

public class PlayerBedObject : InteractableObject
{
    public ScriptSO scriptSO;

    public override void ActivateInteraction()
    {
        if (!EventManager.Instance.GetSwitch(GameSwitch.GoToBed)) return;
        
        GameManager.Instance.player.playerInteraction.SetActive(true);
        GameManager.Instance.player.interactableText.text = "잠들기";
    }
    public override void Interact()
    {
        if (!EventManager.Instance.GetSwitch(GameSwitch.GoToBed)) return;

        DialogueManager.Instance.itemScript.Init(scriptSO);
        DialogueManager.Instance.itemScript.Print();

        StartCoroutine(Sleep());
    }
    IEnumerator Sleep()
    {
        // 스크립트 종료되면 잠들기
        yield return new WaitUntil(() => !DialogueSetting.isTalking);

        yield return GameManager.Instance.fadeManager.FadeStart(FadeState.FadeOut);
        yield return GameManager.Instance.fadeManager.FadeStart(FadeState.FadeIn);
        EventManager.Instance.SetSwitch(GameSwitch.GoToBed, false);
        yield return null;
    }
}
