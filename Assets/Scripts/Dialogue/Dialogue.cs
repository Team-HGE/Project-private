using UnityEngine;
using UnityEngine.UI;
using System.Text;
using TMPro;
using System.Collections;

public class Dialogue : MonoBehaviour
{
    public GameObject dialogueCanvas;
    public Image titleBG;

    public TextMeshProUGUI titleText;
    public TextMeshProUGUI bodyText;
    public Image portrait;

    private NPC npc;
    private NPC_SO npcSO;
    public StringBuilder sbTitle = new StringBuilder();
    public StringBuilder sbBody = new StringBuilder();

    public IEnumerator curPrintLine;
    public bool nowTalking = false;

    public void Init()
    {
        CloseDialogue();
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

        OpenDialogue();
        StartCoroutine(PrintDialogue());
    }

    public IEnumerator PrintDialogue()
    {
        titleText.text = sbTitle.Clear().ToString();
        bodyText.text = sbBody.Clear().ToString();

        portrait.sprite = null;

        for (int i = 0; i < npcSO.testDialogue.Length; i++)
        {
            UtilSB.SetText(titleText, sbTitle, npcSO.npcName + " - " + npc.ChangeNpcState(npcState.Speaking));

            SetImage(portrait, npc.SwitchPortrait(npcSO.emotion));

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
        npc.ChangeNpcState(npcState.Idle);

        yield return null;
    }

    public void ClearDialogue()
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