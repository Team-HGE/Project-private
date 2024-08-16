using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPadGimmick_Close_Btn : MonoBehaviour
{

    [SerializeField] KeyPadGimmick keyPadGimmick;

    public void OnCloseBtn()
    {
        keyPadGimmick.CloseBtn();
    }   
}
