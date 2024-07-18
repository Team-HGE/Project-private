using UnityEngine;

public class Script: Dialogue
{
    public DialogueSO dialogueSO;

    public void Init()
    {
        CloseDialogue();
    }

    public void StartScript()
    {
        if (nowTalking) return;
        OpenDialogue();
        StartCoroutine(PrintDialogue());
    }

}