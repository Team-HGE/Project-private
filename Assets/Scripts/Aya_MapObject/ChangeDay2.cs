using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDay2 : MonoBehaviour
{
    [SerializeField] ScriptSO scriptSO;

    private void OnTriggerEnter(Collider other)
    {
        DialogueManager.Instance.script.InitScript(scriptSO);
        DialogueManager.Instance.script.StartScript();
    }
}