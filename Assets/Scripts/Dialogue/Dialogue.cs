using UnityEngine;
using UnityEngine.UI;
using System.Text;
using TMPro;
using UnityEngine.Diagnostics;
using System.Collections;

public class Dialogue : MonoBehaviour
{
    public DialogueSO dialogueSO;

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

    private void InitSOData(DialogueSO _dialogue)
    {
        dialogueSO = _dialogue;
    }

    public void StartDialogue()
    {
        if (nowTalking) return;
        nowTalking = true;

        Debug.Log("NPC와 대화 시작");

        OpenDialogue();
        InitSOData(dialogueSO);
        StartCoroutine(PrintDialogue());
    }

    private IEnumerator PrintDialogue()
    {
        titleText.text = sbTitle.Clear().ToString();
        bodyText.text = sbBody.Clear().ToString();

        portrait.sprite = null;

        for(int i = 0; i < dialogueSO.bodyTexts.Length; i++)
        {
            UtilSB.SetText(titleText, sbTitle, dialogueSO.speakers[0]);

            SetImage(portrait, dialogueSO.images[0]);

            if (portrait.sprite == null) portrait.transform.localScale = Vector3.zero;
            else 
            { 
                portrait.transform.localScale = Vector3.one;
            }

            curPrintLine = TextEffect.Typing(bodyText, sbBody, dialogueSO.bodyTexts[i]);
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