using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyPadGimmick_Number_Btn : MonoBehaviour
{
    public Button numBtn;
    [SerializeField] Image keyPadBG;
    [SerializeField] int keyNum;
    public void OnClickNumBtn()
    {
        StartCoroutine(BlinkBG());
    }
    IEnumerator BlinkBG()
    {
        keyPadBG.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        keyPadBG.color = Color.black;
    }
}
