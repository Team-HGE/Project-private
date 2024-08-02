using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class KeyPadGimmick : MonoBehaviour
{
    [Header("KeyPadAudioSource")]
    [SerializeField] AudioSource keyPadAudio;

    [Header("Number Image")]
    [SerializeField] Sprite[] imageNumbers;
    [SerializeField] Image[] numberPic_Images;
    [SerializeField] Sprite[] openSprites;
    private Color checkColor = new Color(0.254717f, 0.254717f, 0.254717f, 1);

    [Header("KeyPadGimmick Canvas")]
    [SerializeField] GameObject keyPadGimmickCanvas;

    [Header("Puzzle Gimmick")]
    [SerializeField] List<int> puzzleNumbers;
    [SerializeField] List<int> interactNumbers;

    [SerializeField] KeyPadGimmick_Number_Btn[] keyPadGimmick_Numbers;

    [Header("KeyPadObject")]
    [SerializeField] KeyPadObject keyPadObject;
    
    public void puzzleSetting(int[] puzzlePassworlds, KeyPadObject keyPadObject)
    {
        puzzleNumbers.Clear();
        for (int i = 0; puzzlePassworlds.Length > i; i++)
        {
            puzzleNumbers.Add(puzzlePassworlds[i]);
        }
        foreach (var image in numberPic_Images)
        {
            image.sprite = imageNumbers[0];
        }
        this.keyPadObject = keyPadObject;
        interactNumbers.Clear();
        foreach (var btn in keyPadGimmick_Numbers)
        {
            btn.numBtn.enabled = true;
        }
    }
    public void AddInteractNumber(int num)
    {
        if (interactNumbers.Count >= puzzleNumbers.Count) return;
        interactNumbers.Add(num);
        numberPic_Images[interactNumbers.Count - 1].sprite = imageNumbers[num];
        if (interactNumbers.Count >= puzzleNumbers.Count)
        {
            foreach (var btn in keyPadGimmick_Numbers)
            {
                btn.numBtn.enabled = false;
            }
            ConfirmNum();
        }
    }
    public void ConfirmNum()
    {
        bool isNumber_Mismatch = false;
        for (int i = 0; puzzleNumbers.Count > i; i++)
        {
            if (puzzleNumbers[i] != interactNumbers[i])
            {
                Debug.Log($"{puzzleNumbers[i]}/{interactNumbers[i]}");
                isNumber_Mismatch = true;
                break;
            }
        }

        Sequence sequence = DOTween.Sequence();
        foreach (var image in numberPic_Images)
        {
            sequence.Append(image.DOColor(checkColor, 0.2f));
            sequence.AppendInterval(0.1f); // 대기시간
            sequence.Append(image.DOColor(Color.white, 0.2f));
        }
        if (isNumber_Mismatch)
        {
            sequence.OnComplete(OnFail);
        }
        else
        {
            sequence.AppendCallback(SetSuccess);
            float targetTime = sequence.Duration();
            for (int i = 0; i < 2 ; i++)
            {
                targetTime += i;
                foreach (var image in numberPic_Images)
                {
                    sequence.Insert(targetTime ,image.DOColor(checkColor, 0.2f));
                    sequence.Insert(targetTime + 0.5f ,image.DOColor(Color.white, 0.2f));
                }
            }
            sequence.AppendInterval(0.5f);
            sequence.OnComplete(keyPadObject.GimmickSuccess);
        }
    }
    void SetSuccess()
    {
        for (int i = 0; i < numberPic_Images.Length; i++)
        {
            numberPic_Images[i].sprite = openSprites[i];
        }
    }
    void OnFail()
    {
        interactNumbers.Clear();
        foreach (var image in numberPic_Images)
        {
            image.sprite = imageNumbers[0];
        }
        foreach (var btn in keyPadGimmick_Numbers)
        {
            btn.numBtn.enabled = true;
        }
    }
}
