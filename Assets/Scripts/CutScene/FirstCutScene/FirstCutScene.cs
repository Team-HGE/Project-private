using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FirstCutScene : MonoBehaviour
{
    public GameObject TLTrigger;
    // 획득 아이템
    //public GameObject item;
    public bool onTrigger;

    private FirstTLTrigger trigger;

    private void Start()
    {
        trigger = TLTrigger.GetComponent<FirstTLTrigger>();
        trigger.OnEnd += HandleEnd;
        
        //Item.OnGetItem += HandleGetItem;
    }

    private void HandleGetItem()
    {
        if (trigger.IsEnd) TLTrigger.SetActive(true);
    }

    private void HandleEnd()
    {
        if (trigger.IsEnd) Destroy(gameObject);
    }


    // 최적화 수정 예정**
    private void Update()
    {
        if (onTrigger)
        {
            TLTrigger.SetActive(true);

        }
        else
        {
            TLTrigger.SetActive(false);
        }
    }


}
