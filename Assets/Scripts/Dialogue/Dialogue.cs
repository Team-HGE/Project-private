using UnityEngine;
using System.Text;
using System.Collections;

public class Dialogue : MonoBehaviour
{
    public UIDialogue uiDialogue;

    private NPC npc;
    private NPC_SO npcSO;

    public StringBuilder sbTitle = new StringBuilder();
    public StringBuilder sbBody = new StringBuilder();

    public IEnumerator curPrintLine;
    public bool nowTalking = false;

    public void Init()
    {
        uiDialogue = GetComponent<UIDialogue>();
        uiDialogue.CloseDialogue();
    }

    public void StartDialogue()
    {
        if (nowTalking)
        {
            GameManager.Instance.player.playerInteraction.SetActive(false);
            return;
        }
            
        nowTalking = true;

        GameObject nowInteracting = GameManager.Instance.player.curInteractableGameObject;
        npc = nowInteracting.GetComponent<NPC>();
        npcSO = npc.npcSO;

        uiDialogue.OpenDialogue();
        StartCoroutine(PrintDialogue());
    }

    public IEnumerator PrintDialogue()
    {
        uiDialogue.titleText.text = sbTitle.Clear().ToString();
        uiDialogue.bodyText.text = sbBody.Clear().ToString();

        uiDialogue.portrait.sprite = null;

        for (int i = 0; i < npcSO.testDialogue.Length; i++)
        {
            UtilSB.SetText(uiDialogue.titleText, sbTitle, npcSO.objName + " - " + npc.ChangeNpcState(NpcState.Speaking));

            uiDialogue.SetImage(uiDialogue.portrait, npc.SwitchPortrait(npcSO.emotion));

            if (uiDialogue.portrait.sprite == null) uiDialogue.portrait.transform.localScale = Vector3.zero;
            else
            {
                uiDialogue.portrait.transform.localScale = Vector3.one;
            }

            curPrintLine = TextEffect.Typing(uiDialogue.bodyText, sbBody, npcSO.testDialogue[i]);
            yield return StartCoroutine(curPrintLine);

            //Debug.Log("E 키로 진행하세요");
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

            yield return new WaitForSeconds(1f);

            ClearDialogue();
        }

        uiDialogue.CloseDialogue();

        nowTalking = false;
        npc.ChangeNpcState(NpcState.Idle);

        yield return null;
    }

    public void ClearDialogue()
    {
        UtilSB.ClearText(uiDialogue.titleText, sbTitle);
        UtilSB.ClearText(uiDialogue.bodyText, sbBody);
        uiDialogue.portrait.sprite = null;
    }
}