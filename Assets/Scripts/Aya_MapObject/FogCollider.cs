using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (RenderSettings.fog == true)
            {
                RenderSettings.fog = false;
            }
            else
            {
                RenderSettings.fog = true;
            }
        }
    }
}
