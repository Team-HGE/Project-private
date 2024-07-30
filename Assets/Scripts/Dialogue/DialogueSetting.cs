using System.Collections;
using System.Text;
using UnityEngine;

public class DialogueSetting: MonoBehaviour
{
    public bool isTalking { get; set; }
    [HideInInspector]
    public float printSpeed = 0.5f;
    [HideInInspector]
    public UIDialogue ui;

    public StringBuilder sbTitle = new StringBuilder();
    public StringBuilder sbBody = new StringBuilder();
    public IEnumerator curPrintLine;
    public WaitForSeconds waitTime = new WaitForSeconds(1f);
    public WaitUntil waitLeftClick = new WaitUntil(() => Input.GetMouseButtonDown(0));

    public void Init()
    {
        isTalking = false;
        ui = GetComponent<UIDialogue>();
    }
}
