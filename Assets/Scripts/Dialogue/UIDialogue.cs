using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogue : MonoBehaviour
{
    public GameObject dialogueCanvas;

    public TextMeshProUGUI bodyText;
    public Image titleBG;
    public TextMeshProUGUI titleText;
    public Image portrait;
    public GameObject darkScreen;
    public GameObject FinishStoryBtn;

    public GameObject AnswerCanvas;
    public TextMeshProUGUI answerText1;
    public TextMeshProUGUI answerText2;

    // TODO: 캐릭터 스탠딩 이미지도 받아오기

    public void OpenBG()
    {
        darkScreen.SetActive(true);
        FinishStoryBtn.SetActive(true);
    }

    public void OpenDialogue()
    {
        dialogueCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void CloseDialogue()
    {
        darkScreen.SetActive(false);
        dialogueCanvas.SetActive(false);
        AnswerCanvas.SetActive(false);
        FinishStoryBtn.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //Debug.Log("isTalking : " + DialogueSetting.isTalking);
    }

    public void CheckNullTitle(string speaker)
    {
        if (portrait.sprite == null) portrait.transform.localScale = Vector3.zero;
        else
        {
            portrait.transform.localScale = Vector3.one;
        }

        if (speaker == "") titleBG.transform.localScale = Vector3.zero;
        else
        {
            titleBG.transform.localScale = Vector3.one;
        }
    }

    public void SetImage(Image image, Sprite sprite)
    {
        image.sprite = sprite;
    }

    public void ClearDialogue(StringBuilder _sbTitle, StringBuilder _sbBody)
    {
        titleText.text = _sbTitle.Clear().ToString();
        bodyText.text = _sbBody.Clear().ToString();

        UtilSB.ClearText(titleText, _sbTitle);
        UtilSB.ClearText(bodyText, _sbBody);
        portrait.sprite = null;
    }
}
