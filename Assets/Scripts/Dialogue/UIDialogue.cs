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

    public void OpenDS()
    {
        darkScreen.SetActive(true);
    }
    public void CloseDS()
    {
        darkScreen.SetActive(false);
    }

    public void OpenDialogue()
    {
        dialogueCanvas.SetActive(true);
    }
    public void CloseDialogue()
    {
        dialogueCanvas.SetActive(false);
    }

    public void CheckSpeakerNull(string speaker)
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
}
