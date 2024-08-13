using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEventCollider : MonoBehaviour
{
    [SerializeField] GameObject eventObject;
    [SerializeField] bool isTrigger;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isTrigger)
        {
            isTrigger = true;
            eventObject.SetActive(true);
        }
    }
}
