using System.Runtime.InteropServices.WindowsRuntime;
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
    public GameObject finishStoryBtn;

    public GameObject AnswerCanvas;
    public TextMeshProUGUI answerText1;
    public TextMeshProUGUI answerText2;

    // �� ���ĵ� ���� ����
    public GameObject standingImgLayout;

    private ObjectPool objectPool;
    private GameObject standingObj;
    private Image standingImg;
    private Color originColor;
    private bool firstEncounter;

    public void OpenBG()
    {
        darkScreen.SetActive(true);
        finishStoryBtn.SetActive(true);
    }

    public void OpenDialogue()
    {
        dialogueCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameManager.Instance.PlayerStateMachine.Player.PlayerControllOff();
    }
    public void CloseDialogue()
    {
        darkScreen.SetActive(false);
        dialogueCanvas.SetActive(false);
        AnswerCanvas.SetActive(false);
        finishStoryBtn.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GameManager.Instance.PlayerStateMachine.Player.PlayerControllOn();
        //Debug.Log("isTalking : " + DialogueSetting.isTalking);
    }

    public void CheckNullIndex(string speaker)
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

    public void SetPortrait(Image image, Sprite sprite)
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

    // ���� ���ĵ� ���� �޼ҵ�

    public void ObjectPoolInit()
    {
        objectPool = standingImgLayout.GetComponent<ObjectPool>();
    }

    public void CheckEncounter(string[] speakers, int idx, string speaker)
    {
        if (firstEncounter)
            return;

        for (int i = 0; i < idx; i++)
        {
            if (speakers[i] == speaker)
            {
                firstEncounter = false;
                return;
            }

            Debug.Log(speaker + "ù �����Դϴ�.");
            firstEncounter = true;
        }
    }

    public void PopStanding(Sprite sprite)
    {
        if (sprite == null) { return; }

        // ù ������ ���
        // ������ Ȱ��ȭ, �̹��� �ֱ�
        if (firstEncounter)
        {
            standingObj = objectPool.GetObject();
            standingImg = standingObj.GetComponent<Image>();
        }
        else
        {
            GameObject Obj = objectPool.ReturnObjectby(sprite);
            standingImg = Obj.GetComponent<Image>();
        }

        originColor = standingImg.color;
        standingImg.sprite = sprite;
        standingImg.preserveAspect = true;

        // �̹��� ���۽�Ƽ 100%
        originColor.a = 1.0f;
        standingImg.color = originColor;
    }

    public void FadeStanding(Sprite sprite)
    {
        // ���� ��� ��� ������ �̹��� ���ǽ�Ƽ 10%
        originColor.a = 0.1f;
        standingImg.color = originColor;
    }

    public void DestroyStanding()
    {
        // ĳ���Ͱ� �����ϸ� �ش� ������ ��Ȱ��ȭ
        //objectPool.ReturnObject(standingObj);

        // ��ȭ�� ������ ������ ��Ȱ��ȭ
        objectPool.ReturnAllObject();
    }
}
