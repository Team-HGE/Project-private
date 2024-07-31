using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyPadGimmick : MonoBehaviour
{
    [SerializeField] AudioSource keyPadAudio;
    [SerializeField] Sprite[] imageNumbers;
    [SerializeField] Image[] numberPic_Images;
    [SerializeField] int[] puzzleNumbers;
    [SerializeField] List<int> interactNumbers;

    [SerializeField] KeyPadGimmick_Number_Btn[] keyPadGimmick_Numbers;
    [SerializeField] bool isNumber_Mismatch;
    public void AddInteractNumber(int num)
    {
        interactNumbers.Add(num);
        numberPic_Images[interactNumbers.Count].sprite = imageNumbers[num];
        if (interactNumbers.Count >= puzzleNumbers.Length)
        {
            foreach (var btn in keyPadGimmick_Numbers)
            {
                btn.numBtn.enabled = false;
                ConfirmNum();
            }
        }
    }
    public void ConfirmNum()
    {
        for (int i = 0; puzzleNumbers.Length > i; i++)
        {
            if (puzzleNumbers[i] != interactNumbers[i])
            {
                isNumber_Mismatch = false;
            }
        }

        if (isNumber_Mismatch)
        {

        }
        else
        {

        }
    }
}
