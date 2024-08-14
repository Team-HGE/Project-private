using System.Collections;
using System.Text;
using UnityEngine;

public class DialogueSetting: MonoBehaviour
{
    public static bool isTalking { get; set; }

    [HideInInspector]
    public UIDialogue ui;

    public StringBuilder sbTitle = new StringBuilder();
    public StringBuilder sbBody = new StringBuilder();
    public IEnumerator curPrintLine;
    public WaitForSeconds waitTime = new WaitForSeconds(1f);
    public WaitUntil waitLeftClick = new WaitUntil(() => Input.GetMouseButtonDown(0));

    public void InitUI()
    {
        ui = GetComponent<UIDialogue>();
    }

    public void InitDialogueSetting()
    {
        isTalking = false;
        //Debug.Log("DialogueSetting 초기화:" + isTalking);
        TextEffect.isSkipped = false;
        ui.AnswerCanvas.SetActive(false);
        ui.ClearDialogue(sbTitle, sbBody);
        ui.CloseDialogue();
    }
}