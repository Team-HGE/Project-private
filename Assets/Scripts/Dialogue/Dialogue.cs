using UnityEngine;
using UnityEngine.UI;
using System.Text;
using TMPro;
using UnityEngine.Diagnostics;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;

public class Dialogue : MonoBehaviour
{
    //public DialogueSO dialogueSO;
    public NPC_SO npcSO;
    public NPC npc;

    public GameObject dialogueCanvas;
    public Image titleBG;

    public TextMeshProUGUI titleText;
    public TextMeshProUGUI bodyText;
    public Image portrait;

    private StringBuilder sbTitle = new StringBuilder();
    private StringBuilder sbBody = new StringBuilder();

    private IEnumerator curPrintLine;
    private bool nowTalking = false;

    public void Init()
    {
        CloseDialogue();
    }

    public void StartDialogue()
    {
        if (nowTalking) return;
        nowTalking = true;

        GameObject nowInteracting = GameManager.Instance.player.curInteractableGameObject;
        Debug.Log(nowInteracting);
        npc = nowInteracting.GetComponent<NPC>();

        npcSO = npc.npcSO;

        OpenDialogue();
        StartCoroutine(PrintDialogue());
    }

    private IEnumerator PrintDialogue()
    {
        titleText.text = sbTitle.Clear().ToString();
        bodyText.text = sbBody.Clear().ToString();

        portrait.sprite = null;

        for (int i = 0; i < npcSO.testDialogue.Length; i++)
        {
            UtilSB.SetText(titleText, sbTitle, npcSO.npcName + " - " + npcSO.state);

            SetImage(portrait, npcSO.illusts[0]);

            if (portrait.sprite == null) portrait.transform.localScale = Vector3.zero;
            else
            {
                portrait.transform.localScale = Vector3.one;
            }

            curPrintLine = TextEffect.Typing(bodyText, sbBody, npcSO.testDialogue[i]);
            yield return StartCoroutine(curPrintLine);

            Debug.Log("E 키로 진행하세요");
            yield return new WaitUntil(() => Input.GetKey(KeyCode.E));

            yield return new WaitForSeconds(1f);

            ClearDialogue();
        }

        CloseDialogue();

        nowTalking = false;

        yield return null;
    }

    private void ClearDialogue()
    {
        UtilSB.ClearText(titleText, sbTitle);
        UtilSB.ClearText(bodyText, sbBody);
        portrait.sprite = null;
    }


    // UI

    public void OpenDialogue()
    {
        dialogueCanvas.SetActive(true);
    }
    public void CloseDialogue()
    {
        dialogueCanvas.SetActive(false);
    }

    public void SetImage(Image image, Sprite sprite)
    {
        image.sprite = sprite;
    }
}