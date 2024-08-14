using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEventCollider : MonoBehaviour
{
    [SerializeField] GameObject eventObject;
    [SerializeField] bool isTrigger;
    [SerializeField] float delayTime = 0;

    [SerializeField] BreakableWindow[] glass;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isTrigger)
        {
            isTrigger = true;

            int rand = Random.RandomRange(0, 100);

            if (rand > 100)
            {
                eventObject.SetActive(true);
            }
            else
            {
                foreach (var c in glass)
                {
                    if (c.isBroken || c == null)
                    {
                        continue;
                    }
                    else
                    {
                        c.breakWindow();
                        break;
                    }
                }
            }

            StartCoroutine(EventDelay());
        }
    }

    IEnumerator EventDelay()
    {
        while (delayTime < 60)
        {
            delayTime += Time.deltaTime;
            yield return null;
        }

        isTrigger = false;
        delayTime = 0;
    }
}
