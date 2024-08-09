using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FirstCutScene : MonoBehaviour
{
    public GameObject TLTrigger;
    public bool onTrigger;

    private FirstTLTrigger isEnd;

    private void Start()
    {
        isEnd = TLTrigger.GetComponent<FirstTLTrigger>();
    }

    private void Update()
    {
        if (onTrigger)
        {
            TLTrigger.SetActive(true);

            //if (isEnd != null) 
            //{
            //    if (isEnd.isEnd)
            //    {
            //        Destroy(gameObject);
            //        return;
            //    }
            //}
        }
        else
        {
            TLTrigger.SetActive(false);
        }
    }


}
