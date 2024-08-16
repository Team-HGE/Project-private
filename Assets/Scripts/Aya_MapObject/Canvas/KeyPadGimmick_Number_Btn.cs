using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyPadGimmick_Number_Btn : MonoBehaviour
{
    public Button numBtn;
    [SerializeField] Image keyPadBG;
    [SerializeField] int keyNum;
    [SerializeField] KeyPadGimmick keyPadGimmick;
    public void OnClickNumBtn()
    {
        StartCoroutine(BlinkBG());
        if (keyNum == -1) return;
        keyPadGimmick.AddInteractNumber(keyNum);
    }
    IEnumerator BlinkBG()
    {
        keyPadBG.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        keyPadBG.color = Color.black;
    }
}
