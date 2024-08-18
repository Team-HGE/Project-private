using System;
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

    // 팝 스탠딩 관련 변수
    public GameObject standingImgLayout;

    private ObjectPool objectPool;
    public GameObject standingObj;
    public RectTransform standingTransform;
    private Image standingImg;
    private Image standingImg2;
    private Color originColor;
    private Color fadeColor;
    private int standingCnt = 0;
    private bool firstEncounter = true;

    public event Action playEvent;

    // 리뉴얼 스탠딩 출력 관련 변수



    public void OpenBG()
    {
        darkScreen.SetActive(true);
        finishStoryBtn.SetActive(true);
    }

    public void ChangeBG(Sprite sprite)
    {
        darkScreen.SetActive(true);
        Image Img = darkScreen.GetComponent<Image>();
        if (sprite != null)
        {
            Img.sprite = sprite;
        }
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

        playEvent?.Invoke();
        playEvent = null;
        if (GameManager.Instance.PlayerStateMachine != null)
        {
            GameManager.Instance.PlayerStateMachine.Player.PlayerControllOn();
        }
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

    // 이하 스탠딩 관련 메소드

    public void ObjectPoolInit()
    {
        objectPool = standingImgLayout.GetComponent<ObjectPool>();
    }

    public void CheckEncounter(string[] speakers, int idx, string speaker)
    {
        if (speaker == "" || !firstEncounter) return;

        for (int i = 0; i <= idx; i++)
        {
            if (speakers[i] == speaker) // idx나 idx 전에 등장한 적 있으면
            {
                if(i == idx)
                {
                    Debug.Log(speaker + " 첫 등장입니다.");
                    firstEncounter = true;
                    break;
                }
                else
                {
                    //Debug.Log("firstEnounter false");
                    firstEncounter = false;
                    break;
                }
            }
        }
        // 전에 등장한 적 없으면
        //Debug.Log(speaker + " 첫 등장입니다.2");
        //firstEncounter = false;
    }

    public void PopStanding(Sprite sprite)
    {
        if (sprite == null) return;

        // 첫 등장일 경우
        // 프리팹 활성화, 이미지 넣기
        if (firstEncounter)
        {
            standingObj = objectPool.GetObject();
            standingTransform = standingObj.GetComponent<RectTransform>();
            standingImg = standingObj.GetComponent<Image>();
            standingImg.sprite = sprite;
        }
        else
        {
            GameObject Obj = objectPool.ReturnObjectby(sprite);
            //Debug.Log(Obj);

            if (Obj == null)
            {
                standingObj = objectPool.GetObject();
                standingTransform = standingObj.GetComponent<RectTransform>();
                standingImg = standingObj.GetComponent<Image>();
                standingImg.sprite = sprite;
                standingCnt++;
            }
            else 
            {
                standingTransform = Obj.GetComponent<RectTransform>();
                standingImg = Obj.GetComponent<Image>();
            }
        }

        //standingTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 900);
        standingTransform.sizeDelta = new Vector2(900, 900);
        standingImg.preserveAspect = true;

        //originColor = standingImg.color;

        // 이미지 오퍼시티 100%
        //originColor.a = 10.0f;
        //standingImg.color = originColor;
        standingImg.color = new Color32(255, 255, 255, 255);

        //if (standingCnt > 0 && standingImg2 == null)
        //{
        //    standingImg2 = objectPool.ReturnByIndex(0).GetComponent<Image>();
        //    standingImg2.color = new Color32(255, 255, 255, 255);
        //}
        //else if (standingImg2 != null) 
        //    standingImg2.color = new Color32(255, 255, 255, 255);

        //Debug.Log("투명도 255");
    }

    public void FadeStanding(Sprite sprite)
    {
        //if (sprite == null) { return; }

        //objectPool.FadeColor(standingImg);

        //if (standingImg2 != null)
        //    objectPool.FadeColor(standingImg2);

        // 본인 대사 출력 끝나면 이미지 오피시티 10%

        //fadeColor = standingImg.color;
        //fadeColor.a = 0.5f;
        //standingImg.color = fadeColor;
        //standingImg.color = new Color32(255, 255, 255, 100);
        //Debug.Log("투명도 100");
    }

    public void DestroyStanding()
    {
        // 캐릭터가 퇴장하면 해당 프리펩 비활성화
        //objectPool.ReturnObject(standingObj);

        if (standingObj == null) return;

        // 모든 오브젝트 Sprite 초기화
        objectPool.SpriteInit();

        // 대화가 끝나면 프리펩 비활성화
        objectPool.ReturnAllObject();
    }
}
